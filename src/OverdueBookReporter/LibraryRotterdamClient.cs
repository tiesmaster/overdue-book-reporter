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
        if (_isLoggedIn is false)
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
            $"https://rotterdam.hostedwise.nl//cgi-bin/bx.pl?event=invent;prt=INTERNET;var=opac;taal=nl_NL;vestnr=1012;" +
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
        public string AccessToken { get; set; }
    }

    private async Task<Result> LoginAsync()
    {
        _logger.LogDebug("Logging into kb.nl");
        var credentials = _clientOptions.Login;

        var response = await _httpClient.PostAsJsonAsync(
            "https://login.kb.nl/si/login/api/authenticate/",
            KbLibraryAuthenticationRequest.From(credentials));

        if (!response.IsSuccessStatusCode)
        {
            return Result.Fail($"{response.StatusCode}: {await response.Content.ReadAsStringAsync()}");
        }

        // fire up cookies for wise.oclc.org/realms/rotterdam
        var response2 = await _httpClient.GetAsync("https://iam-emea.wise.oclc.org/realms/rotterdam/protocol/openid-connect/auth?client_id=external-login-wise-cms-i010&scope=openid%20patron-actions%20registration&response_type=code&redirect_uri=https%3A%2F%2Fwww.bibliotheek.rotterdam.nl%2Findex.php%3Foption%3Dcom_oclcwise%26task%3Dopenauth.login");

        // do the actual authorization start, and receive the code
        var response3 = await _httpClient.GetAsync("https://iam-emea.wise.oclc.org/realms/rotterdam/protocol/openid-connect/auth?client_id=opac-via-external-idp&redirect_uri=https%3A%2F%2Fwww.bibliotheek.rotterdam.nl%2Fwise-apps%2Fopac%2F1099%2Fmy-account&response_mode=fragment&response_type=code&scope=openid%20patron-actions%20registration&prompt=none");

        var thingContainingCode = response3.RequestMessage.RequestUri.Fragment;
        var code = thingContainingCode[(thingContainingCode.IndexOf("code=") + 5)..];

        // chip in the code
        var tokenPost = new StringContent($"code={code}&grant_type=authorization_code&client_id=opac-via-external-idp&redirect_uri=https%3A%2F%2Fwww.bibliotheek.rotterdam.nl%2Fwise-apps%2Fopac%2F1099%2Fmy-account");
        tokenPost.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

        var tokenResponse = await _httpClient.PostAsync("https://iam-emea.wise.oclc.org/realms/rotterdam/protocol/openid-connect/token", tokenPost);
        var accessToken = (await tokenResponse.Content.ReadFromJsonAsync<TokenResponse>())!.AccessToken;

        // get the session
        var sessionRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://rotterdam.hostedwise.nl/cgi-bin/bx.pl?event=syncses;prt=INTERNET"),
        };

        sessionRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var sessionResponse = await _httpClient.SendAsync(sessionRequestMessage);
        var session = await sessionResponse.Content.ReadFromJsonAsync<Session>();
        _sid = session.SessionId;

        //var dict = new Dictionary<string, string>
        //{
        //    { "username", credentials.Username },
        //    { "password", credentials.Password },
        //    { "return", loginFormSecurityTokens!.ReturnToken },
        //    { loginFormSecurityTokens!.CsrfToken, "1" },
        //};

        //var body = new FormUrlEncodedContent(dict);
        //var ms = new MemoryStream();
        //await body.CopyToAsync(ms);
        //_cookieJar.AddCookies(body.Headers);

        //_logger.LogTrace("Form content: {FormContent}", Encoding.UTF8.GetString(ms.ToArray()));

        //var loginResponse = await _httpClient.PostAsync("https://www.bibliotheek.rotterdam.nl/login?task=user.login", body);

        //_logger.LogTrace("Received status code: {StatusCode}", loginResponse.StatusCode);
        //_logger.LogTrace("Received HTML: {Html}", await loginResponse.Content.ReadAsStringAsync());

        //_logger.LogTrace("Response headers: {ResponseHeaders}", FlattenHeaderValues(loginResponse.Headers));
        //_logger.LogTrace("Content headers: {ContentHeaders}", FlattenHeaderValues(loginResponse.Content.Headers));

        //_cookieJar.ReadCookieValues(loginResponse);
        //_cookieJar.AddCookies(_httpClient.DefaultRequestHeaders);

        //var redirectResponse = await _httpClient.GetAsync(loginResponse.Headers.Location);
        //_cookieJar.ReadCookieValues(redirectResponse);

        return Result.Ok();
    }

    private static Dictionary<string, string> FlattenHeaderValues(
        IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
    {
        return new(headers.Select(kvp => new KeyValuePair<string, string>(
            kvp.Key,
            string.Join(" ", kvp.Value))));
    }

    private record KbLibraryAuthenticationRequest(KbLibraryAuthenticationCredentials Definition, string Module = "UsernameAndPassword")
    {
        public static KbLibraryAuthenticationRequest From(LibraryRotterdamClientCredentials credentials)
            => new(new KbLibraryAuthenticationCredentials(credentials.Username, credentials.Password + "123"));
    }

    private record KbLibraryAuthenticationCredentials(string Username, string Password, bool RememberMe = false);
}

public class LibraryRotterdamClientOptions
{
    public const string SectionName = "LibraryRotterdamClient";

    public LibraryRotterdamClientCredentials Login { get; set; } = null!;
    public string? UserAgent { get; set; }
}

public class LibraryRotterdamClientCredentials
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}