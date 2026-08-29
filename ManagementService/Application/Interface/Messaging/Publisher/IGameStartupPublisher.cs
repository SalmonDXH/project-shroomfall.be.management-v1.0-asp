using Contract.DTO.Messaging;

namespace Application.Interface.Messaging.Publisher
{
    public interface IGameStartupPublisher
    {
        Task PublishAsync(
            GameStartupDTO startup,
            CancellationToken cancellationToken = default);
    }
}