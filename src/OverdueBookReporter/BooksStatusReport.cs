namespace Tiesmaster.OverdueBookReporter;

public enum BooksStatusReportStatus
{
    NotActive,
    Ok,
    AlmostDue,
    DueToday,
    Overdue,
    Error,
}

public class BooksStatusReport
{
    private readonly DateOnly _today;
    private readonly List<LoanedBook> _books;

    private readonly bool _isError;
    private readonly Exception _exception;

    public IEnumerable<LoanedBook> BookListing => _books;

    public BooksStatusReport(DateOnly today, IEnumerable<LoanedBook> books)
    {
        _today = today;
        _books = books.ToList();

        _exception = null!;
    }

    public BooksStatusReport(Exception exception)
    {
        _isError = true;
        _exception = exception;

        _books = null!;
    }

    public BooksStatusReportStatus Status => this switch
    {
        { _isError: true } => BooksStatusReportStatus.Error,
        { _books.Count: 0 } => BooksStatusReportStatus.NotActive,
        _ => AggregateBooksStatus(_books),
    };

    public Exception Exception => _exception;

    public int CountDueToday => _books.Count(x => x.GetStatus(_today) == BookLoanStatus.DueToday);
    public int CountOverdue => _books.Count(x => x.GetStatus(_today) == BookLoanStatus.Overdue);

    public int CountDaysLeft => (int)(_books.Min(x => x.DueDay).ToDateTime(default) - _today.ToDateTime(default)).TotalDays;

    private BooksStatusReportStatus AggregateBooksStatus(IEnumerable<LoanedBook> books)
    {
        var allStatusses = books.Select(x => x.GetStatus(_today)).Distinct();
        if (allStatusses.Contains(BookLoanStatus.Overdue))
        {
            return BooksStatusReportStatus.Overdue;
        }

        if (allStatusses.Contains(BookLoanStatus.DueToday))
        {
            return BooksStatusReportStatus.DueToday;
        }

        if (allStatusses.Contains(BookLoanStatus.AlmostDue))
        {
            return BooksStatusReportStatus.AlmostDue;
        }

        return BooksStatusReportStatus.Ok;
    }
}