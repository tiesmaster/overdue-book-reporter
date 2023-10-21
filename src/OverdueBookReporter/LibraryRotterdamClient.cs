using System.Text;

using AngleSharp.Io;

using Microsoft.Extensions.Options;

namespace Tiesmaster.OverdueBookReporter;

public class LibraryRotterdamClient
{
    public const string HttpClientName = nameof(LibraryRotterdamClient);

    private readonly LibraryLoginCredentials _credentials;
    private readonly HttpClient _httpClient;
    private readonly CookieJar _cookieJar;
    private readonly ILogger<LibraryRotterdamClient> _logger;

    private bool _isLoggedIn;
    private LoginPageResult? _loginFormSecurityValues;

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

    public async Task StartSessionAsync()
    {
        _logger.LogDebug("Starting session");
        var response = await _httpClient.GetAsync("https://www.bibliotheek.rotterdam.nl/login");

        _cookieJar.ReadCookieValues(response);

        var html = await response.Content.ReadAsStringAsync();
        var result = await LibraryHtmlParser.ParseLoginPageAsync(html);
        _loginFormSecurityValues = result;

        _logger.LogTrace("Login page security values: {LoginFormSecurityValues}", _loginFormSecurityValues);
    }

    public async Task LoginAsync()
    {
        await StartSessionAsync();

        _logger.LogDebug("Logging in");
        var dict = new Dictionary<string, string>
        {
            { "username", _credentials.Username },
            { "password", _credentials.Password },
            { "return", _loginFormSecurityValues!.ReturnToken },
            { _loginFormSecurityValues!.CsrfToken, "1" },
        };
        var body = new FormUrlEncodedContent(dict);
        var ms = new MemoryStream();
        await body.CopyToAsync(ms);
        _cookieJar.AddCookies(body.Headers);

        _logger.LogTrace("Form content: {FormContent}", Encoding.UTF8.GetString(ms.ToArray()));

        var response = await _httpClient.PostAsync("https://www.bibliotheek.rotterdam.nl/login?task=user.login", body);

        _logger.LogTrace("Received status code: {StatusCode}", response.StatusCode);
        _logger.LogTrace("Received HTML: {Html}", await response.Content.ReadAsStringAsync());

        _logger.LogTrace("Response headers: {ResponseHeaders}", FlattenHeaderValues(response.Headers));
        _logger.LogTrace("Content headers: {ContentHeaders}", FlattenHeaderValues(response.Content.Headers));

        _cookieJar.ReadCookieValues(response);
        _cookieJar.AddCookies(_httpClient.DefaultRequestHeaders);

        // TODO: Should read this from Location: redirect header
        var response2 = await _httpClient.GetAsync("https://www.bibliotheek.rotterdam.nl/mijn-menu");
        _cookieJar.ReadCookieValues(response2);
    }

    public async Task<IEnumerable<LoanedBook>> GetBookListingAsync()
    {
        if (_isLoggedIn is false)
        {
            _logger.LogDebug("Not logged in yet; starting log in process");
            await LoginAsync();

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