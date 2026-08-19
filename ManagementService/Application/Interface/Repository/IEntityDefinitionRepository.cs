using Application.Interface.Repository.Base;
using Contract.Enum.EntityDomain;
using Domain.EntityDomain;

namespace Application.Interface.Repository
{
    public interface IEntityDefinitionRepository : IGenericRepository<EntityDefinition>, IRepository
    {
        Task<(IEnumerable<EntityDefinition> Items, int TotalCount)> GetPagedDefinitionsAsync(
            string? searchTerm,
            EntityType? type,
            int pageNumber,
            int pageSize);
    }
}