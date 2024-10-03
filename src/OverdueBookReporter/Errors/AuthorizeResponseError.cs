using IdentityModel.Client;

namespace Tiesmaster.OverdueBookReporter.Errors;

public class AuthorizeResponseError : Error
{
    public AuthorizeResponseError(AuthorizeResponse authorizeResponse)
        : base("Failed authorize request")
    {
        Metadata.Add("Error", authorizeResponse.Error);
        Metadata.Add("ErrorDescription", authorizeResponse.ErrorDescription);
    }
}