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
        var bookListing = await _libraryRotterdamClient.GetBookListingAsync();

        _logger.LogInformation("Received book listing of {CountBooks} books", bookListing.Count());
        foreach (var bookTitle in bookListing)
        {
            _logger.LogDebug("Book in posession: {Book}", bookTitle);
        }

        var today = DateOnly.FromDateTime(DateTime.Today);
        var anyOverdue = bookListing.Any(x => x.GetStatus(today) == BookLoanStatus.Overdue);
        await _emailSender.SendEmailAsync(anyOverdue);

        _lifetime.StopApplication();
    }
}