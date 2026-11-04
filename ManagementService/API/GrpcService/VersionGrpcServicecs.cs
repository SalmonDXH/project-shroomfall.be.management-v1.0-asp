using Application.Interface.Repository;
using Application.Interface.Repository.Base;
using Contract;
using Contract.Grpc.Version;
using Grpc.Core;

namespace API.GrpcService
{
    public class VersionGrpcService : VersionService.VersionServiceBase
    {
        #region Attributes
        private readonly IUnitOfWork uow;
        #endregion

        #region Properties
        #endregion

        public VersionGrpcService(
            IUnitOfWork uow)
        {
            this.uow = uow;
        }

        #region Methods
        public override async Task<VersionCacheResponse> RequestVersionCache(
            VersionCacheRequest request,
            ServerCallContext context)
        {
            var definitionVersionLogRepo = uow.GetRepository<IDefinitionVersionLogRepository>();
            var latest = await definitionVersionLogRepo.GetLatest(Constraint.GLOBAL_DEFINITION_VERSION);

            return new VersionCacheResponse
            {
                Version = latest?.Version ?? 0
            };
        }
        #endregion
    }
}