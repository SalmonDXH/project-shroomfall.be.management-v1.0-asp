using AutoMapper;
using Contract.Common;

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