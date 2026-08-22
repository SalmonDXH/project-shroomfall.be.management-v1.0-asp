using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using AutoMapper;
using Contract.DTO.Abstraction;
using Contract.DTO.Definition.EntityDomain.Component;

namespace Application.Feature.Design.Handler
{
    public class FetchEntityDefinitionHandler : IHandler<FetchEntityDefinitionCommand, PagedResponseDTO<EntityDefinitionDTO>>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        #endregion

        #region Properties
        #endregion

        public FetchEntityDefinitionHandler(
            IUnitOfWork uow,
            IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        #region Methods
        public async Task<PagedResponseDTO<EntityDefinitionDTO>> Handle(
            FetchEntityDefinitionCommand command)
        {
            var queries = command.Queries;

            // Enforce safe boundaries defaults
            int pageNumber = queries.PageNumber < 1 ? 1 : queries.PageNumber;
            int pageSize = queries.PageSize < 1 ? 10 : queries.PageSize;

            // Retrieve entity definition and paging
            var entityRepo = uow.GetRepository<IEntityDefinitionRepository>();
            var (entities, totalCount) = await entityRepo.GetPagedDefinitionsAsync(
                queries.SearchTerm,
                queries.EntityType,
                pageNumber,
                pageSize);

            // Map to result
            var mappedItems = mapper.Map<List<EntityDefinitionDTO>>(entities);
            return new PagedResponseDTO<EntityDefinitionDTO>(
                mappedItems,
                totalCount,
                pageNumber,
                pageSize);
        }
        #endregion
    }
}