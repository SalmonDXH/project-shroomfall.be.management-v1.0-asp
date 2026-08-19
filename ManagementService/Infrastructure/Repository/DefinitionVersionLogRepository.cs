using Application.Interface.Repository;
using Domain;
using Infrastructure.Persistence;
using Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Relational
{
    public class DefinitionVersionLogRepository : GenericRepository<DefinitionVersionLog>, IDefinitionVersionLogRepository
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        public DefinitionVersionLogRepository(RelationalDB context) : base(context) { }

        #region Methods
        public async Task<DefinitionVersionLog?> GetLatest(
            string key)
        {
            return await dbSet
                .Where(l => l.Key == key)
                .OrderByDescending(l => l.Version)
                .FirstOrDefaultAsync();
        }
        #endregion
    }
}