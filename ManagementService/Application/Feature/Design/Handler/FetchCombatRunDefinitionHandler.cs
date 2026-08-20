using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using AutoMapper;
using Contract.DTO.Abstraction;
using Contract.DTO.Definition.WorldDomain;

namespace Application.Feature.Design.Handler
{
    public class FetchCombatRunDefinitionHandler : IHandler<FetchCombatRunDefinitionCommand, PagedResponseDTO<CombatRunDefinitionDTO>>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        #endregion

        #region Properties
        #endregion

        public FetchCombatRunDefinitionHandler(
            IUnitOfWork uow,
            IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        #region Methods
        public async Task<PagedResponseDTO<CombatRunDefinitionDTO>> Handle(
            FetchCombatRunDefinitionCommand command)
        {
            var queries = command.Queries;

            // Enforce safe boundaries defaults
            int pageNumber = queries.PageNumber < 1 ? 1 : queries.PageNumber;
            int pageSize = queries.PageSize < 1 ? 10 : queries.PageSize;

            // Retrieve effect definition and paging
            var combatRunRepo = uow.GetRepository<ICombatRunDefinitionRepository>();
            var (entities, totalCount) = await combatRunRepo.GetPagedDefinitionsAsync(
                queries?.SearchTerm,
                pageNumber,
                pageSize);

            // Map to result
            var dtos = mapper.Map<List<CombatRunDefinitionDTO>>(entities);
            return new PagedResponseDTO<CombatRunDefinitionDTO>(
                dtos,
                totalCount,
                pageNumber,
                pageSize);
        }
        #endregion
    }
}