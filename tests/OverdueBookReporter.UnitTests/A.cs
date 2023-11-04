using System.Collections.Immutable;

namespace Tiesmaster.OverdueBookReporter.UnitTests;

public static class A
{
    public static DateOnly Day => DateOnly.Parse("2023-10-21");

    public static LoanedBook LoanedBook => new(Name: "1984", DueDay: A.Day);

    public static BooksStatusReport StatusReport => new(
        ReportDay: A.Day,
        Username: "erwinleo",
        BookListing: ImmutableList<LoanedBook>.Empty);
}