namespace Application.Interface.Utility
{
    public interface ITokenGenerator
    {
        (string, DateTime) GenerateAccessToken(
            string userId,
            string steamId,
            string role);
        (string, DateTime) GenerateRefreshToken();
    }
}