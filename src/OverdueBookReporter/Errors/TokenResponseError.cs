using IdentityModel.Client;

namespace Tiesmaster.OverdueBookReporter.Errors;

public class TokenResponseError : Error
{
    public TokenResponseError(string description, TokenResponse tokenResponse) : base(description)
    {
        Metadata.Add("ErrorType", tokenResponse.ErrorType);
        Metadata.Add("Error", tokenResponse.Error);
        Metadata.Add("ErrorDescription", tokenResponse.ErrorDescription);

        if (tokenResponse.Exception is Exception ex)
        {
            CausedBy(ex);
        }
    }
}