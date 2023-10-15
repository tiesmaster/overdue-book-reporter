using System.Text;

namespace Tiesmaster.OverdueBookReporter;

public class LibraryRotterdamClient
{
    private const string SsoIdCookieName = "d22f17c4875e337ab168c60059511674";

    // private readonly HttpClient _client;
    private string? _ssoId;
    private string? _bicatSid;
    private string? _csrfToken;
    private readonly LibraryLoginCredentials _credentials;

    public LibraryRotterdamClient(LibraryLoginCredentials credentials)
    {
        _credentials = credentials;
    }

    private static HttpClient CreateLibraryRotterdamClient()
    {
        var client = new HttpClient(new HttpClientHandler
        {
            AllowAutoRedirect = false
        });

        client.DefaultRequestHeaders.Add(
            "User-Agent",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:109.0) Gecko/20100101 Firefox/113.0");

        client.DefaultRequestHeaders.Add(
            "Accept",
            "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8"
        );

        return client;
    }

    public async Task StartSessionAsync()
    {
        var client = CreateLibraryRotterdamClient();
        var response = await client.GetAsync("https://www.bibliotheek.rotterdam.nl/login");

        ReadCookieValues(response.Headers.GetValues("set-cookie"));

        var homePageHtml = await response.Content.ReadAsStringAsync();
        var result = await LibraryHtmlParser.ParseHomePageAsync(homePageHtml);
        _csrfToken = result.CsrfToken;

        Console.WriteLine($"SSOID: {_ssoId}");
        Console.WriteLine($"BICAT_ID: {_bicatSid}");
        Console.WriteLine($"CSRF token: {_csrfToken}");
    }

    public async Task LoginAsync()
    {
        var dict = new Dictionary<string, string>
        {
            { "username", _credentials.Username },
            { "password", _credentials.Password },
            { "return", "MjQ2" }, // TODO: read from the form
            { _csrfToken!, "1" },
        };
        var body = new FormUrlEncodedContent(dict);
        var ms = new MemoryStream();
        await body.CopyToAsync(ms);

        Console.WriteLine(Encoding.UTF8.GetString(ms.ToArray()));

        body.Headers.Add(
            "Cookie",
            $"{SsoIdCookieName}={_ssoId}; BICAT_SID={_bicatSid};");

        var client = CreateLibraryRotterdamClient();
        var response = await client.PostAsync("https://www.bibliotheek.rotterdam.nl/login?task=user.login", body);

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

        ReadCookieValues(response.Headers.GetValues("set-cookie"), shouldReadBicatCookie: false);

        Console.WriteLine($"SSOID: {_ssoId}");
        Console.WriteLine($"BICAT_ID: {_bicatSid}");

        var client2 = CreateLibraryRotterdamClient();

        client2.DefaultRequestHeaders.Add(
            "Cookie",
            $"{SsoIdCookieName}={_ssoId}; BICAT_SID={_bicatSid}; joomla_user_state=logged_in");

        // TODO: Should read this from Location: redirect header
        var response2 = await client2.GetAsync("https://www.bibliotheek.rotterdam.nl/mijn-menu");

        ReadCookieValues2(response2.Headers.GetValues("set-cookie"));

        Console.WriteLine($"SSOID: {_ssoId}");
        Console.WriteLine($"BICAT_ID: {_bicatSid}");
    }

    public async Task<IEnumerable<LoanedBook>> GetBookListingAsync()
    {
        var client = CreateLibraryRotterdamClient();

        var response = await client.GetAsync(
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
        var ssoIdCookie = cookieValues.First(x => x.StartsWith(SsoIdCookieName));
        var part1 = ssoIdCookie.Split("; ")[0];
        _ssoId = part1.Split("=")[1];

        if (shouldReadBicatCookie)
        {
            Console.WriteLine("Reading BICAT_ID cookie");
            var bicatCookie = cookieValues.First(x => x.StartsWith("BICAT_SID"));
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
        // var ssoIdCookie = cookieValues.First(x => x.StartsWith(SsoIdCookieName));
        // var part1 = ssoIdCookie.Split("; ")[0];
        // _ssoId = part1.Split("=")[1];

        Console.WriteLine("Reading BICAT_ID cookie");
        var bicatCookie = cookieValues.First(x => x.StartsWith("BICAT_SID"));
        _bicatSid = bicatCookie[10..46];
    }
}

public class LibraryLoginCredentials
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}