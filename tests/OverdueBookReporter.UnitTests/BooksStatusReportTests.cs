namespace Tiesmaster.OverdueBookReporter.UnitTests;

public class BooksStatusReportTests
{
    [Fact]
    public void GivenFailure_WhenGettingStatus_ThenReportsFailure()
    {
        // arrange
        var exceptionMessage = "Some failure";
        var report = new BooksStatusReport(new Exception(exceptionMessage));

        // act
        var status = report.Status;

        // assert
        status.Should().Be(BooksStatusReportStatus.Error);
    }

    [Fact]
    public void GivenNoBooksInPossesion_WhenGettingStatus_ThenNotActive()
    {
        // arrange
        var today = DateOnly.FromDateTime(DateTime.Today);
        var report = new BooksStatusReport(today, Enumerable.Empty<LoanedBook>());

        // act
        var status = report.Status;

        // assert
        status.Should().Be(BooksStatusReportStatus.NotActive);
    }

    [Fact]
    public void GivenBookDueTomorrow_WhenGettingStatus_ThenOk()
    {
        // arrange
        var today = DateOnly.Parse("2023-10-21");
        var dueTomorrow = new LoanedBook(Name: "1984", DueDay: today.AddDays(1));

        var report = new BooksStatusReport(today, new[] { dueTomorrow });

        // act
        var status = report.Status;

        // assert
        status.Should().Be(BooksStatusReportStatus.Ok);
    }

    [Fact]
    public void GivenBookDueToday_WhenGettingStatus_ThenDueToday()
    {
        // arrange
        var today = DateOnly.Parse("2023-10-21");
        var dueTomorrow = new LoanedBook(Name: "1984", DueDay: today);

        var report = new BooksStatusReport(today, new[] { dueTomorrow });

        // act
        var status = report.Status;

        // assert
        status.Should().Be(BooksStatusReportStatus.DueToday);
    }

    [Fact]
    public void GivenOverdueBooks_WhenGettingStatus_ThenOverdue()
    {
        // arrange
        var today = DateOnly.Parse("2023-10-21");
        var dueTomorrow = new LoanedBook(Name: "1984", DueDay: today.AddDays(-1));

        var report = new BooksStatusReport(today, new[] { dueTomorrow });

        // act
        var status = report.Status;

        // assert
        status.Should().Be(BooksStatusReportStatus.Overdue);
    }
}