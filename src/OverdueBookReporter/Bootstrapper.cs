using Microsoft.Extensions.Options;

namespace Tiesmaster.OverdueBookReporter;

public static class Bootstrapper
{
    public static void Bootstrap(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LibraryLoginCredentials>(configuration.GetRequiredSection(LibraryLoginCredentials.SectionName));
        services.Configure<EmailSettings>(configuration.GetRequiredSection(EmailSettings.SectionName));

        services.AddHostedService<MainUseCase>();

        services.AddTransient<LibraryRotterdamClient>();
        services.AddTransient<CookieJar>();
        services.AddTransient<EmailSender>();

        services
            .AddHttpClient<LibraryRotterdamClient>((services, client) =>
            {
                var libraryClientOptions = services.GetRequiredService<IOptions<LibraryLoginCredentials>>();

                // TODO: Push this to the helm chart, or chart install script/values
                // "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:122.0) Gecko/20100101 Firefox/122.0"

                // TODO: Get header name from HeaderNames.UserAgent
                client.DefaultRequestHeaders.Add("User-Agent", libraryClientOptions.Value.UserAgent);
            })
            .ConfigurePrimaryHttpMessageHandler(_ =>
            {
                return new HttpClientHandler
                {
                    AllowAutoRedirect = false,
                };
            });
    }
}

public class EmailSettings
{
    public const string SectionName = "EmailSettings";

    public EmailAddress From { get; set; } = null!;
    public EmailAddress To { get; set; } = null!;
    public MailServerSettings MailServer { get; set; } = null!;
}

public class EmailAddress
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
}

public class MailServerSettings
{
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public bool UseSsl { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}