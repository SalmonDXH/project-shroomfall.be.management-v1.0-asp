using AutoMapper;
using Contract.DTO.Definition;
using Domain;

namespace Application.Mapper
{
    public class VersioningMapper : Profile
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        public VersioningMapper()
        {
            CreateMap<DefinitionVersionLog, DefinitionVersionLogDTO>();
        }

        #region Methods
        #endregion
    }
}