namespace Tiesmaster.OverdueBookReporter.UnitTests;

public static class LoanedBookTestingExtensions
{
    public static LoanedBook WithDueDate(this LoanedBook loanedBook, DateOnly newDateOnly)
        => loanedBook with { DueDay = newDateOnly };
}