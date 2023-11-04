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
        var reportDay = A.Day;
        var bookDueInFarFuture = A.LoanedBook.WithDueDate(reportDay.AddDays(10));

        var report = A.StatusReport
            .WithReportDay(reportDay)
            .WithBooks(bookDueInFarFuture);

        // act
        var status = report.Status;

        // assert
        status.Should().Be(BooksStatusReportStatus.Ok);
    }

    [Fact]
    public void GivenBookDueTomorrow_WhenGettingStatus_ThenAlmostDue()
    {
        // arrange
        var reportDay = A.Day;
        var bookDueTomorrow = A.LoanedBook.WithDueDate(reportDay.AddDays(1));

        var report = A.StatusReport
            .WithReportDay(reportDay)
            .WithBooks(bookDueTomorrow);

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
        var reportDay = A.Day;
        var bookDueToday = A.LoanedBook.WithDueDate(reportDay);

        var report = A.StatusReport
            .WithReportDay(reportDay)
            .WithBooks(bookDueToday);

        // act
        var status = report.Status;

        // assert
        status.Should().Be(BooksStatusReportStatus.DueToday);
    }

    [Fact]
    public void GivenOverdueBooks_WhenGettingStatus_ThenOverdue()
    {
        // arrange
        var reportDay = A.Day;
        var overdueBook = A.LoanedBook.WithDueDate(reportDay.AddDays(-1));

        var report = A.StatusReport
            .WithReportDay(reportDay)
            .WithBooks(overdueBook);

        // act
        var status = report.Status;

        // assert
        status.Should().Be(BooksStatusReportStatus.Overdue);
    }
}