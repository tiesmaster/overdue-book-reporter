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

        [Fact]
        public void GivenBookDueTomorrow_ThenOneDayLeft()
        {
            // arrange
            var report = A.StatusReport.WithBookDueTomorrow();

            // act
            var subjectLine = report.GetSubjectLine();

            // assert
            subjectLine.Should().Be("AlmostDue: 1 day left [erwinleo]");
        }

        [Fact]
        public void GivenBookDueDayAfterTomorrow_ThenTwoDaysLeft()
        {
            // arrange
            var report = A.StatusReport.WithBooks(A.LoanedBook.DayAfterTomorrow());

            // act
            var subjectLine = report.GetSubjectLine();

            // assert
            subjectLine.Should().Be("AlmostDue: 2 days left [erwinleo]");
        }

        [Fact]
        public void GivenOneBookDueToday_ThenDueTodaySingular()
        {
            // arrange
            var report = A.StatusReport.WithBookDueToday();

            // act
            var subjectLine = report.GetSubjectLine();

            // assert
            subjectLine.Should().Be("DueToday: 1 book due today!! [erwinleo]");
        }

        [Fact]
        public void GivenTwoBooksDueToday_ThenDueTodayPlural()
        {
            // arrange
            var report = A.StatusReport
                .WithBooks(
                    A.LoanedBook.DueToday(),
                    A.LoanedBook.DueToday().WithDifferentName());

            // act
            var subjectLine = report.GetSubjectLine();

            // assert
            subjectLine.Should().Be("DueToday: 2 books due today!! [erwinleo]");
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

    public class WhenGettingEmailBody
    {
        [Fact]
        public void GivenNoBooksInPossesion_ThenNoBooksInPosession()
        {
            // arrange
            var report = A.StatusReport.WithoutBooks();

            // act
            var emailBody = report.GetBody();

            // assert
            emailBody.Should().BeEmpty();
        }

        // Ok:
        //   First book due: tomorrow

        // AlmostDue:
        //   First book due: tomorrow

        // DueToday:
        //   Book due today: ..., ..., ...

        // DueToday (more than 3 books due today):
        //   Book due today: ..., ..., ... + 2 books

        // DueToday (all books):
        //   Book due today: <<ALL BOOKS>>

        // Overdue:
        //   Books overdue: ..., ..., ... (3 days overdue)

        [Fact]
        public void GivenSingleBookInPossessionDueTomorrow_ThenHumanizedToTomorrow()
        {
            // arrange
            var report = A.StatusReport.WithBookDueTomorrow();

            // act
            var emailBody = report.GetBody();

            // assert
            emailBody.Should().Be("""
                Books in posession:
                    1984 (Due date: 22-10-2023, tomorrow)

                """);
        }

        [Fact]
        public void GivenDifferentDueDates_ThenHumanizedDueDateIsDescriptive()
        {
            // arrange
            var report = A.StatusReport
                .WithBooks(
                    A.LoanedBook.WithDueDate(A.Day.AddDays(-10)).WithName("WayOverdue"),
                    A.LoanedBook.Overdue().WithName("Overdue"),
                    A.LoanedBook.DueToday().WithName("DueToday"),
                    A.LoanedBook.DueTomorrow().WithName("DueTomorrow"),
                    A.LoanedBook.DueInFarFuture().WithName("DueInFarFuture"));

            // act
            var emailBody = report.GetBody();

            // assert
            emailBody.Should().Be("""
                Books in posession:
                    WayOverdue (Due date: 11-10-2023, 10 days ago)
                    Overdue (Due date: 20-10-2023, yesterday)
                    DueToday (Due date: 21-10-2023, today)
                    DueTomorrow (Due date: 22-10-2023, tomorrow)
                    DueInFarFuture (Due date: 31-10-2023, 10 days from now)

                """);
        }
    }
}