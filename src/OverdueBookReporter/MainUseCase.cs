using ConsoleTableExt;

namespace Tiesmaster.OverdueBookReporter;

public class MainUseCase : BackgroundService
{
    private readonly LibraryRotterdamClient _libraryRotterdamClient;
    private readonly EmailSender _emailSender;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly ILogger<MainUseCase> _logger;

    public MainUseCase(
        LibraryRotterdamClient libraryRotterdamClient,
        EmailSender emailSender,
        IHostApplicationLifetime lifetime,
        ILogger<MainUseCase> logger)
    {
        _libraryRotterdamClient = libraryRotterdamClient;
        _emailSender = emailSender;
        _lifetime = lifetime;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Retrieving book listing");

        var today = DateOnly.FromDateTime(DateTime.Today);

        var statusReportResult = await GetBooksStatusReportSafeAsync(today);
        if (statusReportResult.IsSuccess)
        {
            LogSuccessStatus(statusReportResult.Value);
            WriteBooksToConsoleAsTable(statusReportResult.Value.BookListing);
        }
        else
        {
            _logger.LogError("Failure retrieving book status report: {Errors}", statusReportResult.Errors.Reverse<IError>());
        }

        await _emailSender.SendEmailAsync(statusReportResult);

        _lifetime.StopApplication();
    }

    private async Task<Result<BooksStatusReport>> GetBooksStatusReportSafeAsync(DateOnly today)
    {
        try
        {
            return await _libraryRotterdamClient.GetBooksStatusReportAsync(today);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Unable to retrieve book status report").CausedBy(ex));
        }
    }

    private void LogSuccessStatus(BooksStatusReport statusReport)
    {
        _logger.LogInformation("Received status report with status '{Status}', and {CountBooks} books", statusReport.Status, statusReport.BookListing.Count);
        foreach (var bookTitle in statusReport.BookListing!)
        {
            _logger.LogDebug("Book in posession: {Book}", bookTitle);
        }
    }

    private static void WriteBooksToConsoleAsTable(IEnumerable<LoanedBook> bookListing)
    {
        ConsoleTableBuilder
            .From(bookListing.ToList())
            .ExportAndWriteLine();
    }
}