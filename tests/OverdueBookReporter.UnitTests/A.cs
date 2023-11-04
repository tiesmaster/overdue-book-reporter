namespace Tiesmaster.OverdueBookReporter.UnitTests;

public static class A
{
    public static DateOnly Today => DateOnly.Parse("2023-10-21");

    public static LoanedBook LoanedBook => new(Name: "1984", DueDay: A.Today);
}