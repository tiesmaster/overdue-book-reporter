namespace Tiesmaster.OverdueBookReporter;

public enum BooksStatusReportStatus
{
    NotActive,
    Ok,
    DueToday,
    Overdue,
    Error,
}

public class BooksStatusReport
{
    private readonly DateOnly _today;
    private readonly IEnumerable<LoanedBook> _books;

    private readonly bool _isError;
    private readonly Exception _exception;

    public IEnumerable<LoanedBook> BookListing => _books;

    public BooksStatusReport(DateOnly today, IEnumerable<LoanedBook> books)
    {
        _today = today;
        _books = books;

        _exception = null!;
    }

    public BooksStatusReport(Exception exception)
    {
        _isError = true;
        _exception = exception;

        _books = null!;
    }

    public BooksStatusReportStatus GetStatus(DateOnly day)
    {
        // var anyOverdue = bookListing.Any(x => x.GetStatus(today) == BookLoanStatus.Overdue);
        throw new NotImplementedException();
    }
}