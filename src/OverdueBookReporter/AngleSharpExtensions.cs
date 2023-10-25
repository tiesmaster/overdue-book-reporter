using System.Text;

using AngleSharp.Dom;

namespace Tiesmaster.OverdueBookReporter;

public static class AngleSharpExtensions
{
    public static IElement QuerySelectorOrThrow(this IDocument input, string selectors)
    {
        var element = input.QuerySelector(selectors);

        return element is IElement el
            ? el
            : throw new InvalidOperationException($"Unable to locate the given element based on the given selectors '{selectors}'");
    }

    public static IElement QuerySelectorOrThrow(this IElement input, string selectors)
    {
        var element = input.QuerySelector(selectors);

        return element is IElement el
            ? el
            : throw new InvalidOperationException($"Unable to locate the given element based on the given selectors '{selectors}'");
    }

    public static string GetAttributeOrThrow(this IElement input, string name)
    {
        var attr = input.GetAttribute(name);

        return attr is string s
            ? s
            : throw new ArgumentException($"Attribute '{name}' doesn't exist on element {input.GetPath()}");
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