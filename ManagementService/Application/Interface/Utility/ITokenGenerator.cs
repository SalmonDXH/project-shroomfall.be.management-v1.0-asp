namespace Application.Interface.Utility
{
    public interface ITokenGenerator
    {
        (string, DateTime) GenerateAccessToken(
            string userId,
            string googleId,
            string role);
        (string, DateTime) GenerateRefreshToken();
    }
}