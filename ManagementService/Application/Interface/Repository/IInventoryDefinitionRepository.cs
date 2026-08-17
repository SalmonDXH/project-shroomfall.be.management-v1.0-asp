using Application.Interface.Repository.Base;
using Domain.EntityDomain.Component;

namespace Application.Interface.Repository
{
    public interface IInventoryDefinitionRepository : IDefinitionRepository<InventoryDefinition>, IRepository
    {
        Task SaveDefaultItemsAsync(
            IEnumerable<InventoryEntry> defaultItems);
        Task ReplaceDefaultItemsAsync(
            Guid inventoryDefinitionId,
            IEnumerable<InventoryEntry> newItems);
    }
}