using AutoMapper;
using Contract.Common;
using Contract.DTO.Abstraction;
using Contract.DTO.Definition;
using Contract.DTO.Definition.EntityDomain.Component;
using Contract.DTO.Definition.IdentityDomain;
using Contract.DTO.Definition.LocalizationDomain;
using Contract.DTO.Definition.MetaDomain;
using Contract.DTO.Definition.WorldDomain;
using Domain;
using Domain.Abstraction;
using Domain.EntityDomain;
using Domain.EntityDomain.Component;
using Domain.IdentityDomain;
using Domain.LocalizationDomain;
using Domain.MetaDomain;
using Domain.WorldDomain;

namespace Application.Mapper
{
    public class EntityDomainMapper : Profile
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        public EntityDomainMapper()
        {
            CreateMap<ComponentDefinition, ComponentDefinitionDTO>()
                .Include<AIDefinition, AIDefinitionDTO>()
                .Include<AppearanceDefinition, AppearanceDefinitionDTO>()
                .Include<CharacteristicDefinition, CharacteristicDefinitionDTO>()
                .Include<CollisionDefinition, CollisionDefinitionDTO>()
                .Include<InventoryDefinition, InventoryDefinitionDTO>()
                .Include<LifetimeDefinition, LifetimeDefinitionDTO>()
                .Include<ProjectileDefinition, ProjectileDefinitionDTO>()
                .Include<TriggeredEffectDefinition, TriggeredEffectDefinitionDTO>();

            // AI Definition
            CreateMap<AIDefinition, AIDefinitionDTO>();

            // Appearance Definition
            CreateMap<AppearanceDefinition, AppearanceDefinitionDTO>();

            // Collision Definition
            CreateMap<CollisionDefinition, CollisionDefinitionDTO>();

            // Characteristic Definition
            CreateMap<CharacteristicDefinition, CharacteristicDefinitionDTO>();
            CreateMap<AttributeValue, AttributeValueDTO>();
            CreateMap<AttributeGrowthValue, AttributeGrowthValueDTO>();

            // Inventory Definition
            CreateMap<InventoryDefinition, InventoryDefinitionDTO>();
            CreateMap<InventoryEntry, InventoryEntryDTO>();

            // Lifetime Definition
            CreateMap<LifetimeDefinition, LifetimeDefinitionDTO>();

            // Projectile Definition
            CreateMap<ProjectileDefinition, ProjectileDefinitionDTO>();

            // Triggered Effect Definition
            CreateMap<TriggeredEffectDefinition, TriggeredEffectDefinitionDTO>();

            // Entity
            CreateMap<EntityPresentationDefinition, EntityPresentationDefinitionDTO>();
            CreateMap<EntityDefinition, EntityDefinitionDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ID));

        }

        #region Methods
        #endregion
    }
}