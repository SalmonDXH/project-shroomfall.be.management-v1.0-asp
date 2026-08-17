using Application.Interface.Repository.Base;
using Domain.WorldDomain;

namespace Application.Interface.Repository
{
    public interface ICombatRunDefinitionRepository : IGenericRepository<CombatRunDefinition>, IRepository
    {
        Task UpsertFloorsAsync(
            string combatRunDefinitionId,
            IEnumerable<Floor> floors);
        Task<(IEnumerable<CombatRunDefinition> Items, int TotalCount)> GetPagedDefinitionsAsync(
            string? searchTerm,
            int pageNumber,
            int pageSize);
    }
}