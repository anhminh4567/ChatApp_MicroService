using MassTransit;
using ThreadLike.Common.Application.EventBus;
using ThreadLike.Common.Contracts.Abstracts;

namespace ThreadLike.Common.Infrastructure.EventBuses;
public class EventBus : IEventBus
{
	private readonly IBus _bus;

	public EventBus(IBus bus)
	{
		_bus = bus;
	}

	public Task PublishAsync<T>(T integrationEvent, CancellationToken token = default) where T : IIntegrationEvent
	{
		return _bus.Publish(integrationEvent, token);
	}
}
