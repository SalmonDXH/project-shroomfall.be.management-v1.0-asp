using Google.Apis.Auth;

namespace Application.Interface.Utility
{
    public interface IGoogleTokenValidator
    {
        Task<GoogleJsonWebSignature.Payload?> ValidateTokenAsync(
            string idToken);
    }
}