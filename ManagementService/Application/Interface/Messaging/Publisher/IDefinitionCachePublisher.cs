using Contract.DTO.Messaging;

namespace Application.Interface.Messaging.Publisher
{
    public interface IDefinitionCachePublisher
    {
        Task PublishAsync(
            DefinitionCacheDTO snapshot,
            CancellationToken cancellationToken = default);
    }
}