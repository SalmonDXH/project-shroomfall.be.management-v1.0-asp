using AutoMapper;
using Contract.DTO.Definition.WorldDomain;
using Domain.WorldDomain;

namespace Application.Mapper
{
    public class WorldDomainMapper : Profile
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        public WorldDomainMapper()
        {
            // Room
            CreateMap<EntitySpawnRule, EntitySpawnRuleDTO>();
            CreateMap<Cell, CellDTO>();
            CreateMap<RoomPresentationDefinition, RoomPresentationDefinitionDTO>();
            CreateMap<RoomDefinition, RoomDefinitionDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ID));
        }

        #region Methods
        #endregion
    }
}