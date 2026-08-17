using Application.Interface.Repository;
using Domain.IdentityDomain;
using Infrastructure.Persistence;
using Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace Infrastructure.Repository.Relational
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        public UserRepository(RelationalDB context) : base(context) { }

        #region Methods
        public async Task<User?> GetByEmailAsync(
            string email)
        {
            return await dbSet
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetBySteamIdAsync(
            string steamId)
        {
            return await dbSet
                .FirstOrDefaultAsync(x => x.SteamID == steamId);
        }
        #endregion
    }
}