using Application.Interface.Repository.Base;
using Domain.WorldDomain;

namespace Application.Interface.Repository
{
    public interface IRoomDefinitionRepository : IGenericRepository<RoomDefinition>, IRepository
    {
        Task<(IEnumerable<RoomDefinition> Items, int TotalCount)> GetPagedDefinitionsAsync(
            string? searchTerm,
            RoomType? type,
            int pageNumber,
            int pageSize);
        Task UpsertChildrenAsync(
            string roomDefinitionId,
            IEnumerable<Cell> cells,
            IEnumerable<EntitySpawnRule> spawnRules);
    }
}