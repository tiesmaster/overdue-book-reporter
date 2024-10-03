using System.Collections.Immutable;

namespace Tiesmaster.OverdueBookReporter.UnitTests;

public class BooksStatusReportTests
{
    [Fact]
    public void GivenNoBooksInPossession_WhenGettingStatus_ThenNotActive()
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
        var report = A.StatusReport.WithBookDueInFarFuture();

        // act
        var status = report.Status;

        // assert
        status.Should().Be(BooksStatusReportStatus.Ok);
    }

    [Fact]
    public void GivenBookDueTomorrow_WhenGettingStatus_ThenAlmostDue()
    {
        // arrange
        var report = A.StatusReport.WithBookDueTomorrow();

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
        var report = A.StatusReport.WithBookDueToday();

        // act
        var status = report.Status;

        // assert
        status.Should().Be(BooksStatusReportStatus.DueToday);
    }

    [Fact]
    public void GivenOverdueBooks_WhenGettingStatus_ThenOverdue()
    {
        // arrange
        var report = A.StatusReport.WithOverdueBook();

        // act
        var status = report.Status;

        // assert
        status.Should().Be(BooksStatusReportStatus.Overdue);
    }
}