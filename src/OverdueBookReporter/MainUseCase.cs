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
        var statusReport = await _libraryRotterdamClient.GetBooksStatusReportAsync(today);

        LogStatus(statusReport);

        await _emailSender.SendEmailAsync(statusReport);

        _lifetime.StopApplication();
    }

    private void LogStatus(BooksStatusReport statusReport)
    {
        _logger.LogInformation("Received book listing of {CountBooks} books", statusReport.BookListing.Count());
        foreach (var bookTitle in statusReport.BookListing)
        {
            _logger.LogDebug("Book in posession: {Book}", bookTitle);
        }
    }
}
