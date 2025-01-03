using System.Globalization;
using System.Text;

using Humanizer;
using Humanizer.Localisation;

using MailKit.Net.Smtp;

using Microsoft.Extensions.Options;

using MimeKit;

namespace Tiesmaster.OverdueBookReporter;

public class EmailSender(IOptions<EmailSettings> emailOptions)
{
    private readonly EmailSettings _settings = emailOptions.Value;

    public async Task SendEmailAsync(Result<BooksStatusReport> statusReportResult)
    {
        using var _ = Telemetry.StartSendEmail();
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
    private static readonly CultureInfo _dutchCulture = CultureInfo.GetCultureInfo("nl-NL");

    public static string GetSubjectLine(this BooksStatusReport statusReport)
    {
        var statusLine = $"{statusReport.DescribeStatus()} [{statusReport.Username}]";
        return statusReport.IsMaxDueDateReachedOfAnyBook
            ? $"{statusLine} (MAX DUE DATE REACHED)"
            : statusLine;
    }

    public static string DescribeStatus(this BooksStatusReport statusReport)
    {
        var status = statusReport.Status;
        return status switch
        {
            BooksStatusReportStatus.NotActive => $"{status}: no books in possession",
            BooksStatusReportStatus.Ok => $"{status}: all good",
            BooksStatusReportStatus.AlmostDue => $"{status}: {"day".ToQuantity(statusReport.CountDaysLeft)} left",
            BooksStatusReportStatus.DueToday => $"{status}: {"books".ToQuantity(statusReport.CountDueToday)} due today!!",
            BooksStatusReportStatus.Overdue => $"{status}: {"books".ToQuantity(statusReport.CountOverdue)} {"is".ToQuantity(statusReport.CountOverdue, ShowQuantityAs.None)} overdue!!!",
            _ => throw new NotImplementedException(),
        };
    }
    public static string GetBody(this BooksStatusReport statusReport)
    {
        if (statusReport.Status == BooksStatusReportStatus.NotActive)
        {
            return string.Empty;
        }

        var sb = new StringBuilder();

        AppendManagementSummaryLine(sb, statusReport);
        AppendBookListingTable(sb, statusReport);

        return sb.ToString();
    }

    private static void AppendManagementSummaryLine(StringBuilder builder, BooksStatusReport statusReport)
    {
        if (statusReport.Status is BooksStatusReportStatus.Ok or BooksStatusReportStatus.AlmostDue)
        {
            AppendFirstBookDueLine(builder, statusReport);
        }

        if (statusReport.Status is BooksStatusReportStatus.DueToday)
        {
            AppendBooksDueTodayLine(builder, statusReport);
        }

        if (statusReport.Status is BooksStatusReportStatus.Overdue)
        {
            AppendOverdueBooksLine(builder, statusReport);
        }

        builder.AppendLine();
    }

    private static void AppendFirstBookDueLine(StringBuilder builder, BooksStatusReport statusReport)
    {
        if (statusReport.FirstBooksDue.Count == statusReport.BookListing.Count)
        {
            builder.AppendLine($"ALL books due: {statusReport.FirstDueDay.Humanize(statusReport.ReportDay)}");
        }
        else
        {
            builder.AppendLine($"First book due: {statusReport.FirstDueDay.Humanize(statusReport.ReportDay)}");
        }
    }

    private static void AppendBooksDueTodayLine(StringBuilder builder, BooksStatusReport statusReport)
    {
        builder.AppendLine(
            $"Books due today: {statusReport.FirstBooksDue.Humanize(totalBookCount: statusReport.BookListing.Count)}");
    }

    private static void AppendOverdueBooksLine(StringBuilder builder, BooksStatusReport statusReport)
    {
        builder.AppendLine(
            $"Books overdue: {statusReport.OverdueBooks.Humanize(totalBookCount: statusReport.BookListing.Count)} " +
            $"({"day".ToQuantity(statusReport.CountDaysOverdue)} overdue)");
    }

    private static void AppendBookListingTable(StringBuilder builder, BooksStatusReport statusReport)
    {
        builder.AppendLine("Books in posession:");
        foreach (var book in statusReport.BookListing)
        {
            var dutchDueDateText = book.DueDay.ToString(_dutchCulture);
            builder.AppendLine($"    {book.Name} (Due date: {dutchDueDateText}, " +
                $"{CalculateHumanizedDueText(statusReport.ReportDay, book.DueDay)})");
        }
    }

    private static string CalculateHumanizedDueText(DateOnly reportDay, DateOnly dueDay)
        => reportDay == dueDay
            ? "today"
            : dueDay.Humanize(reportDay);

    private static string Humanize(this IEnumerable<LoanedBook> books, int totalBookCount, int maxListedItems = 3)
    {
        var itemCount = books.Count();
        if (itemCount == totalBookCount)
        {
            return "<<ALL BOOKS>>";
        }

        return itemCount > maxListedItems
            ? string.Join(", ", books.Take(maxListedItems).Select(x => x.Name)) + $" [+ {itemCount - maxListedItems} more]"
            : string.Join(", ", books.Select(x => x.Name));
    }
}