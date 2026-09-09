using AutoMapper;
using Contract.DTO.Definition.LocalizationDomain;
using Domain.LocalizationDomain;

namespace Application.Mapper
{
    public class LocalizationDomainMapper : Profile
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        public LocalizationDomainMapper()
        {
            CreateMap<LocalizationEntry, LocalizationEntryDTO>();
            CreateMap<LocalizedText, LocalizedTextDTO>();
            CreateMap<Locale, LocaleDTO>();
        }

        #region Methods
        #endregion
    }
}