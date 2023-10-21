namespace Tiesmaster.OverdueBookReporter;

public record LoanedBook(string Name, DateOnly DueDay)
{
    public BookLoanStatus GetStatus(DateOnly currentDay)
    {
        return currentDay == DueDay
            ? BookLoanStatus.DueToday
            : currentDay > DueDay ? BookLoanStatus.Overdue : BookLoanStatus.Ok;
    }
}

public enum BookLoanStatus
{
    Ok,
    DueToday,
    Overdue,
}