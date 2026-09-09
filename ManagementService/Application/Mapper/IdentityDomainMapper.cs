using AutoMapper;
using Contract.DTO.Definition.IdentityDomain;
using Domain.IdentityDomain;

namespace Application.Mapper
{
    public class IdentityDomainMapper : Profile
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        public IdentityDomainMapper()
        {
            CreateMap<User, UserDTO>();
        }

        #region Methods
        #endregion
    }
}