using System.Collections.Immutable;
using System.Text;

using Microsoft.Extensions.Options;

namespace Tiesmaster.OverdueBookReporter;

public class LibraryRotterdamClient
{
    private readonly LibraryLoginCredentials _credentials;
    private readonly HttpClient _httpClient;
    private readonly CookieJar _cookieJar;
    private readonly ILogger<LibraryRotterdamClient> _logger;

    private bool _isLoggedIn;

    public LibraryRotterdamClient(
        HttpClient httpClient,
        CookieJar cookieJar,
        IOptions<LibraryLoginCredentials> credentials,
        ILogger<LibraryRotterdamClient> logger)
    {
        _credentials = credentials.Value;
        _httpClient = httpClient;
        _cookieJar = cookieJar;
        _logger = logger;
    }

    public async Task<Result<BooksStatusReport>> GetBooksStatusReportAsync(DateOnly today)
    {
        var loanedBooksResult = await GetBookListingAsync();
        if (loanedBooksResult.IsSuccess)
        {
            return new BooksStatusReport(today, _credentials.Username, loanedBooksResult.Value.ToImmutableList());
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
            $"https://wise-web.bibliotheek.rotterdam.nl//cgi-bin/bx.pl?event=invent;var=frame;" +
            $"ssoid={_cookieJar.SsoId}&ssokey=joomla&sid={_cookieJar.BicatSid}");

        var content = await response.Content.ReadAsStringAsync();

        _logger.LogTrace("Received HTML: {Html}", content);

        return await LibraryHtmlParser.ParseBookListingAsync(content);
    }

    private async Task<Result> LoginAsync()
    {
        var result = await StartSessionAsync();
        if (result.IsFailed)
        {
            return result.ToResult().WithError("Login failed");
        }

        var loginFormSecurityTokens = result.Value;

        _logger.LogDebug("Logging in");
        var dict = new Dictionary<string, string>
        {
            { "username", _credentials.Username },
            { "password", _credentials.Password },
            { "return", loginFormSecurityTokens!.ReturnToken },
            { loginFormSecurityTokens!.CsrfToken, "1" },
        };

        var body = new FormUrlEncodedContent(dict);
        var ms = new MemoryStream();
        await body.CopyToAsync(ms);
        _cookieJar.AddCookies(body.Headers);

        _logger.LogTrace("Form content: {FormContent}", Encoding.UTF8.GetString(ms.ToArray()));

        var loginResponse = await _httpClient.PostAsync("https://www.bibliotheek.rotterdam.nl/login?task=user.login", body);

        _logger.LogTrace("Received status code: {StatusCode}", loginResponse.StatusCode);
        _logger.LogTrace("Received HTML: {Html}", await loginResponse.Content.ReadAsStringAsync());

        _logger.LogTrace("Response headers: {ResponseHeaders}", FlattenHeaderValues(loginResponse.Headers));
        _logger.LogTrace("Content headers: {ContentHeaders}", FlattenHeaderValues(loginResponse.Content.Headers));

        _cookieJar.ReadCookieValues(loginResponse);
        _cookieJar.AddCookies(_httpClient.DefaultRequestHeaders);

        var redirectResponse = await _httpClient.GetAsync(loginResponse.Headers.Location);
        _cookieJar.ReadCookieValues(redirectResponse);

        return Result.Ok();
    }

    private async Task<Result<LoginPageSecurityTokens>> StartSessionAsync()
    {
        _logger.LogDebug("Starting session");
        var response = await _httpClient.GetAsync("https://www.bibliotheek.rotterdam.nl/login");

        _cookieJar.ReadCookieValues(response);

        var html = await response.Content.ReadAsStringAsync();
        var result = await LibraryHtmlParser.ParseLoginPageAsync(html);

        if (result.IsSuccess)
        {
            _logger.LogTrace("Login page security values: {LoginFormSecurityValues}", result);
        }

        return result;
    }

    private static Dictionary<string, string> FlattenHeaderValues(
        IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
    {
        return new(headers.Select(kvp => new KeyValuePair<string, string>(
            kvp.Key,
            string.Join(" ", kvp.Value))));
    }
}

public class LibraryLoginCredentials
{
    public const string SectionName = "LibraryLoginCredentials";

    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}