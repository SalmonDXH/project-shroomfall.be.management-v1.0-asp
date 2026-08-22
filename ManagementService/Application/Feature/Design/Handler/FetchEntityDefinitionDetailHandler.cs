using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using Application.Service.DesignService;
using AutoMapper;
using Contract.DTO.Abstraction;
using Contract.DTO.Definition.EntityDomain.Component;
using Domain.DomainException;
using ResponseCode;

namespace Application.Feature.Design.Handler
{
    public class FetchEntityDefinitionDetailHandler : IHandler<FetchEntityDefinitionDetailCommand, EntityDefinitionDTO>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly ComponentDiscoveryRegistry discoveryRegistry;
        #endregion

        #region Properties
        #endregion

        public FetchEntityDefinitionDetailHandler(
            IUnitOfWork uow,
            IMapper mapper,
            ComponentDiscoveryRegistry discoveryRegistry)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.discoveryRegistry = discoveryRegistry;
        }

        #region Methods
        public async Task<EntityDefinitionDTO> Handle(
            FetchEntityDefinitionDetailCommand command)
        {
            // Retrieve entity definition
            var entityRepo = uow.GetRepository<IEntityDefinitionRepository>();
            var rootEntity = await entityRepo.GetByIdAsync(command.ID);
            if (rootEntity == null)
                throw new NotFound(
                    ApplicationCode.DesignHandlerCode.EntityDefinitionNotFound,
                    $"Entity variant definition configuration targets containing the ID '{command.ID}' could not be resolved.");

            // Retrieve components of the entity
            var componentList = new List<ComponentDefinitionDTO>();
            foreach (var component in discoveryRegistry.GetComponents())
            {
                // Resolve component repository
                var repositoryInstance = component.GetRepositoryMethod.Invoke(uow, null);
                if (repositoryInstance == null)
                    continue;

                // Retrieve component data
                var task = (Task)component.GetByEntityIdMethod.Invoke(repositoryInstance, new object[] { command.ID })!;
                await task;

                // Map result to DTO
                var domainComponent = task.GetType().GetProperty("Result")?.GetValue(task);
                if (domainComponent != null)
                {
                    var mappedDto = mapper.Map(domainComponent, domainComponent.GetType(), component.DtoType);
                    if (mappedDto is ComponentDefinitionDTO componentDto)
                    {
                        componentList.Add(componentDto);
                    }
                }
            }

            // Map to result
            var detailDto = mapper.Map<EntityDefinitionDTO>(rootEntity);
            detailDto.Components = componentList;
            return detailDto;
        }
        #endregion
    }
}