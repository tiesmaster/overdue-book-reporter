using System.Collections.Immutable;

namespace Tiesmaster.OverdueBookReporter.UnitTests;

public static class BooksStatusReportTestingExtensions
{
    public static BooksStatusReport WithReportDay(this BooksStatusReport statusReport, DateOnly reportDay)
        => statusReport with { ReportDay = reportDay };

    public static BooksStatusReport WithBooks(this BooksStatusReport statusReport, ImmutableHashSet<LoanedBook> books)
        => statusReport with { BookListing = books };

    public static BooksStatusReport WithBooks(this BooksStatusReport statusReport, params LoanedBook[] books)
        => statusReport.WithBooks(books.ToImmutableHashSet());

    public static BooksStatusReport WithoutBooks(this BooksStatusReport statusReport)
        => statusReport.WithBooks(ImmutableHashSet<LoanedBook>.Empty);

    public static BooksStatusReport WithBookDueInFarFuture(this BooksStatusReport statusReport)
        => statusReport.WithBooks(A.LoanedBook.DueInFarFuture());

    public static BooksStatusReport WithBookDueTomorrow(this BooksStatusReport statusReport)
        => statusReport.WithBooks(A.LoanedBook.DueTomorrow());

    public static BooksStatusReport WithBookDueToday(this BooksStatusReport statusReport)
        => statusReport.WithBooks(A.LoanedBook.DueToday());

    public static BooksStatusReport WithOverdueBook(this BooksStatusReport statusReport)
        => statusReport.WithBooks(A.LoanedBook.Overdue());
}