using System.Text;

using AngleSharp.Io;

using Microsoft.Extensions.Options;

namespace Tiesmaster.OverdueBookReporter;

public class LibraryRotterdamClient
{
    private static class CookieNames
    {
        public const string SsoId = "d22f17c4875e337ab168c60059511674";
        public const string BicatSid = "BICAT_SID";
    }

    private readonly LibraryLoginCredentials _credentials;
    private readonly ILogger<LibraryRotterdamClient> _logger;

    private readonly HttpClient _client;

    private bool _isLoggedIn;
    private string? _ssoId;
    private string? _bicatSid;
    private LoginPageResult? _loginFormSecurityValues;

    public LibraryRotterdamClient(
        IOptions<LibraryLoginCredentials> credentials,
        ILogger<LibraryRotterdamClient> logger)
    {
        _credentials = credentials.Value;
        _logger = logger;

        _client = CreateLibraryRotterdamClient();
    }

    private static HttpClient CreateLibraryRotterdamClient()
    {
        return new HttpClient(new HttpClientHandler
        {
            AllowAutoRedirect = false
        });
    }

    public async Task StartSessionAsync()
    {
        _logger.LogDebug("Starting session");
        var response = await _client.GetAsync("https://www.bibliotheek.rotterdam.nl/login");

        ReadCookieValues(response.Headers.GetValues(HeaderNames.SetCookie));

        var html = await response.Content.ReadAsStringAsync();
        var result = await LibraryHtmlParser.ParseLoginPageAsync(html);
        _loginFormSecurityValues = result;

        _logger.LogTrace("SSOID: {SsoId}", _ssoId);
        _logger.LogTrace("BICAT_ID: {BicatSid}", _bicatSid);
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

        _logger.LogTrace("Form content: {FormContent}", Encoding.UTF8.GetString(ms.ToArray()));

        body.Headers.Add(
            HeaderNames.Cookie,
            $"{CookieNames.SsoId}={_ssoId}; {CookieNames.BicatSid}={_bicatSid};");

        var response = await _client.PostAsync("https://www.bibliotheek.rotterdam.nl/login?task=user.login", body);

        _logger.LogTrace("Received status code: {StatusCode}", response.StatusCode);
        _logger.LogTrace("Received HTML: {Html}", await response.Content.ReadAsStringAsync());

        _logger.LogTrace("Response headers: {ResponseHeaders}", FlattenHeaderValues(response.Headers));
        _logger.LogTrace("Content headers: {ContentHeaders}", FlattenHeaderValues(response.Content.Headers));

        ReadCookieValues(response.Headers.GetValues(HeaderNames.SetCookie), shouldReadBicatCookie: false);

        _logger.LogTrace("SSOID: {SsoId}", _ssoId);
        _logger.LogTrace("BICAT_ID: {BicatSid}", _bicatSid);

        _client.DefaultRequestHeaders.Add(
            HeaderNames.Cookie,
            $"{CookieNames.SsoId}={_ssoId}; {CookieNames.BicatSid}={_bicatSid}; joomla_user_state=logged_in");

        // TODO: Should read this from Location: redirect header
        var response2 = await _client.GetAsync("https://www.bibliotheek.rotterdam.nl/mijn-menu");

        ReadCookieValues2(response2.Headers.GetValues(HeaderNames.SetCookie));

        _logger.LogTrace("SSOID: {SsoId}", _ssoId);
        _logger.LogTrace("BICAT_ID: {BicatSid}", _bicatSid);
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
        var response = await _client.GetAsync(
            $"https://wise-web.bibliotheek.rotterdam.nl//cgi-bin/bx.pl?event=invent;var=frame;" +
            $"ssoid={_ssoId}&ssokey=joomla&sid={_bicatSid}");

        var content = await response.Content.ReadAsStringAsync();

        _logger.LogTrace("Received HTML: {Html}", content);

        return await LibraryHtmlParser.ParseBookListingAsync(content);
    }

    private void ReadCookieValues(IEnumerable<string> cookieValues, bool shouldReadBicatCookie = true)
    {
        _logger.LogTrace("ALL received set-cookie values: {SetCookieValues}", cookieValues);
        _logger.LogTrace("Reading SSOID cookie");

        var ssoIdCookie = cookieValues.First(x => x.StartsWith(CookieNames.SsoId));
        var part1 = ssoIdCookie.Split("; ")[0];
        _ssoId = part1.Split("=")[1];

        if (shouldReadBicatCookie)
        {
            _logger.LogTrace("Reading BICAT_ID cookie");
            var bicatCookie = cookieValues.First(x => x.StartsWith(CookieNames.BicatSid));
            _bicatSid = bicatCookie[10..46];
        }
    }

    private void ReadCookieValues2(IEnumerable<string> cookieValues)
    {
        _logger.LogTrace("ALL received set-cookie values: {SetCookieValues}", cookieValues);
        _logger.LogTrace("Reading BICAT_ID cookie");

        var bicatCookie = cookieValues.First(x => x.StartsWith(CookieNames.BicatSid));
        _bicatSid = bicatCookie[10..46];
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