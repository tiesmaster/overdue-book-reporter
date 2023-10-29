using System.Text;

using Humanizer;

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

    public async Task SendEmailAsync(Result<BooksStatusReport> statusReportResult)
    {
        var (isSuccess, _, statusReport, errors) = statusReportResult;
        var email = isSuccess
            ? ComposeEmail(statusReport)
            : ComposeFailureEmail(errors.Reverse<IError>());

        await SendAsync(email);
    }

    private MimeMessage ComposeEmail(BooksStatusReport statusReport)
    {
        var message = EmailWithAddressing;

        message.Subject = statusReport.GetSubjectLine();
        message.Body = new TextPart("plain")
        {
            Text = statusReport.GetBody(),
        };

        return message;
    }

    private MimeMessage ComposeFailureEmail(IEnumerable<IError> errors)
    {
        var message = EmailWithAddressing;

        message.Subject = $"Error: {errors.First().Message}";
        message.Body = new TextPart("plain")
        {
            Text = string.Join(Environment.NewLine, errors),
        };

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
        => $"{statusReport.DescribeStatus()} [{statusReport.Username}]";

    public static string DescribeStatus(this BooksStatusReport statusReport)
    {
        var status = statusReport.Status;
        return status switch
        {
            BooksStatusReportStatus.NotActive => $"{status}: no books in possession",
            BooksStatusReportStatus.Ok => $"{status}: all good",
            BooksStatusReportStatus.AlmostDue => $"{status}: {"day".ToQuantity(statusReport.CountDaysLeft)} left",
            BooksStatusReportStatus.DueToday => $"{status}: {"books".ToQuantity(statusReport.CountDueToday)} due today!!",
            BooksStatusReportStatus.Overdue => $"{status}: {"books".ToQuantity(statusReport.CountOverdue)} are overdue!!!",
            _ => throw new NotImplementedException(),
        };
    }
    public static string GetBody(this BooksStatusReport statusReport)
    {
        return statusReport.Status switch
        {
            BooksStatusReportStatus.NotActive => "",
            BooksStatusReportStatus.Ok or BooksStatusReportStatus.AlmostDue or BooksStatusReportStatus.DueToday or BooksStatusReportStatus.Overdue
                => GetBookListingTable(statusReport.BookListing),
            _ => throw new NotImplementedException(),
        };
    }

    private static string GetBookListingTable(IEnumerable<LoanedBook> bookListing)
    {
        var sb = new StringBuilder();

        sb.AppendLine("Books in posession:");
        foreach (var book in bookListing)
        {
            sb.AppendLine($"    {book.Name} (Due date: {book.DueDay})");
        }

        return sb.ToString();
    }
}