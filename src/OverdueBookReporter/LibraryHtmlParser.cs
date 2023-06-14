using AngleSharp;

namespace Tiesmaster.OverdueBookReporter;

public static class LibraryHtmlParser
{
    public static async Task<IEnumerable<LoanedBook>> ParseBookListingAsync(string mainHtml)
    {
        var config = Configuration.Default;
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(req => req.Content(mainHtml));

        var allTitles = document.QuerySelectorAll("a.title");

        return allTitles.Select(x => new LoanedBook(
            Name: x.InnerHtml,
            Status: default,
            DueDay: default
        ));
    }
}