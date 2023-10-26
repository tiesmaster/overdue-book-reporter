using System.Reflection;

using FluentResults.Extensions.FluentAssertions;

namespace Tiesmaster.OverdueBookReporter.UnitTests;

public class LibraryHtmlParserTests
{
    [Fact]
    public async Task ParseLoginPageAsync_WithCouplePages_ReturnsCsrfToken()
    {
        // arrange
        var expectedCsrfToken = "f14501b9585db15e368c694fe66b079a";
        var expectedReturnToken = "MjQ2";

        var testFileName = "loginpage.html";
        var html = GetEmbeddedResourceHtml(testFileName);

        // act
        var result = await LibraryHtmlParser.ParseLoginPageAsync(html);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.CsrfToken.Should().Be(expectedCsrfToken);
        result.Value.ReturnToken.Should().Be(expectedReturnToken);
    }

    [Fact]
    public async Task ParseLoginPageAsync_WithInvalidHtml_ReturnsError()
    {
        // arrange
        var html = string.Empty;

        // act
        var result = await LibraryHtmlParser.ParseLoginPageAsync(html);

        // assert
        result.Should().BeFailure();
        result.Should().HaveReason("Unable to locate login page security tokens (the 'CSRF' token, or 'return' token)");
        result.Should().HaveReason("selectors", (actualMessage, expectedMessage) => actualMessage.Contains(expectedMessage));
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