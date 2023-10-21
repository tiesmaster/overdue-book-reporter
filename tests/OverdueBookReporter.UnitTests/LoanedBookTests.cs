namespace Tiesmaster.OverdueBookReporter.UnitTests;

public class LoanedBookTests
{
    [Theory]
    [InlineData("2023-06-14")]
    [InlineData("2023-06-15")]
    public void GetStatus_WithDueDateInFuture_ReturnsNormalStatus(string currentDay)
    {
        // arrange
        var sut = new LoanedBook(Name: "1984", DueDay: DateOnly.Parse("2023-06-15"));

        // act
        var status = sut.GetStatus(DateOnly.Parse(currentDay));

        // assert
        status.Should().Be(BookLoanStatus.Normal);
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