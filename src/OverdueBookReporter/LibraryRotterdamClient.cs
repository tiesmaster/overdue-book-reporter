// Ignore Spelling: Username prt opac taal vestnr sid html bibrott www openid idp

using System.Collections.Immutable;
using System.Net.Http.Json;

using IdentityModel;
using IdentityModel.Client;

using Microsoft.Extensions.Options;

namespace Tiesmaster.OverdueBookReporter;

public class LibraryRotterdamClient
{
    private const string UserAgentHeaderName = "User-Agent";

    private readonly LibraryRotterdamClientOptions _clientOptions;
    private readonly HttpClient _httpClient;
    private readonly ILogger<LibraryRotterdamClient> _logger;

    private bool _isLoggedIn;
    private string? _sid;

    public LibraryRotterdamClient(
        HttpClient httpClient,
        IOptions<LibraryRotterdamClientOptions> clientOptions,
        ILogger<LibraryRotterdamClient> logger)
    {
        _clientOptions = clientOptions.Value;
        _httpClient = httpClient;
        _logger = logger;

        if (_clientOptions.UserAgent is string ua && ua is { Length: > 0 })
        {
            _httpClient.DefaultRequestHeaders.Add(UserAgentHeaderName, ua);
        }
    }

    public async Task<Result<BooksStatusReport>> GetBooksStatusReportAsync(DateOnly today)
    {
        var loanedBooksResult = await GetBookListingAsync();
        if (loanedBooksResult.IsSuccess)
        {
            return new BooksStatusReport(today, _clientOptions.Login.Username, loanedBooksResult.Value.ToImmutableList());
        }
        else
        {
            return loanedBooksResult.ToResult();
        }
    }

    public async Task<Result<IEnumerable<LoanedBook>>> GetBookListingAsync()
    {
        if (!_isLoggedIn)
        {
            _logger.LogDebug("Not logged in yet; starting log in process");
            var loginResult = await LoginAsync();
            if (loginResult.IsFailed)
            {
                return loginResult.ToResult<IEnumerable<LoanedBook>>();
            }

            _isLoggedIn = true;
        }

        _logger.LogDebug("Retrieving book listing");
        var response = await _httpClient.GetAsync(
            "https://rotterdam.hostedwise.nl//cgi-bin/bx.pl?event=invent;prt=INTERNET;var=opac;taal=nl_NL;vestnr=1012;" +
            $"sid={_sid}");

        var content = await response.Content.ReadAsStringAsync();

        _logger.LogTrace("Received HTML: {Html}", content);

        return await LibraryHtmlParser.ParseBookListingAsync(content);
    }

    private async Task<Result> LoginAsync()
    {
        // The login process consists of logging into 2 separate authorization servers: the kb.nl
        // one, and the oclc.org one. The former is the upstream one for the latter. The flow is
        // like this: bibrott [resource owner] -> oclc.org [downstream AS] -> kb.nl [upstream AS]
        // We need access to the RO bibrott, but for that to get access, we need to do the
        // authorization code flow to oclc.org, but that forwards up to kb.nl.

        // So to get access, we first login to kb.nl with a JSON API. Next, we start the
        // authorization code flow to oclc.org, and that returns a proper token, that we can chip
        // in for an access token.

        var result = await LoginToKbAsync();
        if (result.IsFailed)
        {
            return result;
        }

        result = await DoDummyAuthorizeRequestCallAsync();
        if (result.IsFailed)
        {
            return result;
        }

        var accessTokenResult = await GetAccessTokenAsync();
        if (accessTokenResult.IsFailed)
        {
            return accessTokenResult.ToResult();
        }

        var accessToken = accessTokenResult.Value;

        var sessionResult = await StartSessionAsync(accessToken);
        if (sessionResult.IsFailed)
        {
            return sessionResult.ToResult();
        }

        _sid = sessionResult.Value;

        return Result.Ok();
    }

    private async Task<Result> LoginToKbAsync()
    {
        var credentials = _clientOptions.Login;

        var kbLoginResponse = await _httpClient.PostAsJsonAsync(
            "https://login.kb.nl/si/login/api/authenticate/",
            KbLibraryAuthenticationRequest.From(credentials));

        if (!kbLoginResponse.IsSuccessStatusCode)
        {
            return Result.Fail($"KB login failure: {kbLoginResponse.StatusCode}: {await kbLoginResponse.Content.ReadAsStringAsync()}");
        }

        return Result.Ok();
    }

