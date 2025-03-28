using ThreadLike.Common.Contracts.Abstracts;

namespace ThreadLike.Common.Application.EventBus;

// implemented in ----------------------- Common.Infrastructure -----------------------
public interface IEventBus
{
	Task PublishAsync<T>(T integrationEvent, CancellationToken token = default) where T : IIntegrationEvent;

}

