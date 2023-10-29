using System.Text;

using AngleSharp.Dom;

using FluentResults;

namespace Tiesmaster.OverdueBookReporter;

public static class AngleSharpExtensions
{
    public static Result<IElement> QuerySelectorWithResult(this IDocument input, string selectors)
    {
        var element = input.QuerySelector(selectors);

        return element is IElement el
            ? Result.Ok(el)
            : Result.Fail($"Unable to locate the given element based on the given selectors '{selectors}'");
    }

    public static Result<IElement> QuerySelectorWithResult(this IElement input, string selectors)
    {
        var element = input.QuerySelector(selectors);

        return element is IElement el
            ? Result.Ok(el)
            : Result.Fail($"Unable to locate the given element based on the given selectors '{selectors}'");
    }

    public static Result<string> GetAttribute(this Result<IElement> resultOfInput, string name)
    {
        if (resultOfInput.IsFailed)
        {
            return resultOfInput
                .ToResult<string>();
        }

        var input = resultOfInput.Value;

        var attr = input.GetAttribute(name);

        return attr is string s
            ? Result.Ok(s)
            : Result.Fail($"Attribute '{name}' doesn't exist on element {input.GetPath()}");
    }

    public static Result<string> GetAttributeWithResult(this IElement input, string name)
    {
        var attr = input.GetAttribute(name);

        return attr is string s
            ? Result.Ok(s)
            : Result.Fail($"Attribute '{name}' doesn't exist on element {input.GetPath()}");
    }

    public static string GetPath(this IElement input)
    {
        var ancestorsInReverse = input.GetInclusiveAncestors().Reverse();
        var sb = new StringBuilder();
        sb.Append(ancestorsInReverse.First().NodeName);

        foreach (var el in ancestorsInReverse.Skip(1))
        {
            sb.Append(" > ");
            sb.Append(el.NodeName);
        }

        return sb.ToString();
    }
}