namespace Application.Interface.Utility
{
    public interface ITokenGenerator
    {
        string GenerateAccessToken(
            string userId,
            string steamId,
            string role);
        string GenerateRefreshToken();
        DateTime GetRefreshTokenExpiry();
    }
}