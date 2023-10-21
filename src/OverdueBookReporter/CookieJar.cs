using System.Net.Http.Headers;

using AngleSharp.Io;

namespace Tiesmaster.OverdueBookReporter;

public class CookieJar
{
    private readonly Dictionary<string, string> _cookies = new();
    private readonly ILogger<CookieJar> _logger;

    public CookieJar(ILogger<CookieJar> logger)
    {
        _logger = logger;
    }

    public void ReadCookieValues(HttpResponseMessage response)
        => ReadCookieValues(response.Headers.GetValues(HeaderNames.SetCookie));

    public void ReadCookieValues(IEnumerable<string> cookieValues)
    {
        _logger.LogTrace("ALL received set-cookie values: {SetCookieValues}", cookieValues);

        foreach (var cookie in cookieValues)
        {
            var (cookieName, cookieValue) = ReadCookie(cookie);
            _cookies[cookieName] = cookieValue;
        }

        _logger.LogDebug("Current cookie jar content: {Cookies}", _cookies);
    }

    public void AddCookies(HttpHeaders headers)
    {
        headers.Add(
            HeaderNames.Cookie,
            FlatttenCookies());
    }

    public string SsoId => _cookies[CookieNames.SsoId];
    public string BicatSid => _cookies[CookieNames.BicatSid];

    private string FlatttenCookies()
    {
        return string.Join(
            "; ",
            _cookies.Select(kvp => $"{kvp.Key}={kvp.Value}"));
    }

    private static (string, string) ReadCookie(string cookie)
    {
        var cookieValue = cookie.Split("; ")[0];
        var cookieParts = cookieValue.Split("=");
        return (cookieParts[0], cookieParts[1]);
    }

    private static class CookieNames
    {
        public const string SsoId = "d22f17c4875e337ab168c60059511674";
        public const string BicatSid = "BICAT_SID";
    }
}