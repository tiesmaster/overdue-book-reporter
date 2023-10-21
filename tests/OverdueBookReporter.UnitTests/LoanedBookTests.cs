namespace Tiesmaster.OverdueBookReporter.UnitTests;

public class LoanedBookTests
{
    // 08: all good

    [Fact]
    public void GetStatus_WithDueDateInFuture_ReturnsOkStatus()
    {
        // arrange
        var today = DateOnly.Parse("2023-06-08");
        var sut = new LoanedBook(Name: "1984", DueDay: DateOnly.Parse("2023-06-14"));

        // act
        var status = sut.GetStatus(today);

        // assert
        status.Should().Be(BookLoanStatus.Ok);
    }

    // 09: 5 days left
    // 10: 4 days left
    // 11: 3 days left
    // 12: 2 days left
    // 13: 1 day left
    // 14: due date

    [Theory]
    [InlineData("2023-06-09")]
    [InlineData("2023-06-13")]
    public void GetStatus_WithBookAlmostDue_ReturnsOkStatus(string todaText)
    {
        // arrange
        var today = DateOnly.Parse(todaText);
        var sut = new LoanedBook(Name: "1984", DueDay: DateOnly.Parse("2023-06-14"));

        // act
        var status = sut.GetStatus(today);

        // assert
        status.Should().Be(BookLoanStatus.AlmostDue);
    }

    [Fact]
    public void GetStatus_WithDueDateToday_ReturnsDueToday()
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