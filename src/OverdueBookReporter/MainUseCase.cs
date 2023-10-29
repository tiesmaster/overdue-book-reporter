using FluentResults;

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

        // TODO: Maybe lift up the exception handling from the client to here in the main use case
        var statusReportResult = await _libraryRotterdamClient.GetBooksStatusReportAsync(today);

        //LogStatus(statusReportResult);

        await _emailSender.SendEmailAsync(statusReportResult.Value);

        _lifetime.StopApplication();
    }

    private void LogStatus(BooksStatusReport statusReport)
    {
        _logger.LogInformation("Received status report with status '{Status}', and {CountBooks} books", statusReport.Status, statusReport.BookListing?.Count());
        if (statusReport.Status != default) // BooksStatusReportStatus.Error)
        {
            foreach (var bookTitle in statusReport.BookListing!)
            {
                _logger.LogDebug("Book in posession: {Book}", bookTitle);
            }
        }
    }
}
