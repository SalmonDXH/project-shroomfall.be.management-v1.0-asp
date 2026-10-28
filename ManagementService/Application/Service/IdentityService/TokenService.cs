using Application.Interface.Utility;
using Contract.DTO.Feature.Identity.Response;
using Domain.IdentityDomain;

namespace Application.Service.IdentityService
{
    public class TokenService
    {
        #region Attributes
        private readonly ITokenGenerator tokenGenerator;
        #endregion

        #region Properties
        #endregion

        public TokenService(
            ITokenGenerator tokenGenerator)
        {
            this.tokenGenerator = tokenGenerator;
        }

        #region Methods
        public TokenDTO Generate(
            User user)
        {
            var (accessToken, accessExpiry) = tokenGenerator.GenerateAccessToken(user.ID, user.GoogleID ?? "", user.Role.ToString());
            var (refreshToken, refreshExpiry) = tokenGenerator.GenerateRefreshToken();
            user.SetRefreshToken(refreshToken, refreshExpiry);

            return new TokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessExpiry = accessExpiry,
                RefreshExpiry = refreshExpiry
            };
        }
        #endregion
    }
}