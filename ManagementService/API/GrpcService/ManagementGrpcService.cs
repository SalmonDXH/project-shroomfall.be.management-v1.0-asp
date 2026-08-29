using Application.Interface.Messaging.Publisher;
using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using Application.Service.DesignService;
using Contract;
using Contract.DTO.Messaging;
using Contract.Grpc.Management;
using Grpc.Core;

namespace API.GrpcService
{
    public class ManagementGrpcService : DefinitionService.DefinitionServiceBase
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        private readonly CacheBuilder cacheBuilder;
        private readonly IGameStartupPublisher gameStartupPublisher;
        #endregion

        #region Properties
        #endregion

        public ManagementGrpcService(
            IUnitOfWork uow,
            CacheBuilder cacheBuilder,
            IGameStartupPublisher gameStartupPublisher)
        {
            this.uow = uow;
            this.cacheBuilder = cacheBuilder;
            this.gameStartupPublisher = gameStartupPublisher;
        }

        #region Methods
        public override async Task<DefinitionCacheResponse> RequestDefinitionCache(
            DefinitionCacheRequest request,
            ServerCallContext context)
        {
            // Get current version
            var definitionVersionLogRepo = uow.GetRepository<IDefinitionVersionLogRepository>();
            var latest = await definitionVersionLogRepo.GetLatest(Constraint.GLOBAL_DEFINITION_VERSION);
            var version = latest?.Version ?? 0;

            // Start the game service with cache data
            var definitionCache = await cacheBuilder.BuildAsync(version);
            await gameStartupPublisher.PublishAsync(new GameStartupDTO() { DefinitionCache = definitionCache }, context.CancellationToken);

            return new DefinitionCacheResponse
            {
                Accepted = true
            };
        }
        #endregion
    }
}