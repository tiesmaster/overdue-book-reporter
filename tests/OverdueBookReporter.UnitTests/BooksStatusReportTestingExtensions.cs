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
        => statusReport with { BookListing = ImmutableHashSet<LoanedBook>.Empty };
}