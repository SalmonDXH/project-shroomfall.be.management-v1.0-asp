using Application.Interface.Repository.Base;
using Domain.EntityDomain.Component;

namespace Application.Interface.Repository
{
    public interface ICharacteristicDefinitionRepository : IDefinitionRepository<CharacteristicDefinition>, IRepository
    {
        Task SaveAttributeValuesAsync(
            IEnumerable<AttributeValue> attributeValues);
        Task SaveAttributeGrowthValuesAsync(
            IEnumerable<AttributeGrowthValue> growthValues);
        Task ReplaceAttributeValuesAsync(
            Guid characteristicId,
            IEnumerable<AttributeValue> newValues);
        Task ReplaceAttributeGrowthValuesAsync(
            Guid attributeValueId,
            IEnumerable<AttributeGrowthValue> newGrowths);
    }
}