using System.Reflection;

namespace OverdueBookReporter.UnitTests;

public class LibraryHtmlParserTests
{
    [Fact]
    public async Task ParseBookListingAsync_WithBooksLendOutAndTomorrowDue_ReturnsThoseBooks()
    {
        // arrange
        var html = GetEmbeddedResourceHtml("2-books-lend-out_tomorrow-due.html");

        // act
        var booksListing = await LibraryHtmlParser.ParseBookListingAsync(html);

        // assert
        booksListing.Should().HaveCount(2);

        var firstBook = booksListing.First();
        firstBook.Name.Should().Be("Ik kan alleen wormen tekenen");
        // firstBook.DueDay.Should().Be(DateOnly.Parse("2023-06-15"));

        var otherBook = booksListing.Last();
        otherBook.Name.Should().Be("Olivier en het Brulmonster");
        // otherBook.DueDay.Should().Be(DateOnly.Parse("2023-06-15"));
    }

    [Fact]
    public async Task ParseBookListingAsync_WithoutBooksInPosession_ReturnsEmptyList()
    {
        // arrange
        var html = GetEmbeddedResourceHtml("no-books-in-posession.html");

        // act
        var booksListing = await LibraryHtmlParser.ParseBookListingAsync(html);

        // assert
        booksListing.Should().BeEmpty();
    }

    // TODO: Validate that the message "Geen leningen bekend" is shown when no books in posession

    private static string GetEmbeddedResourceHtml(string testFileName)
    {
        const string prefix = "OverdueBookReporter.UnitTests.test_files";

        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"{prefix}.{testFileName}";

        using var stream = assembly.GetManifestResourceStream(resourceName)!;
        using var reader = new StreamReader(stream);
        string result = reader.ReadToEnd();

        return result;
    }
}