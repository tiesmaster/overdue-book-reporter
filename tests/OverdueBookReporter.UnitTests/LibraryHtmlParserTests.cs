using System.Reflection;

using FluentResults.Extensions.FluentAssertions;

namespace Tiesmaster.OverdueBookReporter.UnitTests;

public class LibraryHtmlParserTests
{
    [Fact]
    public async Task ParseBookListingAsync_WithOneBookLendOutFirstDay_ReturnsThatBook()
    {
        // arrange
        var html = GetEmbeddedResourceHtml("1-book-lend-out_first-day.html");

        // act
        var booksListingResult = await LibraryHtmlParser.ParseBookListingAsync(html);

        // assert
        var booksListing = booksListingResult.Should().BeSuccess().Subject.Value;
        booksListing.Should().HaveCount(1);

        var book = booksListing.First();
        book.Name.Should().Be("Hoop");
        book.DueDay.Should().Be(DateOnly.Parse("2023-07-07"));
        book.IsMaxDueDateReached.Should().BeFalse();
    }

    [Fact]
    public async Task ParseBookListingAsync_WithBooksLendOutAndTomorrowDue_ReturnsThoseBooks()
    {
        // arrange
        var html = GetEmbeddedResourceHtml("2-books-lend-out_tomorrow-due.html");

        // act
        var booksListingResult = await LibraryHtmlParser.ParseBookListingAsync(html);

        // assert
        var booksListing = booksListingResult.Should().BeSuccess().Subject.Value;
        booksListing.Should().HaveCount(2);

        var firstBook = booksListing.First();
        firstBook.Name.Should().Be("Ik kan alleen wormen tekenen");
        firstBook.DueDay.Should().Be(DateOnly.Parse("2023-06-15"));
        firstBook.IsMaxDueDateReached.Should().Be(true);

        var otherBook = booksListing.Last();
        otherBook.Name.Should().Be("Olivier en het Brulmonster");
        otherBook.DueDay.Should().Be(DateOnly.Parse("2023-06-15"));
        otherBook.IsMaxDueDateReached.Should().BeTrue();
    }

    [Fact]
    public async Task ParseBookListingAsync_WithoutBooksInPossession_ReturnsEmptyList()
    {
        // arrange
        var html = GetEmbeddedResourceHtml("no-books-in-posession.html");

        // act
        var booksListingResult = await LibraryHtmlParser.ParseBookListingAsync(html);

        // assert
        var booksListing = booksListingResult.Should().BeSuccess().Subject.Value;
        booksListing.Should().BeEmpty();
    }

    private static string GetEmbeddedResourceHtml(string testFileName)
    {
        const string prefix = "Tiesmaster.OverdueBookReporter.UnitTests.test_files";

        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"{prefix}.{testFileName}";

        using var stream = assembly.GetManifestResourceStream(resourceName)!;
        using var reader = new StreamReader(stream);
        var result = reader.ReadToEnd();

        return result;
    }
}