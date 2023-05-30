namespace Tiesmaster.OverdueBookReporter;

public class LibraryRotterdamClient
{
    private readonly HttpClient _client;
    private string? _ssoId;
    private string? _bicatSid;
    private string? _csrfToken;

    public LibraryRotterdamClient()
    {
        _client = new HttpClient();

        _client.DefaultRequestHeaders.Add(
            "User-Agent",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:109.0) Gecko/20100101 Firefox/113.0");
    }

    public async Task StartSessionAsync()
    {
        var response = await _client.GetAsync("https://www.bibliotheek.rotterdam.nl");

        ReadCookieValues(response.Headers.GetValues("set-cookie"));

        Console.WriteLine(_ssoId);
        Console.WriteLine(_bicatSid);
    }

    private void ReadCookieValues(IEnumerable<string> cookieValues)
    {
        var ssoIdCookie = cookieValues.First(x => x.StartsWith("1f23ba878c08449ef3595a1f6bec6273"));
        var part1 = ssoIdCookie.Split("; ")[0];
        _ssoId = part1.Split("=")[1];

        var bicatCookie = cookieValues.First(x => x.StartsWith("BICAT_SID"));
        _bicatSid = bicatCookie[10..46];
    }
}