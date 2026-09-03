using Application.Interface.Messaging.Publisher;
using Contract.DTO.Messaging;
using MassTransit;

namespace Infrastructure.Messaging.Publisher
{
    public class GameStartupPublisher : IGameStartupPublisher
    {
        #region Attributes
        private readonly IPublishEndpoint publishEndpoint;
        #endregion

        #region Properties
        #endregion

        public GameStartupPublisher(
            IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        #region Methods
        public async Task PublishAsync(
            GameStartupDTO startup,
            CancellationToken cancellationToken = default)
        {
            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            await publishEndpoint.Publish(
                startup,
                cancellationToken);
        }
        #endregion
    }
}