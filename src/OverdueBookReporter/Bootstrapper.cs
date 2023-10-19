using OverdueBookReporter;

namespace Tiesmaster.OverdueBookReporter;

public static class Bootstrapper
{
    public static void Bootstrap(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LibraryLoginCredentials>(configuration.GetRequiredSection(LibraryLoginCredentials.SectionName));
        services.Configure<EmailSettings>(configuration.GetRequiredSection(EmailSettings.SectionName));

        services.AddHostedService<MainUseCase>();
    }
}