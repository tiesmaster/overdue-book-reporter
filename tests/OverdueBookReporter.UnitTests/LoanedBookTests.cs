namespace Tiesmaster.OverdueBookReporter.UnitTests;

public class LoanedBookTests
{
    [Fact]
    public void GetStatus_WithDueDatToday_ReturnsDueToday()
    {
        // arrange
        var today = DateOnly.Parse("2023-06-14");
        var sut = new LoanedBook(Name: "1984", DueDay: today);

        // act
        var status = sut.GetStatus(today);

        // assert
        status.Should().Be(BookLoanStatus.DueToday);
    }

    [Fact]
    public void GetStatus_WithDueDateInFuture_ReturnsOkStatus()
    {
        // arrange
        var today = DateOnly.Parse("2023-06-14");
        var sut = new LoanedBook(Name: "1984", DueDay: today.AddDays(1));

        // act
        var status = sut.GetStatus(today);

        // assert
        status.Should().Be(BookLoanStatus.Ok);
    }

    [Fact]
    public void GetStatus_WithDueDateInThePast_ReturnsOverdueStatus()
    {
        // arrange
        var sut = new LoanedBook(Name: "1984", DueDay: DateOnly.Parse("2023-06-15"));

        // act
        var status = sut.GetStatus(currentDay: DateOnly.Parse("2023-06-16"));

        // assert
        status.Should().Be(BookLoanStatus.Overdue);
    }
}