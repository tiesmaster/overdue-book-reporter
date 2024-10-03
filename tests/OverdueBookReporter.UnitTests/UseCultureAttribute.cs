using System.Globalization;
using System.Reflection;

using Xunit.Sdk;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Xunit;
#pragma warning restore IDE0130 // Namespace does not match folder structure

// Inspired by: https://github.com/xunit/samples.xunit/blob/main/UseCulture/UseCultureAttribute.cs
// Courtesy to the xunit authors

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class UseCultureAttribute(string culture, string uiCulture) : BeforeAfterTestAttribute
{
    private readonly Lazy<CultureInfo> _culture = new(() => new CultureInfo(culture, false));
    private readonly Lazy<CultureInfo> _uiCulture = new(() => new CultureInfo(uiCulture, false));

    private CultureInfo _originalCulture = default!;
    private CultureInfo _originalUICulture = default!;

    public UseCultureAttribute(string culture) : this(culture, culture)
    {
    }

    public CultureInfo Culture => _culture.Value;
    public CultureInfo UICulture => _uiCulture.Value;

    public override void Before(MethodInfo methodUnderTest)
    {
        _originalCulture = Thread.CurrentThread.CurrentCulture;
        _originalUICulture = Thread.CurrentThread.CurrentUICulture;

        Thread.CurrentThread.CurrentCulture = Culture;
        Thread.CurrentThread.CurrentUICulture = UICulture;

        CultureInfo.CurrentCulture.ClearCachedData();
        CultureInfo.CurrentUICulture.ClearCachedData();
    }

    public override void After(MethodInfo methodUnderTest)
    {
        Thread.CurrentThread.CurrentCulture = _originalCulture;
        Thread.CurrentThread.CurrentUICulture = _originalUICulture;

        CultureInfo.CurrentCulture.ClearCachedData();
        CultureInfo.CurrentUICulture.ClearCachedData();
    }
}