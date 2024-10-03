using IdentityModel.Client;

using Tiesmaster.OverdueBookReporter.Errors;

namespace Tiesmaster.OverdueBookReporter;

public static class ResultExtensions
{
    public static async Task<Result> ToFailedResult(this HttpResponseMessage httpResponse, string description)
    {
        var responseBody = await httpResponse.Content.ReadAsStringAsync();
        return new HttpResponseError(description, httpResponse.StatusCode, responseBody);
    }

    public static Result ToFailedResult(this TokenResponse tokenResponse, string description)
        => new TokenResponseError(description, tokenResponse);

    public static Result ToFailedResult(this AuthorizeResponse authorizeResponse)
        => new AuthorizeResponseError(authorizeResponse);
}