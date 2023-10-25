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

        // TODO: Check if we can get something like a "breadcrumbs" for input

        return attr is string s
            ? s
            : throw new ArgumentException($"Attribute '{name}' doesn't exist on element {input}");
    }
}