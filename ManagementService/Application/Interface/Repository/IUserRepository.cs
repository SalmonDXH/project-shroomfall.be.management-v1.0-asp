using Application.Interface.Repository.Base;
using Domain.IdentityDomain;

namespace Application.Interface.Repository
{
    public interface IUserRepository : IGenericRepository<User>, IRepository
    {
        Task<User?> GetByEmailAsync(
            string email);
        Task<User?> GetBySteamIdAsync(
            string steamId);
    }
}