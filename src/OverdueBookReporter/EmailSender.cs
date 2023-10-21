using MailKit.Net.Smtp;

using Microsoft.Extensions.Options;

using MimeKit;

namespace Tiesmaster.OverdueBookReporter;

public class EmailSender
{
    private readonly EmailSettings _settings;

    public EmailSender(IOptions<EmailSettings> emailOptions)
    {
        _settings = emailOptions.Value;
    }

    public async Task SendEmailAsync(BooksStatusReport statusReport)
    {
        var email = ComposeEmail(statusReport);
        await SendAsync(email);
    }

    private MimeMessage ComposeEmail(BooksStatusReport statusReport)
    {
        var message = EmailWithAddressing;
        message.Subject = statusReport.GetSubjectLine();
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

public static class BooksStatusReportEmailExtensions
{
    public static string GetSubjectLine(this BooksStatusReport statusReport)
    {
        var status = statusReport.Status;
        return status switch
        {
            BooksStatusReportStatus.NotActive => $"{status}: no books in possession",
            BooksStatusReportStatus.Ok => $"{status}: all good",
            BooksStatusReportStatus.DueToday => $"{status}: {statusReport.CountDueToday} books due today!!",
            BooksStatusReportStatus.Overdue => $"{status}: {statusReport.CountOverdue} books are overdue!!!",
            BooksStatusReportStatus.Error => $"{status}: {statusReport.Exception.Message}",
            _ => throw new NotImplementedException(),
        };
    }
}