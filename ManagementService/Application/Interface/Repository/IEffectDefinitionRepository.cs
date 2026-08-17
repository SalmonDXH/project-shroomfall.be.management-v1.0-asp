using Application.Interface.Repository.Base;
using Domain.MetaDomain;

namespace Application.Interface.Repository
{
    public interface IEffectDefinitionRepository : IGenericRepository<EffectDefinition>, IRepository
    {
        Task<(IEnumerable<EffectDefinition> Items, int TotalCount)> GetPagedDefinitionsAsync(
            string? searchTerm,
            EffectType? type,
            AttributeType? attributeType,
            int pageNumber,
            int pageSize);
    }
}