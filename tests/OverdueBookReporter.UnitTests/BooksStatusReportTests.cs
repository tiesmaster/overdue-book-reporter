using System.Collections.Immutable;

namespace Tiesmaster.OverdueBookReporter.UnitTests;

public class BooksStatusReportTests
{
    [Fact]
    public void GivenNoBooksInPossesion_WhenGettingStatus_ThenNotActive()
    {
        // arrange
        var report = A.StatusReport.WithoutBooks();

        // act
        var status = report.Status;

        // assert
        status.Should().Be(BooksStatusReportStatus.NotActive);
    }

    [Fact]
    public void GivenBookDueInFarFuture_WhenGettingStatus_ThenOk()
    {
        // arrange
        var today = A.Today;
        var dueTomorrow = A.LoanedBook.WithDueDate(today.AddDays(10));

        var report = A.StatusReport.WithBooks(dueTomorrow);

        // act
        var status = report.Status;

        // assert
        status.Should().Be(BooksStatusReportStatus.Ok);
    }

    [Fact]
    public void GivenBookDueTomorrow_WhenGettingStatus_ThenAlmostDue()
    {
        // arrange
        var today = A.Today;
        var dueTomorrow = A.LoanedBook.WithDueDate(today.AddDays(1));

        var report = CreateReport(today, new[] { dueTomorrow });

        // act
        var status = report.Status;

        // assert
        status.Should().Be(BooksStatusReportStatus.AlmostDue);
        report.CountDaysLeft.Should().Be(1);
    }

    [Fact]
    public void GivenBookDueToday_WhenGettingStatus_ThenDueToday()
    {
        // arrange
        var today = A.Today;
        var dueTomorrow = A.LoanedBook.WithDueDate(today);

        var report = CreateReport(today, new[] { dueTomorrow });

        // act
        var status = report.Status;

        // assert
        status.Should().Be(BooksStatusReportStatus.DueToday);
    }

    [Fact]
    public void GivenOverdueBooks_WhenGettingStatus_ThenOverdue()
    {
        // arrange
        var today = A.Today;
        var dueTomorrow = A.LoanedBook.WithDueDate(today.AddDays(-1));

        var report = CreateReport(today, new[] { dueTomorrow });

        // act
        var status = report.Status;

        // assert
        status.Should().Be(BooksStatusReportStatus.Overdue);
    }

    private static BooksStatusReport CreateReport(DateOnly today, IEnumerable<LoanedBook> loanedBooks)
    {
        return new BooksStatusReport(today, Username: string.Empty, loanedBooks.ToImmutableHashSet());
    }
}