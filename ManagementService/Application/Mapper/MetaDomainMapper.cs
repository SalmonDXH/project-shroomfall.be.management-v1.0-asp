using AutoMapper;
using Contract.DTO.Definition.MetaDomain;
using Domain.MetaDomain;

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