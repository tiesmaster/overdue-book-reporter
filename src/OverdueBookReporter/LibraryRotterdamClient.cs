// Ignore Spelling: Username prt opac taal vestnr sid html bibrott www https apps urlencoded

using System.Collections.Immutable;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Options;

using static Tiesmaster.OverdueBookReporter.LibraryRotterdamClient;

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

    public class Session
    {
        public required string SessionId { get; set; }
    }

    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public required string AccessToken { get; set; }
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

        _logger.LogDebug("Logging into kb.nl");
        var credentials = _clientOptions.Login;

        var kbLoginResponse = await _httpClient.PostAsJsonAsync(
            "https://login.kb.nl/si/login/api/authenticate/",
            KbLibraryAuthenticationRequest.From(credentials));

        if (!kbLoginResponse.IsSuccessStatusCode)
        {
            return Result.Fail($"KB login failure: {kbLoginResponse.StatusCode}: {await kbLoginResponse.Content.ReadAsStringAsync()}");
        }

        // Invoke dummy Authorization Request that initiates the authorize process normally. Needed, as that sets the authorization session cookies by KeyCloak
        var dummyResponse = await _httpClient.GetAsync("https://iam-emea.wise.oclc.org/realms/rotterdam/protocol/openid-connect/auth?client_id=external-login-wise-cms-i010&scope=openid%20patron-actions%20registration&response_type=code&redirect_uri=https%3A%2F%2Fwww.bibliotheek.rotterdam.nl%2Findex.php%3Foption%3Dcom_oclcwise%26task%3Dopenauth.login");

        if (!dummyResponse.IsSuccessStatusCode)
        {
            return Result.Fail($"Dummy Authorize Request (dummy) failure: {dummyResponse.StatusCode}: {await dummyResponse.Content.ReadAsStringAsync()}");
        }

        // Do the actual Authorization Request, and receive the Authorization Code
        var authorizeResponse = await _httpClient.GetAsync("https://iam-emea.wise.oclc.org/realms/rotterdam/protocol/openid-connect/auth?client_id=opac-via-external-idp&redirect_uri=https%3A%2F%2Fwww.bibliotheek.rotterdam.nl%2Fwise-apps%2Fopac%2F1099%2Fmy-account&response_mode=fragment&response_type=code&scope=openid%20patron-actions%20registration&prompt=none");

        if (!authorizeResponse.IsSuccessStatusCode)
        {
            return Result.Fail($"Authorize Request (actual) failure: {authorizeResponse.StatusCode}: {await authorizeResponse.Content.ReadAsStringAsync()}");
        }

        var thingContainingCode = authorizeResponse.RequestMessage!.RequestUri!.Fragment;
        var code = thingContainingCode[(thingContainingCode.IndexOf("code=") + 5)..];

        // Hit the token endpoint, and do the Access Token Request
        var tokenPost = new StringContent($"code={code}&grant_type=authorization_code&client_id=opac-via-external-idp&redirect_uri=https%3A%2F%2Fwww.bibliotheek.rotterdam.nl%2Fwise-apps%2Fopac%2F1099%2Fmy-account");
        tokenPost.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

        var tokenResponse = await _httpClient.PostAsync("https://iam-emea.wise.oclc.org/realms/rotterdam/protocol/openid-connect/token", tokenPost);

        if (!tokenResponse.IsSuccessStatusCode)
        {
            return Result.Fail($"Access Token Request failure: {tokenResponse.StatusCode}: {await tokenResponse.Content.ReadAsStringAsync()}");
        }

        var accessToken = (await tokenResponse.Content.ReadFromJsonAsync<TokenResponse>())!.AccessToken;

        // Start a new session with the access token
        var sessionRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://rotterdam.hostedwise.nl/cgi-bin/bx.pl?event=syncses;prt=INTERNET"),
        };

        sessionRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var sessionResponse = await _httpClient.SendAsync(sessionRequestMessage);
        var session = await sessionResponse.Content.ReadFromJsonAsync<Session>();
        _sid = session!.SessionId;

        return Result.Ok();
    }

    private record KbLibraryAuthenticationRequest(KbLibraryAuthenticationCredentials Definition, string Module = "UsernameAndPassword")
    {
        public static KbLibraryAuthenticationRequest From(LibraryRotterdamClientCredentials credentials)
            => new(new KbLibraryAuthenticationCredentials(credentials.Username, credentials.Password));
    }

    private record KbLibraryAuthenticationCredentials(string Username, string Password, bool RememberMe = false);
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