using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using AutoMapper;
using Contract.DTO.Abstraction;
using Contract.DTO.Definition.LocalizationDomain;

namespace Application.Feature.Design.Handler
{
    public class FetchLocalizationEntryHandler : IHandler<FetchLocalizationEntryCommand, PagedResponseDTO<LocalizationEntryDTO>>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        #endregion

        #region Properties
        #endregion

        public FetchLocalizationEntryHandler(
            IUnitOfWork uow,
            IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        #region Methods
        public async Task<PagedResponseDTO<LocalizationEntryDTO>> Handle(
            FetchLocalizationEntryCommand command)
        {
            var queries = command.Queries;

            // Enforce safe boundaries defaults
            int pageNumber = queries.PageNumber < 1 ? 1 : queries.PageNumber;
            int pageSize = queries.PageSize < 1 ? 10 : queries.PageSize;

            // Retrieve localization entries and paging
            var localizationRepo = uow.GetRepository<ILocaleRepository>();
            var (entities, totalCount) = await localizationRepo.GetPagedDefinitionsAsync(
                queries.SearchTerm,
                queries.LocaleCode,
                pageNumber,
                pageSize);

            // Map to result
            var dtos = mapper.Map<List<LocalizationEntryDTO>>(entities);
            return new PagedResponseDTO<LocalizationEntryDTO>(
                dtos,
                totalCount,
                pageNumber,
                pageSize);
        }
        #endregion
    }
}