using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using Contract;
using Domain;

namespace Application.Feature.Design.Handler
{
    public class UpdateDefinitionHandler : IHandler<UpdateDefinitionCommand>
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        //private readonly ICacheProvider cacheLoader;
        //private readonly IEventBus eventBus;
        #endregion

        #region Properties
        #endregion

        public UpdateDefinitionHandler(
            IUnitOfWork uow
            //ICacheProvider cacheLoader,
            //IEventBus eventBus
            )
        {
            this.uow = uow;
            //this.cacheLoader = cacheLoader;
            //this.eventBus = eventBus;
        }

        #region Methods
        public async Task Handle(
            UpdateDefinitionCommand command)
        {
            // TODO: Add GRPC or Queue Messaging to handle this command in a distributed system
            var dto = command.DTO;

            // Resolve repository
            var definitionVersionLogRepo = uow.GetRepository<IDefinitionVersionLogRepository>();

            // Generate latest version for this key
            var key = dto.Key ?? Constraint.GLOBAL_DEFINITION_VERSION;
            var latest = await definitionVersionLogRepo.GetLatest(key);
            var nextVersion = latest == null ? 1 : latest.Version + 1;

            // Apply domain - Create new version log
            var log = new DefinitionVersionLog(
                Guid.NewGuid().ToString(),
                key,
                nextVersion,
                dto.Description);

            // Apply peristence - Save changes
            await definitionVersionLogRepo.AddAsync(log);
            await uow.SaveChangesAsync();

            // Reload cache - Note: improve by using key to reload needed cache only
            //await cacheLoader.LoadAllAsync();

            // Publish realtime invalidation event
            //eventBus.Publish(new DefinitionUpdatedEvent(key, nextVersion));
        }
        #endregion
    }
}