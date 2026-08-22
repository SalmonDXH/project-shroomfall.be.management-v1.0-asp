using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using AutoMapper;
using Contract.DTO.Abstraction;
using Contract.DTO.Definition.MetaDomain;

namespace Application.Feature.Design.Handler
{
    public class FetchEffectDefinitionHandler : IHandler<FetchEffectDefinitionCommand, PagedResponseDTO<EffectDefinitionDTO>>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        #endregion

        #region Properties
        #endregion

        public FetchEffectDefinitionHandler(
            IUnitOfWork uow,
            IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        #region Methods
        public async Task<PagedResponseDTO<EffectDefinitionDTO>> Handle(
            FetchEffectDefinitionCommand command)
        {
            var queries = command.Queries;

            // Enforce safe boundaries defaults
            int pageNumber = queries.PageNumber < 1 ? 1 : queries.PageNumber;
            int pageSize = queries.PageSize < 1 ? 10 : queries.PageSize;

            // Retrieve effect definition and paging
            var effectRepo = uow.GetRepository<IEffectDefinitionRepository>();
            var (entities, totalCount) = await effectRepo.GetPagedDefinitionsAsync(
                queries?.SearchTerm,
                queries?.Type,
                queries?.AttributeType,
                pageNumber,
                pageSize);

            // Map to result
            var dtos = mapper.Map<List<EffectDefinitionDTO>>(entities);
            return new PagedResponseDTO<EffectDefinitionDTO>(
                dtos,
                totalCount,
                pageNumber,
                pageSize);
        }
        #endregion
    }
}