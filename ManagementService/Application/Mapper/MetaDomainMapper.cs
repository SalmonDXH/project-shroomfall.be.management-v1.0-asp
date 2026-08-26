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
    public class MetaDomainMapper : Profile
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        public MetaDomainMapper()
        {
            // Effect
            CreateMap<EffectPresentationDefinition, EffectPresentationDefinitionDTO>();
            CreateMap<EffectDefinition, EffectDefinitionDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ID));

            // Item
            CreateMap<ConsumableConfig, ConsumableConfigDTO>();
            CreateMap<EquippableConfig, EquippableConfigDTO>();
            CreateMap<PlaceableConfig, PlaceableConfigDTO>();
            CreateMap<RangedConfig, RangedConfigDTO>();
            CreateMap<MeleeConfig, MeleeConfigDTO>();
            CreateMap<CostConfig, CostConfigDTO>();
            CreateMap<ItemPresentationDefinition, ItemPresentationDefinitionDTO>();
            CreateMap<ItemDefinition, ItemDefinitionDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ID));
        }

        #region Methods
        #endregion
    }
}