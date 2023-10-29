namespace Tiesmaster.OverdueBookReporter;

public enum BooksStatusReportStatus
{
    NotActive,
    Ok,
    AlmostDue,
    DueToday,
    Overdue,
}

public class BooksStatusReport
{
    private readonly DateOnly _today;
    private readonly List<LoanedBook> _books;

    public IEnumerable<LoanedBook> BookListing => _books;

    public BooksStatusReport(DateOnly today, string username, IEnumerable<LoanedBook> books)
    {
        _today = today;
        Username = username;
        _books = books.ToList();
    }

    public BooksStatusReportStatus Status => this switch
    {
        { _books.Count: 0 } => BooksStatusReportStatus.NotActive,
        _ => AggregateBooksStatus(_books),
    };

    public int CountDueToday => _books.Count(x => x.GetStatus(_today) == BookLoanStatus.DueToday);
    public int CountOverdue => _books.Count(x => x.GetStatus(_today) == BookLoanStatus.Overdue);

    public int CountDaysLeft => (int)(_books.Min(x => x.DueDay).ToDateTime(default) - _today.ToDateTime(default)).TotalDays;

    public string Username { get; }

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