using Application.Interface.Messaging.Publisher;
using Contract.DTO.Messaging;
using MassTransit;

namespace Infrastructure.Messaging.Publisher
{
    public class DefinitionCachePublisher : IDefinitionCachePublisher
    {
        #region Attributes
        private readonly IPublishEndpoint publishEndpoint;
        #endregion

        #region Properties
        #endregion

        public DefinitionCachePublisher(
            IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        #region Methods
        public async Task PublishAsync(
            DefinitionCacheDTO snapshot,
            CancellationToken cancellationToken = default)
        {
            await publishEndpoint.Publish(
                snapshot,
                cancellationToken);
        }
        #endregion
    }
}