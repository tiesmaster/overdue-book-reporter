namespace Tiesmaster.OverdueBookReporter;

public record LoanedBook(string Name, DateOnly DueDay, bool IsMaxDueDateReached = false)
{
    public BookLoanStatus GetStatus(DateOnly currentDay)
    {
        if (currentDay > DueDay)
        {
            return BookLoanStatus.Overdue;
        }

        if (currentDay == DueDay)
        {
            return BookLoanStatus.DueToday;
        }

        if (currentDay.AddDays(5) >= DueDay)
        {
            return BookLoanStatus.AlmostDue;
        }

        return BookLoanStatus.Ok;
    }
}

public enum BookLoanStatus
{
    Ok,
    AlmostDue,
    DueToday,
    Overdue,
}