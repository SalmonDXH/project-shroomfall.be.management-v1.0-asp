using Application.Interface.Repository;
using Contract.Enum.MetaDomain.Item;
using Domain.MetaDomain;
using Infrastructure.Persistence;
using Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ItemDefinitionRepository : GenericRepository<ItemDefinition>, IItemDefinitionRepository
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        public ItemDefinitionRepository(ManagementDBContext context) : base(context) { }

        #region Methods
        public async Task<(IEnumerable<ItemDefinition> Items, int TotalCount)> GetPagedDefinitionsAsync(
            string? searchTerm,
            ItemType? type,
            ItemCategory? category,
            int pageNumber,
            int pageSize)
        {
            // Create the queryable shell
            var query = dbSet
                .AsNoTracking()
                .Include(x => x.RequiredMaterials)
                .AsQueryable();

            // Conditionally append dynamic WHERE expressions
            if (type.HasValue)
            {
                query = query.Where(x => x.Type == type.Value);
            }

            if (category.HasValue)
            {
                query = query.Where(x => x.Category == category.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim().ToLower();
                // Matches against Primary ID Key or Owned Type Localization configuration keys
                query = query.Where(x =>
                    x.ID.ToLower().Contains(term) ||
                    x.Presentation.LocalizedText.NameKey.ToLower().Contains(term));
            }

            // Get total count tracking balance before executing pagination splits
            int totalCount = await query.CountAsync();

            // Slicing row constraints using database server execution bounds
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task UpsertRequiredMaterialsAsync(
            string itemDefinitionId,
            IEnumerable<RequiredMaterial> requiredMaterials)
        {
            var requiredMaterialSet = context.Set<RequiredMaterial>();
            var incomingMaterials = requiredMaterials.ToList();

            // Retrieve existing required material of the item (existing recipe)
            var existingMaterials = await requiredMaterialSet
                .Where(x => x.ItemDefinitionID == itemDefinitionId)
                .ToListAsync();

            // Indexing
            var incomingById = incomingMaterials.ToDictionary(x => x.MaterialItemDefinitionID);
            var existingById = existingMaterials.ToDictionary(x => x.MaterialItemDefinitionID);

            // Remove the existing recipe
            foreach (var existing in existingMaterials)
            {
                if (!incomingById.ContainsKey(existing.MaterialItemDefinitionID))
                {
                    requiredMaterialSet.Remove(existing);
                }
            }

            // Add new recipe
            foreach (var incoming in incomingMaterials)
            {
                if (existingById.TryGetValue(incoming.MaterialItemDefinitionID, out var existing))
                {
                    existing.Quantity = incoming.Quantity;
                }
                else
                {
                    await requiredMaterialSet.AddAsync(new RequiredMaterial
                    {
                        ItemDefinitionID = itemDefinitionId,
                        MaterialItemDefinitionID = incoming.MaterialItemDefinitionID,
                        Quantity = incoming.Quantity
                    });
                }
            }
        }
        #endregion
    }
}