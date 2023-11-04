namespace Tiesmaster.OverdueBookReporter.UnitTests;

public static class LoanedBookTestingExtensions
{
    public static LoanedBook WithName(this LoanedBook loanedBook, string name)
        => loanedBook with { Name = name };

    public static LoanedBook WithDueDate(this LoanedBook loanedBook, DateOnly dueDay)
        => loanedBook with { DueDay = dueDay };

    public static LoanedBook WithDifferentName(this LoanedBook loanedBook)
        => loanedBook.WithName("The Great Gatsby");

    public static LoanedBook DueInFarFuture(this LoanedBook loanedBook)
        => loanedBook.WithDueDate(loanedBook.DueDay.AddDays(10));

    public static LoanedBook DueTomorrow(this LoanedBook loanedBook)
        => loanedBook.WithDueDate(loanedBook.DueDay.AddDays(1));

    public static LoanedBook DayAfterTomorrow(this LoanedBook loanedBook)
        => loanedBook.WithDueDate(loanedBook.DueDay.AddDays(2));

    public static LoanedBook DueToday(this LoanedBook loanedBook)
        => loanedBook;

    public static LoanedBook Overdue(this LoanedBook loanedBook)
        => loanedBook.WithDueDate(loanedBook.DueDay.AddDays(-1));
}