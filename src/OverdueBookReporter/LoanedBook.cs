namespace Tiesmaster.OverdueBookReporter;

public record LoanedBook(string Name, BookLoanStatus Status, DateOnly DueDay);

public enum BookLoanStatus
{
    Normal,
    Overdue,
}