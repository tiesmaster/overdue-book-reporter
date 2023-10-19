using MailKit.Net.Smtp;

using Microsoft.Extensions.Options;

using MimeKit;

using Tiesmaster.OverdueBookReporter;

namespace OverdueBookReporter;

public class MainUseCase : BackgroundService
{
    private readonly LibraryLoginCredentials _credentials;
    private readonly EmailSettings _emailSettings;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly ILogger<MainUseCase> _logger;

    public MainUseCase(
        IOptions<LibraryLoginCredentials> credentialOptions,
        IOptions<EmailSettings> emailOptions,
        IHostApplicationLifetime lifetime,
        ILogger<MainUseCase> logger)
    {
        _credentials = credentialOptions.Value;
        _emailSettings = emailOptions.Value;
        _lifetime = lifetime;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var client = new LibraryRotterdamClient(_credentials);

        Console.WriteLine("Starting session");
        await client.StartSessionAsync();

        Console.WriteLine("Logging in");
        await client.LoginAsync();

        Console.WriteLine("Retrieving book listing");
        var bookListing = await client.GetBookListingAsync();

        foreach (var bookTitle in bookListing)
        {
            Console.WriteLine(bookTitle);
        }

        var today = DateOnly.FromDateTime(DateTime.Today);

        var anyOverdue = bookListing.Any(x => x.GetStatus(today) == BookLoanStatus.Overdue);

        await SendEmailAsync(anyOverdue, _emailSettings);

        _lifetime.StopApplication();
    }

    static async Task SendEmailAsync(bool anyOverdue, EmailSettings settings)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(settings.From.Name, settings.From.Address));
        message.To.Add(new MailboxAddress(settings.To.Name, settings.To.Address));
        message.Subject = anyOverdue ? "OVERDUE!!!" : "all good!";

        // message.Body = new TextPart("plain")
        // {
        //     Text = @""
        // };

        using var smtpClient = new SmtpClient();

        var mailServerSettings = settings.MailServer;
        smtpClient.Connect(host: mailServerSettings.Host, port: mailServerSettings.Port, useSsl: mailServerSettings.UseSsl);
        await smtpClient.AuthenticateAsync(mailServerSettings.Username, mailServerSettings.Password);

        await smtpClient.SendAsync(message);

        await smtpClient.DisconnectAsync(quit: true);
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