using System.Diagnostics.Tracing;

namespace Tiesmaster.OverdueBookReporter;

public class HttpClientRedirectsObserver : EventListener
{
    private readonly List<string> _observedRedirects = [];

    protected override void OnEventSourceCreated(EventSource eventSource)
    {
        if (eventSource.Name == "System.Net.Http")
        {
            EnableEvents(eventSource, EventLevel.Informational);
        }
    }

    protected override void OnEventWritten(EventWrittenEventArgs eventData)
    {
        if (eventData.EventName == "Redirect")
        {
            _observedRedirects.Add((string)eventData.Payload![0]!);
        }
    }

    public IEnumerable<string> PopObservedRedirects()
    {
        var redirects = _observedRedirects.ToArray();
        _observedRedirects.Clear();

        return redirects;
    }
}