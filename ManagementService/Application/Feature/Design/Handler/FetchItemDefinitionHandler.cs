using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using AutoMapper;
using Contract.DTO.Abstraction;
using Contract.DTO.Definition.MetaDomain;

namespace Application.Feature.Design.Handler
{
    public class FetchItemDefinitionHandler : IHandler<FetchItemDefinitionCommand, PagedResponseDTO<ItemDefinitionDTO>>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        #endregion

        #region Properties
        #endregion

        public FetchItemDefinitionHandler(
            IUnitOfWork uow,
            IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        #region Methods
        public async Task<PagedResponseDTO<ItemDefinitionDTO>> Handle(
            FetchItemDefinitionCommand command)
        {
            var queries = command.Queries;

            // Enforce safe boundaries defaults
            int pageNumber = queries.PageNumber < 1 ? 1 : queries.PageNumber;
            int pageSize = queries.PageSize < 1 ? 10 : queries.PageSize;

            // Retrieve item definition and paging
            var itemRepo = uow.GetRepository<IItemDefinitionRepository>();
            var (entities, totalCount) = await itemRepo.GetPagedDefinitionsAsync(
                queries?.SearchTerm,
                queries?.Type,
                queries?.Category,
                pageNumber,
                pageSize);

            // Map to result
            var dtos = mapper.Map<List<ItemDefinitionDTO>>(entities);
            return new PagedResponseDTO<ItemDefinitionDTO>(
                dtos,
                totalCount,
                pageNumber,
                pageSize);
        }
        #endregion
    }
}