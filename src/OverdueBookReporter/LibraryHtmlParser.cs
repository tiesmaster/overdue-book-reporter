using System.Globalization;

using AngleSharp;
using AngleSharp.Dom;

using FluentResults;

namespace Tiesmaster.OverdueBookReporter;

public static class LibraryHtmlParser
{
    public static async Task<Result<LoginPageSecurityTokens>> ParseLoginPageAsync(string mainHtml)
    {
        var document = await ReadHtmlAsync(mainHtml);

        var securityTokensResult = document.QuerySelectorWithResult("form#com-users-login__form");
        if (securityTokensResult.IsFailed)
        {
            return securityTokensResult
                .ToResult<LoginPageSecurityTokens>()
                .WithError("Unable to locate login page security tokens (the 'CSRF' token, or 'return' token)");
        }

        var securityTokens = securityTokensResult.Value;
        var csrfToken = ParseCsrfToken(securityTokens);

        // a[href="https://example.org"]
        var returnToken = securityTokens.QuerySelectorOrThrow("""input[name="return"]""").GetAttributeOrThrow("value");

        return Result.Ok(new LoginPageSecurityTokens(csrfToken, returnToken));
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

    private static string ParseCsrfToken(IElement loginForm)
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

        var csrfTokenQuery =
            from ie in inputElements
            let nameAttribute = ie.GetAttributeOrThrow("name")
            where !expectedInputs.Contains(nameAttribute)
            select nameAttribute;

        if (csrfTokenQuery.Count() != 1)
        {
            throw new InvalidOperationException($"Unable to locate CSRF token in the login form, excpected 1 element. Got {csrfTokenQuery.Count()} elements");
        }

        return csrfTokenQuery.Single();
    }

    private static LoanedBook ParseTitle(IElement titleHtml)
    {
        var titleName = titleHtml.QuerySelectorOrThrow("a.title").InnerHtml;

        var loanInfoHtml = titleHtml.QuerySelectorOrThrow("span.vet:nth-child(2)");
        var dueDate = DateOnly.Parse(loanInfoHtml.InnerHtml, CultureInfo.GetCultureInfo("nl-NL"));

        return new(
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