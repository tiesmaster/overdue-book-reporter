using System.Globalization;

using AngleSharp;
using AngleSharp.Dom;

namespace Tiesmaster.OverdueBookReporter;

public static class LibraryHtmlParser
{
    public static async Task<Result<LoginPageSecurityTokens>> ParseLoginPageAsync(string mainHtml)
    {
        static Result<LoginPageSecurityTokens> ToFailureResult<T>(Result<T> result) =>
            result
                .ToResult<LoginPageSecurityTokens>()
                .WithError("Unable to locate login page security tokens (the 'CSRF' token, or 'return' token)");

        var document = await ReadHtmlAsync(mainHtml);

        var securityTokensResult = document.QuerySelectorWithResult("form#com-users-login__form");
        if (securityTokensResult.IsFailed)
        {
            return ToFailureResult(securityTokensResult);
        }

        var securityTokens = securityTokensResult.Value;
        var csrfTokenResult = ParseCsrfToken(securityTokens);
        if (csrfTokenResult.IsFailed)
        {
            return ToFailureResult(csrfTokenResult);
        }

        var csrfToken = csrfTokenResult.Value;

        // a[href="https://example.org"]
        var returnTokenResult = securityTokens.QuerySelectorWithResult("""input[name="return"]""").GetAttribute("value");
        if (returnTokenResult.IsFailed)
        {
            return ToFailureResult(returnTokenResult);
        }

        return new LoginPageSecurityTokens(csrfToken, returnTokenResult.Value);
    }

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

    private static Result<string> ParseCsrfToken(IElement loginForm)
    {
        // We cannot query for the CSRF token field, as the name of it is random generated. However,
        // we do know all the other inputs, and exclude all of them to find the CSRF token field.

        var inputElements = loginForm.QuerySelectorAll("input");
        var expectedInputs = new HashSet<string>
        {
            "username",
            "password",
            "remember",
            "option",
            "task",
            "return"
        };

        var csrfTokenResultQuery =
            from ie in inputElements
            select ie.GetAttributeWithResult("name");

        if (csrfTokenResultQuery.Any(x => x.IsFailed))
        {
            return Result
                .Fail("Unable to retrieve any of the name attributes of the login form")
                .WithErrors(csrfTokenResultQuery.Where(x => x.IsFailed).SelectMany(x => x.Errors));
        }

        var csrfTokenQuery =
            from result in csrfTokenResultQuery
            let nameAttribute = result.Value
            where !expectedInputs.Contains(nameAttribute)
            select nameAttribute;

        if (csrfTokenQuery.Count() != 1)
        {
            return Result.Fail($"Unable to locate CSRF token in the login form, excpected 1 element. Got {csrfTokenQuery.Count()} elements");
        }

        return csrfTokenQuery.Single();
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