using Domain.Abstraction;

namespace Application.Interface.Repository.Base
{
    public interface IGenericRepository<T>
        where T : class
    {
        Task<T?> GetByIdAsync<TKey>(
            TKey id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(
            T entity);
        Task UpdateAsync(
            T entity);
        Task DeleteAsync<TKey>(
            TKey id);
    }

    public interface IDefinitionRepository<T> : IGenericRepository<T>
        where T : ComponentDefinition
    {
        Task<T?> GetByEntityIdAsync(string entityDefinitionId);
        Task UpsertAsync(T entity);
    }
}