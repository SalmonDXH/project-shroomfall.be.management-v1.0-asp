using Application.Interface.Repository.Base;
using Domain.MetaDomain;

namespace Application.Interface.Repository
{
    public interface IItemDefinitionRepository : IGenericRepository<ItemDefinition>, IRepository
    {
        Task<(IEnumerable<ItemDefinition> Items, int TotalCount)> GetPagedDefinitionsAsync(
            string? searchTerm,
            ItemType? type,
            ItemCategory? category,
            int pageNumber,
            int pageSize);
    }
}