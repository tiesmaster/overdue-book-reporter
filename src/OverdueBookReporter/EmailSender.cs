using MailKit.Net.Smtp;

using Microsoft.Extensions.Options;

using MimeKit;

using Tiesmaster.OverdueBookReporter;

namespace OverdueBookReporter;

public class EmailSender
{
    private readonly EmailSettings _settings;

    public EmailSender(IOptions<EmailSettings> emailOptions)
    {
        _settings = emailOptions.Value;
    }

    public async Task SendEmailAsync(bool anyOverdue)
    {
        var email = ComposeEmail(anyOverdue);
        await SendAsync(email);
    }

    private MimeMessage ComposeEmail(bool anyOverdue)
    {
        var message = EmailWithAddressing;
        message.Subject = anyOverdue ? "OVERDUE!!!" : "all good!";
        return message;
    }

    private MimeMessage EmailWithAddressing
    {
        get
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_settings.From.Name, _settings.From.Address));
            message.To.Add(new MailboxAddress(_settings.To.Name, _settings.To.Address));

            return message;
        }
    }

    private async Task SendAsync(MimeMessage message)
    {
        using var smtpClient = new SmtpClient();

        var mailServerSettings = _settings.MailServer;
        smtpClient.Connect(host: mailServerSettings.Host, port: mailServerSettings.Port, useSsl: mailServerSettings.UseSsl);
        await smtpClient.AuthenticateAsync(mailServerSettings.Username, mailServerSettings.Password);

        await smtpClient.SendAsync(message);

        await smtpClient.DisconnectAsync(quit: true);
    }
}