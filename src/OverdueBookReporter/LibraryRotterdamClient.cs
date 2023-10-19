using AngleSharp.Io;

using System.Text;

namespace Tiesmaster.OverdueBookReporter;

public class LibraryRotterdamClient
{
    private static class CookieNames
    {
        public const string SsoId = "d22f17c4875e337ab168c60059511674";
        public const string BicatSid = "BICAT_SID";
    }

    private readonly HttpClient _client;
    private string? _ssoId;
    private string? _bicatSid;
    private LoginPageResult? _loginFormSecurityValues;
    private readonly LibraryLoginCredentials _credentials;

    public LibraryRotterdamClient(LibraryLoginCredentials credentials)
    {
        _credentials = credentials;
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
        var response = await _client.GetAsync("https://www.bibliotheek.rotterdam.nl/login");

        ReadCookieValues(response.Headers.GetValues(HeaderNames.SetCookie));

        var html = await response.Content.ReadAsStringAsync();
        var result = await LibraryHtmlParser.ParseLoginPageAsync(html);
        _loginFormSecurityValues = result;

        Console.WriteLine($"SSOID: {_ssoId}");
        Console.WriteLine($"BICAT_ID: {_bicatSid}");
        Console.WriteLine($"Login page security values: {_loginFormSecurityValues}");
    }

    public async Task LoginAsync()
    {
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

        Console.WriteLine(Encoding.UTF8.GetString(ms.ToArray()));

        body.Headers.Add(
            HeaderNames.Cookie,
            $"{CookieNames.SsoId}={_ssoId}; {CookieNames.BicatSid}={_bicatSid};");

        var response = await _client.PostAsync("https://www.bibliotheek.rotterdam.nl/login?task=user.login", body);

        Console.WriteLine(response.StatusCode);
        // Console.WriteLine(await response.Content.ReadAsStringAsync());

        // Console.WriteLine("Response headers:");
        // foreach (var header in response.Headers)
        // {
        //     Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
        // }

        // Console.WriteLine("Content headers:");
        // foreach (var header in response.Content.Headers)
        // {
        //     Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
        // }

        ReadCookieValues(response.Headers.GetValues(HeaderNames.SetCookie), shouldReadBicatCookie: false);

        Console.WriteLine($"SSOID: {_ssoId}");
        Console.WriteLine($"BICAT_ID: {_bicatSid}");

        _client.DefaultRequestHeaders.Add(
            HeaderNames.Cookie,
            $"{CookieNames.SsoId}={_ssoId}; {CookieNames.BicatSid}={_bicatSid}; joomla_user_state=logged_in");

        // TODO: Should read this from Location: redirect header
        var response2 = await _client.GetAsync("https://www.bibliotheek.rotterdam.nl/mijn-menu");

        ReadCookieValues2(response2.Headers.GetValues(HeaderNames.SetCookie));

        Console.WriteLine($"SSOID: {_ssoId}");
        Console.WriteLine($"BICAT_ID: {_bicatSid}");
    }

    public async Task<IEnumerable<LoanedBook>> GetBookListingAsync()
    {
        var response = await _client.GetAsync(
            $"https://wise-web.bibliotheek.rotterdam.nl//cgi-bin/bx.pl?event=invent;var=frame;" +
            $"ssoid={_ssoId}&ssokey=joomla&sid={_bicatSid}");

        var content = await response.Content.ReadAsStringAsync();

        // Console.WriteLine(content);

        return await LibraryHtmlParser.ParseBookListingAsync(content);

        // if (content.Contains("Brul"))
        // {
        //     Console.WriteLine("SUCCESS");
        // }
        // else
        // {
        //     Console.WriteLine("FAIL");
        // }
    }

    private void ReadCookieValues(IEnumerable<string> cookieValues, bool shouldReadBicatCookie = true)
    {
        // Console.WriteLine("ALL received set-cookie values");
        // foreach (var cookieValue in cookieValues)
        // {
        //     Console.WriteLine(cookieValue);
        // }

        Console.WriteLine("Reading SSOID cookie");
        var ssoIdCookie = cookieValues.First(x => x.StartsWith(CookieNames.SsoId));
        var part1 = ssoIdCookie.Split("; ")[0];
        _ssoId = part1.Split("=")[1];

        if (shouldReadBicatCookie)
        {
            Console.WriteLine("Reading BICAT_ID cookie");
            var bicatCookie = cookieValues.First(x => x.StartsWith(CookieNames.BicatSid));
            _bicatSid = bicatCookie[10..46];
        }
    }

    private void ReadCookieValues2(IEnumerable<string> cookieValues)
    {
        // Console.WriteLine("ALL received set-cookie values");
        // foreach (var cookieValue in cookieValues)
        // {
        //     Console.WriteLine(cookieValue);
        // }

        // Console.WriteLine("Reading SSOID cookie");
        // var ssoIdCookie = cookieValues.First(x => x.StartsWith(CookieNames.SsoId));
        // var part1 = ssoIdCookie.Split("; ")[0];
        // _ssoId = part1.Split("=")[1];

        Console.WriteLine("Reading BICAT_ID cookie");
        var bicatCookie = cookieValues.First(x => x.StartsWith(CookieNames.BicatSid));
        _bicatSid = bicatCookie[10..46];
    }
}

public class LibraryLoginCredentials
{
    public const string SectionName = "LibraryLoginCredentials";

    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}