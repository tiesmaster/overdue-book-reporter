using System.Collections.Immutable;

namespace Tiesmaster.OverdueBookReporter;

public enum BooksStatusReportStatus
{
    NotActive,
    Ok,
    AlmostDue,
    DueToday,
    Overdue,
}

public record BooksStatusReport(DateOnly ReportDay, string Username, ImmutableList<LoanedBook> BookListing)
{
    public BooksStatusReportStatus Status => this switch
    {
        { BookListing.Count: 0 } => BooksStatusReportStatus.NotActive,
        _ => AggregateBooksStatus(BookListing),
    };

    public int CountDueToday => BookListing.Count((Func<LoanedBook, bool>)(x => x.GetStatus(ReportDay) == BookLoanStatus.DueToday));
    public int CountOverdue => BookListing.Count((Func<LoanedBook, bool>)(x => x.GetStatus(ReportDay) == BookLoanStatus.Overdue));

    public int CountDaysLeft => (int)(BookListing.Min(x => x.DueDay).ToDateTime(default) - ReportDay.ToDateTime(default)).TotalDays;

    private BooksStatusReportStatus AggregateBooksStatus(IEnumerable<LoanedBook> books)
    {
        var allStatusses = books.Select(x => x.GetStatus(ReportDay)).Distinct();
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