using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using Contract.DTO.Definition.MetaDomain;
using Contract.Enum.MetaDomain.Item;
using Domain.DomainException;
using Domain.LocalizationDomain;
using Domain.MetaDomain;
using ResponseCode;

namespace Application.Service.DesignService
{
    public class ItemDefinitionService
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly LocalizationEntryFactory localizationEntryFactory;
        #endregion

        #region Properties
        #endregion

        public ItemDefinitionService(
            IUnitOfWork uow,
            LocalizationEntryFactory localizationEntryFactory)
        {
            this.uow = uow;
            this.localizationEntryFactory = localizationEntryFactory;
        }

        #region Methods
        public async Task UpsertWithoutSave(
            ItemDefinitionDTO dto)
        {
            // Validate upsert DTO
            ValidateCategoryConfigurations(dto);

            ValidateRequiredMaterials(dto.Id, dto.RequiredMaterials);

            var itemRepo = uow.GetRepository<IItemDefinitionRepository>();
            var existingItem = await itemRepo.GetByIdAsync(dto.Id);

            // Build category-specific configuration structures
            ConsumableConfig? consumableConfig = null;
            if (dto.ConsumableConfig != null)
            {
                consumableConfig = new ConsumableConfig
                {
                    EffectDefinitionIDs = dto.ConsumableConfig.EffectDefinitionIDs
                };
            }

            EquippableConfig? equippableConfig = null;
            if (dto.EquippableConfig != null)
            {
                equippableConfig = new EquippableConfig
                {
                    Slot = dto.EquippableConfig.Slot,
                    EffectDefinitionIDs = dto.EquippableConfig.EffectDefinitionIDs
                };
            }

            PlaceableConfig? placeableConfig = null;
            if (dto.PlaceableConfig != null)
            {
                placeableConfig = new PlaceableConfig
                {
                    EntityDefinitionID = dto.PlaceableConfig.EntityDefinitionID
                };
            }

            RangedConfig? rangedConfig = null;
            if (dto.RangedConfig != null)
            {
                rangedConfig = new RangedConfig
                {
                    EntityDefinitionID = dto.RangedConfig.EntityDefinitionID
                };
            }

            MeleeConfig? meleeConfig = null;
            if (dto.MeleeConfig != null)
            {
                meleeConfig = new MeleeConfig
                {
                    EntityDefinitionID = dto.MeleeConfig.EntityDefinitionID
                };
            }

            TradingConfig? tradingConfig = null;
            if (dto.TradingConfig != null)
            {
                tradingConfig = new TradingConfig
                {
                    NPCSellerID = dto.TradingConfig.NPCSellerID,
                    Price = dto.TradingConfig.Price
                };
            }

            var costConfig = new CostConfig
            {
                Method = dto.CostConfig.Method,
            };

            if (existingItem == null)
            {
                // CREATE FLOW (Set identity, presentation, and icons ONCE)
                var localizedText = ForItem(dto.Id);
                var presentation = new ItemPresentationDefinition(localizedText, dto.Id);

                var item = new ItemDefinition(
                    dto.Id,
                    dto.Type,
                    dto.Category,
                    dto.MaxStack,
                    dto.MaxDurability,
                    dto.TriggeredAction,
                    presentation,
                    consumableConfig,
                    equippableConfig,
                    placeableConfig,
                    rangedConfig,
                    meleeConfig,
                    tradingConfig,
                    costConfig
                );

                await itemRepo.AddAsync(item);
                await localizationEntryFactory.PreSavePlaceholderKeysAsync(localizedText);
            }
            else
            {
                // UPDATE FLOW 
                existingItem.UpdateFields(
                    dto.Type,
                    dto.Category,
                    dto.MaxStack,
                    dto.MaxDurability,
                    dto.TriggeredAction,
                    consumableConfig,
                    equippableConfig,
                    placeableConfig,
                    rangedConfig,
                    meleeConfig,
                    tradingConfig,
                    costConfig
                );

                await itemRepo.UpdateAsync(existingItem);
            }

            // Map DTO to domain object
            var requiredMaterials = dto.RequiredMaterials
            .Select(x => new RequiredMaterial
            {
                MaterialItemDefinitionID = x.MaterialItemDefinitionID,
                Quantity = x.Quantity
            })
            .ToList();

            await itemRepo.UpsertRequiredMaterialsAsync(dto.Id, requiredMaterials);
        }

        private static void ValidateCategoryConfigurations(
            ItemDefinitionDTO dto)
        {
            var category = dto.Category;
            AssertConfigRule(nameof(dto.ConsumableConfig), category == ItemCategory.Consumable, dto.ConsumableConfig != null);
            AssertConfigRule(nameof(dto.EquippableConfig), category == ItemCategory.Equippable, dto.EquippableConfig != null);
            AssertConfigRule(nameof(dto.PlaceableConfig), category == ItemCategory.Placeable, dto.PlaceableConfig != null);
            AssertConfigRule(nameof(dto.RangedConfig), category == ItemCategory.Ranged, dto.RangedConfig != null);
            AssertConfigRule(nameof(dto.MeleeConfig), category == ItemCategory.Melee, dto.MeleeConfig != null);
        }

        private static void AssertConfigRule(
            string configName,
            bool isTargetCategory,
            bool hasConfig)
        {
            // Triggered if: 
            // 1. It IS the target category but lacks the config (Missing)
            // 2. It IS NOT the target category but has the config (Conflict)
            if ((isTargetCategory && !hasConfig) || (!isTargetCategory && hasConfig))
            {
                var code = isTargetCategory
                    ? ApplicationCode.ItemDefinitionServiceCode.ItemCategoryConfigMissing
                    : ApplicationCode.ItemDefinitionServiceCode.ItemCategoryConfigConflict;

                var message = isTargetCategory
                    ? $"{configName} configuration is required for this item category."
                    : $"{configName} configuration conflicts with this item category.";

                throw new BadRequest(code, message);
            }
        }

        private static void ValidateRequiredMaterials(
            string itemDefinitionId,
            IEnumerable<RequiredMaterialDTO> requiredMaterials)
        {
            var materials = requiredMaterials.ToList();

            if (materials.Any(x => string.IsNullOrWhiteSpace(x.MaterialItemDefinitionID)))
                throw new BadRequest(
                    ApplicationCode.ItemDefinitionServiceCode.InvalidRequiredMaterialsID,
                    "Material item definition ID is required.");

            if (materials.Any(x => x.Quantity <= 0))
                throw new BadRequest(
                    ApplicationCode.ItemDefinitionServiceCode.InvalidRequiredMaterialsQuantity,
                    "Required material quantity must be greater than zero.");

            if (materials.GroupBy(x => x.MaterialItemDefinitionID).Any(group => group.Count() > 1))
                throw new BadRequest(
                    ApplicationCode.ItemDefinitionServiceCode.InvalidRequiredMaterialsDuplicated,
                    "A required material can only appear once per item definition.");

            if (materials.Any(x => string.Equals(x.MaterialItemDefinitionID, itemDefinitionId, StringComparison.OrdinalIgnoreCase)))
                throw new BadRequest(
                    ApplicationCode.ItemDefinitionServiceCode.InvalidRequiredMaterialsSelfCrafted,
                    "An item cannot require itself as a crafting material.");
        }

        private static LocalizedText ForItem(
            string itemId)
        {
            itemId = string.IsNullOrWhiteSpace(itemId) ? "unknown" : itemId.Trim().ToLowerInvariant();

            return new LocalizedText
            {
                NameKey = $"item.{itemId}.name",
                DescriptionKey = $"item.{itemId}.description"
            };
        }
        #endregion
    }
}