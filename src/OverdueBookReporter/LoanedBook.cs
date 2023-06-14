namespace Tiesmaster.OverdueBookReporter;

public record LoanedBook(string Name, DateOnly DueDay)
{
    public BookLoanStatus GetStatus(DateOnly currentDay)
    {
        return currentDay > DueDay ? BookLoanStatus.Overdue : BookLoanStatus.Normal;
    }
}

public enum BookLoanStatus
{
    Normal,
    Overdue,
}