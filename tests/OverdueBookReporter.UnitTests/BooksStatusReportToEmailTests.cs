using System.Globalization;

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
        public void GivenNoBooksInPossesion_ThenEmailBodyIsCompletelyEmpty()
        {
            // arrange
            var report = A.StatusReport.WithoutBooks();

            // act
            var emailBody = report.GetBody();

            // assert
            emailBody.Should().BeEmpty();
        }

        // Ok/AlmostDue:
        //   First book due: tomorrow

        [Fact]
        public void GivenSingleBookInPossessionDueInFarFuture_ThenSummaryStatesDueIn10Days()
        {
            // arrange
            var report = A.StatusReport.WithBookDueInFarFuture();

            // act
            var emailBody = report.GetBody();

            // assert
            CultureInfo.CurrentCulture.Should().Be(CultureInfo.GetCultureInfo("nl-NL"));
            emailBody.Should().Be("""
                ALL books due: 10 days from now

                Books in posession:
                    1984 (Due date: 31-10-2023, 10 days from now)

                """);
        }

        [Fact]
        public void GivenMultipleBooksDueInFutureWithFirstOneDueTomorrow_ThenSummaryStatesDueTomorrow()
        {
            // arrange
            var report = A.StatusReport
                .AddDueTomorrowBook()
                .AddBook(
                    A.LoanedBook.DueInFarFuture().WithName("DueInFarFuture"));

            // act
            var emailBody = report.GetBody();

            // assert
            emailBody.Should().Be("""
                First book due: tomorrow

                Books in posession:
                    DueTomorrow (Due date: 22-10-2023, tomorrow)
                    DueInFarFuture (Due date: 31-10-2023, 10 days from now)

                """);
        }

        // DueToday:
        //   Book due today: ..., ..., ...

        [Fact]
        public void GivenMultipleBooksDueToday_ThenSummaryListsThoseBooks()
        {
            // arrange
            var report = A.StatusReport
                .WithBooks(
                    A.LoanedBook.DueToday().WithName("Due today 1"),
                    A.LoanedBook.DueToday().WithName("Due today 2"),
                    A.LoanedBook.DueToday().WithName("Due today 3"))
                .AddDueTomorrowBook();

            // act
            var emailBody = report.GetBody();

            // assert
            emailBody.Should().Be("""
                Books due today: Due today 1, Due today 2, Due today 3

                Books in posession:
                    Due today 1 (Due date: 21-10-2023, today)
                    Due today 2 (Due date: 21-10-2023, today)
                    Due today 3 (Due date: 21-10-2023, today)
                    DueTomorrow (Due date: 22-10-2023, tomorrow)

                """);
        }

        [Fact]
        public void GivenAllBooksDueToday_ThenSummaryIndicatesAllBooksAreDue()
        {
            // arrange
            var report = A.StatusReport
                .WithBooks(
                    A.LoanedBook.DueToday().WithName("Due today 1"),
                    A.LoanedBook.DueToday().WithName("Due today 2"),
                    A.LoanedBook.DueToday().WithName("Due today 3"));

            // act
            var emailBody = report.GetBody();

            // assert
            emailBody.Should().Be("""
                Books due today: <<ALL BOOKS>>

                Books in posession:
                    Due today 1 (Due date: 21-10-2023, today)
                    Due today 2 (Due date: 21-10-2023, today)
                    Due today 3 (Due date: 21-10-2023, today)

                """);
        }

        // DueToday (more than 3 books due today):
        //   Book due today: ..., ..., ... + 2 books

        [Fact]
        public void GivenMoreThanThreeBooksDueToday_ThenSummaryOnlyListsFirstThreeBooksAndShowsHowManyMore()
        {
            // arrange
            var report = A.StatusReport
                .WithBooks(
                    A.LoanedBook.DueToday().WithName("Due today 1"),
                    A.LoanedBook.DueToday().WithName("Due today 2"),
                    A.LoanedBook.DueToday().WithName("Due today 3"),
                    A.LoanedBook.DueToday().WithName("Due today 4"),
                    A.LoanedBook.DueToday().WithName("Due today 5"))
                .AddDueTomorrowBook();

            // act
            var emailBody = report.GetBody();

            // assert
            emailBody.Should().Be("""
                Books due today: Due today 1, Due today 2, Due today 3 [+ 2 more]

                Books in posession:
                    Due today 1 (Due date: 21-10-2023, today)
                    Due today 2 (Due date: 21-10-2023, today)
                    Due today 3 (Due date: 21-10-2023, today)
                    Due today 4 (Due date: 21-10-2023, today)
                    Due today 5 (Due date: 21-10-2023, today)
                    DueTomorrow (Due date: 22-10-2023, tomorrow)
                
                """);
        }

        // Overdue:
        //   Books overdue: ..., ..., ... (3 days overdue)

        [Fact]
        public void GivenMultipleBooksOverdue_ThenSummaryListsThoseBooks()
        {
            // arrange
            var report = A.StatusReport
                .WithBooks(
                    A.LoanedBook.Overdue().WithName("Overdue 1"),
                    A.LoanedBook.Overdue().WithName("Overdue 2"),
                    A.LoanedBook.Overdue().WithName("Overdue 3"))
                .AddDueTomorrowBook();

            // act
            var emailBody = report.GetBody();

            // assert
            emailBody.Should().Be("""
                Books overdue: Overdue 1, Overdue 2, Overdue 3 (1 day overdue)

                Books in posession:
                    Overdue 1 (Due date: 20-10-2023, yesterday)
                    Overdue 2 (Due date: 20-10-2023, yesterday)
                    Overdue 3 (Due date: 20-10-2023, yesterday)
                    DueTomorrow (Due date: 22-10-2023, tomorrow)
                
                """);
        }

        [Fact]
        public void GivenAllBooksBooksOverdue_ThenSummaryIndicatesAllBooks()
        {
            // arrange
            var report = A.StatusReport
                .WithBooks(
                    A.LoanedBook.Overdue().WithName("Overdue 1"),
                    A.LoanedBook.Overdue().WithName("Overdue 2"),
                    A.LoanedBook.Overdue().WithName("Overdue 3"));

            // act
            var emailBody = report.GetBody();

            // assert
            emailBody.Should().Be("""
                Books overdue: <<ALL BOOKS>> (1 day overdue)

                Books in posession:
                    Overdue 1 (Due date: 20-10-2023, yesterday)
                    Overdue 2 (Due date: 20-10-2023, yesterday)
                    Overdue 3 (Due date: 20-10-2023, yesterday)
                
                """);
        }

        [Fact]
        public void GivenMoreThanThreeBooksOverdue_ThenSummaryOnlyListsFirstThreeBooksAndShowsHowManyMore()
        {
            // arrange
            var report = A.StatusReport
                .WithBooks(
                    A.LoanedBook.Overdue().WithName("Overdue 1"),
                    A.LoanedBook.Overdue().WithName("Overdue 2"),
                    A.LoanedBook.Overdue().WithName("Overdue 3"),
                    A.LoanedBook.Overdue().WithName("Overdue 4"),
                    A.LoanedBook.Overdue().WithName("Overdue 5"))
                .AddDueTomorrowBook();

            // act
            var emailBody = report.GetBody();

            // assert
            emailBody.Should().Be("""
                Books overdue: Overdue 1, Overdue 2, Overdue 3 [+ 2 more] (1 day overdue)

                Books in posession:
                    Overdue 1 (Due date: 20-10-2023, yesterday)
                    Overdue 2 (Due date: 20-10-2023, yesterday)
                    Overdue 3 (Due date: 20-10-2023, yesterday)
                    Overdue 4 (Due date: 20-10-2023, yesterday)
                    Overdue 5 (Due date: 20-10-2023, yesterday)
                    DueTomorrow (Due date: 22-10-2023, tomorrow)
                
                """);
        }

        [Fact]
        public void GivenDifferentDueDates_ThenBooksInPossessionTableGivesDescriptiveDueDatesAdditionally()
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
                Books overdue: WayOverdue, Overdue (10 days overdue)

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