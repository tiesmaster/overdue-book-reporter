namespace Tiesmaster.OverdueBookReporter.UnitTests;

public static class LoanedBookTestingExtensions
{
    public static LoanedBook WithDueDate(this LoanedBook loanedBook, DateOnly newDateOnly)
        => loanedBook with { DueDay = newDateOnly };

    public static LoanedBook DueInFarFuture(this LoanedBook loanedBook)
        => loanedBook.WithDueDate(loanedBook.DueDay.AddDays(10));

    public static LoanedBook DueTomorrow(this LoanedBook loanedBook)
        => loanedBook.WithDueDate(loanedBook.DueDay.AddDays(1));

    public static LoanedBook DueToday(this LoanedBook loanedBook)
        => loanedBook;

    public static LoanedBook Overdue(this LoanedBook loanedBook)
        => loanedBook.WithDueDate(loanedBook.DueDay.AddDays(-1));
}