using System.Reflection;

namespace OverdueBookReporter.UnitTests;

public class LibraryHtmlParserTests
{
    [Theory]
    [InlineData("homepage-1.html", "c8868c5ce2ff296ef698e9a47e1974fc")]
    [InlineData("homepage-2.html", "5f2529acbab589eeeecd985d5fc588cf")]
    public async Task ParseHomePageAsync_WithHomePages_ReturnsCsrfToken(string testFileName, string csrfToken)
    {
        // arrange
        var html = GetEmbeddedResourceHtml(testFileName);

        // act
        var homePageResult = await LibraryHtmlParser.ParseHomePageAsync(html);

        // assert
        homePageResult.CsrfToken.Should().Be(csrfToken);
    }

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
        firstBook.DueDay.Should().Be(DateOnly.Parse("2023-06-15"));

        var otherBook = booksListing.Last();
        otherBook.Name.Should().Be("Olivier en het Brulmonster");
        otherBook.DueDay.Should().Be(DateOnly.Parse("2023-06-15"));
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