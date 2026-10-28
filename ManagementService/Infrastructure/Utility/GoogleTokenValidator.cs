using Application.Interface.Utility;
using Domain.DomainException;
using Google.Apis.Auth;
using ResponseCode;

namespace Infrastructure.Utility
{
    public class GoogleTokenValidator : IGoogleTokenValidator
    {
        #region Attributes
        private readonly string googleClientId;
        #endregion

        #region Properties
        #endregion

        public GoogleTokenValidator(
            string googleClientId)
        {
            this.googleClientId = googleClientId;
        }

        #region Methods
        public async Task<GoogleJsonWebSignature.Payload?> ValidateTokenAsync(
            string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new[] { googleClientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                return payload; // Contains Email, Name, Subject (Google ID/Sub), etc.
            }
            catch (Exception ex)
            {
                throw new Unauthorized(
                    InfrastructureCode.GoogleTokenValidatorCode.GoogleValidatationFailed,
                    $"Google token validation failed: {ex.Message}");
            }
        }
        #endregion
    }
}