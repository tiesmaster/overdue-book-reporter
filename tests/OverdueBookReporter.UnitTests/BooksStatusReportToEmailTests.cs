namespace Tiesmaster.OverdueBookReporter.UnitTests;

public class BooksStatusReportToEmailTests
{
    public class WhenGettingSubjectLine
    {
        [Fact]
        public void GivenNoBooksInPossesion_ThenNoBooksInPosession()
        {
            // arrange
            var report = A.StatusReport.WithoutBooks();

            // act
            var subjectLine = report.GetSubjectLine();

            // assert
            subjectLine.Should().Be("NotActive: no books in possession [erwinleo]");
        }

        [Fact]
        public void GivenBookDueInFarFuture_ThenAllGood()
        {
            // arrange
            var report = A.StatusReport.WithBookDueInFarFuture();

            // act
            var subjectLine = report.GetSubjectLine();

            // assert
            subjectLine.Should().Be("Ok: all good [erwinleo]");
        }

        // TODO: Also test for x days left

        [Fact]
        public void GivenBookDueTomorrow_Then1DayLeft()
        {
            // arrange
            var report = A.StatusReport.WithBookDueTomorrow();

            // act
            var subjectLine = report.GetSubjectLine();

            // assert
            subjectLine.Should().Be("AlmostDue: 1 day left [erwinleo]");
        }

        // TODO: Also test for plural

        [Fact]
        public void GivenBookDueToday_ThenDueToday()
        {
            // arrange
            var report = A.StatusReport.WithBookDueToday();

            // act
            var subjectLine = report.GetSubjectLine();

            // assert
            subjectLine.Should().Be("DueToday: 1 book due today!! [erwinleo]");
        }

        [Fact]
        public void GivenBookOverdue_ThenOk()
        {
            // arrange
            var report = A.StatusReport.WithOverdueBook();

            // act
            var subjectLine = report.GetSubjectLine();

            // assert
            subjectLine.Should().Be("Overdue: 1 book is overdue!!! [erwinleo]");
        }
    }
}