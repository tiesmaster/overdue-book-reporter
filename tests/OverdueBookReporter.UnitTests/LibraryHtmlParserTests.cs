using System.Reflection;

namespace OverdueBookReporter.UnitTests;

public class LibraryHtmlParserTests
{
    [Fact]
    public async Task Hoi()
    {
        var html = GetEmbeddedResourceHtml("2-books-lend-out_tomorrow-due.html");

        var booksListing = await LibraryHtmlParser.ParseBookListingAsync(html);
        booksListing.Should().NotBeEmpty();
        booksListing.Should().HaveCount(2);

        var firstLendOutBook = booksListing.First();
        firstLendOutBook.Name.Should().Be("Ik kan alleen wormen tekenen");

        booksListing.Last().Name.Should().Be("Olivier en het Brulmonster");

    }

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