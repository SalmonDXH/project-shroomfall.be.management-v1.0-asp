namespace Application.Interface.Utility
{
    public interface ISteamValidator
    {
        Task<string?> ValidateTicket(
            string ticket);
    }
}