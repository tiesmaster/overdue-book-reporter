// Ignore Spelling: Bootstrapper ssl username

namespace Tiesmaster.OverdueBookReporter;

public static class Bootstrapper
{
    public static void Bootstrap(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LibraryRotterdamClientOptions>(configuration.GetRequiredSection(LibraryRotterdamClientOptions.SectionName));
        services.Configure<EmailSettings>(configuration.GetRequiredSection(EmailSettings.SectionName));

        services.AddHostedService<MainUseCase>();

        services.AddTransient<LibraryRotterdamClient>();
        services.AddTransient<EmailSender>();

        services
            .AddHttpClient<LibraryRotterdamClient>()
            .ConfigurePrimaryHttpMessageHandler(_ =>
            {
                return new HttpClientHandler
                {
                    AllowAutoRedirect = true,
                    UseCookies = true,
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