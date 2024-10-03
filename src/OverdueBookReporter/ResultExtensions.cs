using Tiesmaster.OverdueBookReporter.Errors;

namespace Tiesmaster.OverdueBookReporter;

public static class ResultExtensions
{
    public static async Task<Result> ToFailedResult(this HttpResponseMessage response, string description)
    {
        var responseBody = await response.Content.ReadAsStringAsync();
        return new HttpResponseError(description, response.StatusCode, responseBody);
    }
}