    private async Task<Result> DoDummyAuthorizeRequestCallAsync()
    {
        // Invoke dummy Authorization Request that initiates the authorize process normally.
        // Needed, as that sets the authorization session cookies by KeyCloak
        var dummyAuthorizationUrl = new RequestUrl("https://iam-emea.wise.oclc.org/realms/rotterdam/protocol/openid-connect/auth")
            .CreateAuthorizeUrl(
                clientId: "external-login-wise-cms-i010",
                responseType: OidcConstants.ResponseTypes.Code,
                scope: "openid patron-actions registration",
                redirectUri: "https://www.bibliotheek.rotterdam.nl/index.php?option=com_oclcwise&task=openauth.login");

        var dummyResponse = await _httpClient.GetAsync(dummyAuthorizationUrl);

        if (!dummyResponse.IsSuccessStatusCode)
        {
            return Result.Fail($"Dummy Authorize Request (dummy) failure: {dummyResponse.StatusCode}: {await dummyResponse.Content.ReadAsStringAsync()}");
        }

        return Result.Ok();
    }

    private async Task<Result<string>> GetAccessTokenAsync()
    {
        // Do the actual Authorization Request, and receive the Authorization Code
        var clientId = "opac-via-external-idp";
        var redirectUri = "https://www.bibliotheek.rotterdam.nl/wise-apps/opac/1099/my-account";

        var authorizationUrl = new RequestUrl("https://iam-emea.wise.oclc.org/realms/rotterdam/protocol/openid-connect/auth")
            .CreateAuthorizeUrl(
                clientId: clientId,
                responseType: OidcConstants.ResponseTypes.Code,
                scope: "openid patron-actions registration",
                redirectUri: redirectUri,
                prompt: OidcConstants.PromptModes.None,
                responseMode: OidcConstants.ResponseModes.Fragment);

        var authorizeResponse = await _httpClient.GetAsync(authorizationUrl);

        if (!authorizeResponse.IsSuccessStatusCode)
        {
            return Result.Fail($"Authorize Request (actual) failure: {authorizeResponse.StatusCode}: {await authorizeResponse.Content.ReadAsStringAsync()}");
        }

        var code = ParseCodeFromUrl(authorizeResponse);

        // Hit the token endpoint, and do the Access Token Request
        var tokenResponse = await _httpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
        {
            Address = "https://iam-emea.wise.oclc.org/realms/rotterdam/protocol/openid-connect/token",
            ClientId = clientId,
            Code = code,
            RedirectUri = redirectUri,
        });

        if (tokenResponse.IsError)
        {
            var tokenError = new Error($"Access Token Request failure: {tokenResponse.ErrorType}: {tokenResponse.Error}: {tokenResponse.ErrorDescription}");
            if (tokenResponse.Exception is Exception ex)
            {
                tokenError.CausedBy(ex);
            }

            return Result.Fail(tokenError);
        }

        return Result.Ok(tokenResponse.AccessToken!);
    }

    private async Task<Result<string>> StartSessionAsync(string accessToken)
    {
        // Start a new session with the access token
        _httpClient.SetBearerToken(accessToken);

        var sessionResponse = await _httpClient.GetAsync("https://rotterdam.hostedwise.nl/cgi-bin/bx.pl?event=syncses;prt=INTERNET");
        if (!sessionResponse.IsSuccessStatusCode)
        {
            return Result.Fail($"Start session failure: {sessionResponse.StatusCode}: {await sessionResponse.Content.ReadAsStringAsync()}");
        }

        var session = await sessionResponse.Content.ReadFromJsonAsync<Session>();

        return Result.Ok(session!.SessionId);
    }

    private static string ParseCodeFromUrl(HttpResponseMessage authorizeResponse)
    {
        var thingContainingCode = authorizeResponse.RequestMessage!.RequestUri!.Fragment;
        var code = thingContainingCode[(thingContainingCode.IndexOf("code=") + 5)..];
        return code;
    }

    private record KbLibraryAuthenticationRequest(KbLibraryAuthenticationCredentials Definition, string Module = "UsernameAndPassword")
    {
        public static KbLibraryAuthenticationRequest From(LibraryRotterdamClientCredentials credentials)
            => new(new KbLibraryAuthenticationCredentials(credentials.Username, credentials.Password));
    }

    private record KbLibraryAuthenticationCredentials(string Username, string Password, bool RememberMe = false);

    private class Session
    {
        public required string SessionId { get; set; }
    }
}

public class LibraryRotterdamClientOptions
{
    public const string SectionName = "LibraryRotterdamClient";

    public required LibraryRotterdamClientCredentials Login { get; set; }
    public string? UserAgent { get; set; }
}

public class LibraryRotterdamClientCredentials
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}