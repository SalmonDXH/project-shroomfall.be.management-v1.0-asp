using Application.Feature.Abstraction;
using Application.Features.Design.Command;
using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using AutoMapper;
using Contract.DTO.Abstraction;
using Contract.DTO.Definition.WorldDomain;

namespace Application.Feature.Design.Handler
{
    public class FetchRoomDefinitionHandler : IHandler<FetchRoomDefinitionCommand, PagedResponseDTO<RoomDefinitionDTO>>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        #endregion

        #region Properties
        #endregion

        public FetchRoomDefinitionHandler(
            IUnitOfWork uow,
            IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        #region Methods
        public async Task<PagedResponseDTO<RoomDefinitionDTO>> Handle(
            FetchRoomDefinitionCommand command)
        {
            var queries = command.Queries;

            // Enforce safe boundaries defaults
            int pageNumber = queries.PageNumber < 1 ? 1 : queries.PageNumber;
            int pageSize = queries.PageSize < 1 ? 10 : queries.PageSize;

            // Retrieve room definition and paging
            var roomRepo = uow.GetRepository<IRoomDefinitionRepository>();
            var (entities, totalCount) = await roomRepo.GetPagedDefinitionsAsync(
                queries?.SearchTerm,
                queries?.Type,
                pageNumber,
                pageSize);

            // Map to result
            var dtos = mapper.Map<List<RoomDefinitionDTO>>(entities);
            return new PagedResponseDTO<RoomDefinitionDTO>(
                dtos,
                totalCount,
                pageNumber,
                pageSize);
        }
        #endregion
    }
}