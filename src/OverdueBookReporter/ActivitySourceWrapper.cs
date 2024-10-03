using System.Diagnostics;

namespace Tiesmaster.OverdueBookReporter;

public class ActivitySourceWrapper
{
    public const string SourceName = "Tiesmaster.OverdueBookReporter";

    private static readonly ActivitySource _source = new(SourceName, "1.0.0");

    public static IDisposable StartMainUseCase()
        => _source.StartActivity("Main use case")!;

    public static IDisposable StartGetBookStatus()
        => _source.StartActivity("Get book status")!;

    public static IDisposable StartSendEmail()
        => _source.StartActivity("Send email")!;
}
