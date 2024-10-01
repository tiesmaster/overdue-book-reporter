using System.Globalization;

using AngleSharp;
using AngleSharp.Dom;

namespace Tiesmaster.OverdueBookReporter;

public static class LibraryHtmlParser
{
    public static async Task<Result<IEnumerable<LoanedBook>>> ParseBookListingAsync(string mainHtml)
    {
        var document = await ReadHtmlAsync(mainHtml);

        var allTitles = document.QuerySelector(".list_titels");
        if (allTitles is null)
        {
            return Result.Ok(Enumerable.Empty<LoanedBook>());
        }

        var parsedTitlesResuls = allTitles.Children.Select(x => ParseTitle(x));
        if (parsedTitlesResuls.Any(x => x.IsFailed))
        {
            return Result
                .Fail("")
                .WithErrors(parsedTitlesResuls.Where(x => x.IsFailed).SelectMany(x => x.Errors));
        }
        else
        {
            return Result.Ok(parsedTitlesResuls.Select(x => x.Value));
        }
    }

    private static Result<LoanedBook> ParseTitle(IElement titleHtml)
    {
        var titleNameResult = titleHtml.QuerySelectorWithResult("a.title");
        if (titleNameResult.IsFailed)
        {
            return titleNameResult.ToResult().WithError("Unable to locate the title of the book");
        }

        var loanInfoHtmlResult = titleHtml.QuerySelectorWithResult("span.vet:nth-child(2)");
        if (loanInfoHtmlResult.IsFailed)
        {
            return loanInfoHtmlResult.ToResult().WithError("Unable to locate the 'loan info' element of the book");
        }

        var loanInfoHtml = loanInfoHtmlResult.Value;
        var dueDate = DateOnly.Parse(loanInfoHtml.InnerHtml, CultureInfo.GetCultureInfo("nl-NL"));

        var titleName = titleNameResult.Value.InnerHtml;

        return new LoanedBook(
            Name: titleName,
            DueDay: dueDate
        );
    }

    private static async Task<IDocument> ReadHtmlAsync(string mainHtml)
    {
        var config = Configuration.Default;
        var context = BrowsingContext.New(config);

        return await context.OpenAsync(req => req.Content(mainHtml));
    }
}

public record LoginPageSecurityTokens(string CsrfToken, string ReturnToken);