using Application.Interface.Repository.Base;
using Domain.Abstraction;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Base
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        #region Attributes
        protected readonly DbSet<T> dbSet;
        protected readonly ManagementDBContext context;
        #endregion

        #region Properties
        #endregion

        public GenericRepository(
            ManagementDBContext context)
        {
            dbSet = context.Set<T>();
            this.context = context;
        }

        #region Methods
        public async Task<T?> GetByIdAsync<TKey>(
            TKey id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(
            T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(
            T entity)
        {
            dbSet.Update(entity);
        }

        public async Task DeleteAsync<TKey>(
            TKey id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return;

            dbSet.Remove(entity);
        }
        #endregion
    }

    public class DefinitionRepository<T> : GenericRepository<T>, IDefinitionRepository<T>
        where T : ComponentDefinition
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        public DefinitionRepository(ManagementDBContext context) : base(context) { }

        #region Methods
        public virtual async Task<T?> GetByEntityIdAsync(
            string entityDefinitionId)
        {
            return await dbSet.FirstOrDefaultAsync(x => x.EntityDefinitionID == entityDefinitionId);
        }

        public virtual async Task UpsertAsync(T entity)
        {
            var existing = await dbSet.FirstOrDefaultAsync(x => x.EntityDefinitionID == entity.EntityDefinitionID);

            if (existing != null)
            {
                dbSet.Remove(existing);
            }

            await dbSet.AddAsync(entity);
        }
        #endregion
    }
}