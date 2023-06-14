using AngleSharp;
using AngleSharp.Dom;

namespace Tiesmaster.OverdueBookReporter;

public static class LibraryHtmlParser
{
    public static async Task<IEnumerable<LoanedBook>> ParseBookListingAsync(string mainHtml)
    {
        var config = Configuration.Default;
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(req => req.Content(mainHtml));

        var allTitles = document.QuerySelector(".list_titels");
        if (allTitles is null)
        {
            return Enumerable.Empty<LoanedBook>();
        }

        return allTitles.Children.Select(x => ParseTitle(x));
    }

    private static LoanedBook ParseTitle(IElement titleHtml)
    {
        var titleName = titleHtml.QuerySelector("a.title")!.InnerHtml;

        return new(
            Name: titleName,
            Status: default,
            DueDay: default
        );
    }
}