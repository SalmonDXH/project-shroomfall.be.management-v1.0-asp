using Application.Feature.Abstraction;
using Application.Feature.Design.Command;
using Application.Interface.Messaging.Publisher;
using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using Application.Service.DesignService;
using Contract;
using Domain;

namespace Application.Feature.Design.Handler
{
    public class UpdateDefinitionHandler : IHandler<UpdateDefinitionCommand>
    {
        #region Attributes
        private readonly CacheBuilder cacheBuilder;
        private readonly IUnitOfWork uow;
        private readonly IDefinitionCachePublisher definitionCachePublisher;
        #endregion

        #region Properties
        #endregion

        public UpdateDefinitionHandler(
            CacheBuilder cacheBuilder,
            IUnitOfWork uow,
            IDefinitionCachePublisher definitionCachePublisher)
        {
            this.cacheBuilder = cacheBuilder;
            this.uow = uow;
            this.definitionCachePublisher = definitionCachePublisher;
        }

        #region Methods
        public async Task Handle(
            UpdateDefinitionCommand command)
        {
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

            // Build cache data
            var definitionCache = await cacheBuilder.BuildAsync(nextVersion);

            // Publish update caching
            await definitionCachePublisher.PublishAsync(definitionCache);

            // TODO: Publish realtime invalidation event
            //eventBus.Publish(new DefinitionUpdatedEvent(key, nextVersion));
        }
        #endregion
    }
}