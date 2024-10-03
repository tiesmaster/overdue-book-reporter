using System.Net;

namespace Tiesmaster.OverdueBookReporter.Errors;

public class HttpResponseError : Error
{
    public HttpResponseError(string description, HttpStatusCode statusCode, string responseBody)
        : base(description)
    {
        Metadata.Add("HttpStatusCode", statusCode);
        Metadata.Add("ResponseBody", responseBody);
    }
}