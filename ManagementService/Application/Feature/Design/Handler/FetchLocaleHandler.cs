using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using AutoMapper;
using Contract.DTO.Definition.LocalizationDomain;

namespace Application.Feature.Design.Handler
{
    public class FetchLocaleHandler : IHandler<FetchLocaleCommand, List<LocaleDTO>>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        #endregion

        #region Properties
        #endregion

        public FetchLocaleHandler(
            IUnitOfWork uow,
            IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        #region Methods
        public async Task<List<LocaleDTO>> Handle(
            FetchLocaleCommand command)
        {
            // Retrieve locale
            var localeRepo = uow.GetRepository<ILocaleRepository>();
            var entities = await localeRepo.GetAllAsyncWithoutJoined();

            // Map to result
            return mapper.Map<List<LocaleDTO>>(entities);
        }
        #endregion
    }
}