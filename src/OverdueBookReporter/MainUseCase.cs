using ConsoleTableExt;

namespace Tiesmaster.OverdueBookReporter;

public class MainUseCase(
    LibraryRotterdamClient libraryRotterdamClient,
    EmailSender emailSender,
    IHostApplicationLifetime lifetime,
    ILogger<MainUseCase> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var _ = ActivitySourceWrapper.StartMainUseCase();
        logger.LogInformation("Retrieving book listing");

        var today = DateOnly.FromDateTime(DateTime.Today);

        var statusReportResult = await GetBooksStatusReportSafeAsync(today);
        if (statusReportResult.IsSuccess)
        {
            LogSuccessStatus(statusReportResult.Value);
            WriteBooksToConsoleAsTable(statusReportResult.Value.BookListing);
        }
        else
        {
            logger.LogError("Failure retrieving book status report: {Errors}", statusReportResult.Errors.Reverse<IError>());
        }

        await emailSender.SendEmailAsync(statusReportResult);

        lifetime.StopApplication();
    }

    private async Task<Result<BooksStatusReport>> GetBooksStatusReportSafeAsync(DateOnly today)
    {
        using var _ = ActivitySourceWrapper.StartGetBookStatus();
        try
        {
            return await libraryRotterdamClient.GetBooksStatusReportAsync(today);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Unable to retrieve book status report").CausedBy(ex));
        }
    }

    private void LogSuccessStatus(BooksStatusReport statusReport)
    {
        logger.LogInformation("Received status report with status '{Status}', and {CountBooks} books", statusReport.Status, statusReport.BookListing.Count);
        foreach (var bookTitle in statusReport.BookListing!)
        {
            logger.LogDebug("Book in posession: {Book}", bookTitle);
        }
    }

    private static void WriteBooksToConsoleAsTable(IEnumerable<LoanedBook> bookListing)
    {
        ConsoleTableBuilder
            .From(bookListing.ToList())
            .ExportAndWriteLine();
    }
}