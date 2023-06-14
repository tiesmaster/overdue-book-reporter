using System.Globalization;
using AngleSharp;
using AngleSharp.Dom;

namespace Tiesmaster.OverdueBookReporter;

public static class LibraryHtmlParser
{
    public static async Task<HomePageResult> ParseHomePageAsync(string mainHtml)
    {
        var document = await ReadHtmlAsync(mainHtml);

        var loginForm = document.QuerySelector("form.lp-form");
        var csrfToken = ParseCsrfToken(loginForm);

        return new(csrfToken!);
    }

    public static async Task<IEnumerable<LoanedBook>> ParseBookListingAsync(string mainHtml)
    {
        var document = await ReadHtmlAsync(mainHtml);

        var allTitles = document.QuerySelector(".list_titels");
        if (allTitles is null)
        {
            return Enumerable.Empty<LoanedBook>();
        }

        return allTitles.Children.Select(x => ParseTitle(x));
    }

    private static string? ParseCsrfToken(IElement? formHtml)
    {
        // We cannot query for the CSRF token field, as the name of it is random generated. However,
        // we do know all the other inputs, and exclude all of them to find the CSRF token field.

        var inputElements = formHtml!.QuerySelectorAll("input");
        var expectedInputs = new HashSet<string>
        {
            "username",
            "password",
            "remember",
            "option",
            "task",
            "return"
        };

        var csrfToken = inputElements
            .Select(x => x.GetAttribute("name"))
            .Single(nameAttribute => !expectedInputs.Contains(nameAttribute!));

        return csrfToken;
    }

    private static LoanedBook ParseTitle(IElement titleHtml)
    {
        var titleName = titleHtml.QuerySelector("a.title")!.InnerHtml;

        var loanInfoHtml = titleHtml.QuerySelector("span.vet:nth-child(2)");
        var dueDate = DateOnly.Parse(loanInfoHtml!.InnerHtml, CultureInfo.GetCultureInfo("nl-NL"));

        return new(
            Name: titleName,
            Status: default,
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

public record HomePageResult(string CsrfToken);