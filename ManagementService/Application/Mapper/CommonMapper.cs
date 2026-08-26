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
    public class CommonMapper : Profile
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        public CommonMapper()
        {
            CreateMap<HSV, HSV>();
            CreateMap<Vector2, Vector2>();
        }

        #region Methods
        #endregion
    }
}