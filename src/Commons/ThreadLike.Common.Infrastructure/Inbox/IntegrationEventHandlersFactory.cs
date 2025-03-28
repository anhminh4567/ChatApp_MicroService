using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ThreadLike.Common.Application.EventBus;

namespace ThreadLike.Common.Infrastructure.Inbox;

public static class IntegrationEventHandlersFactory
{
	private static readonly ConcurrentDictionary<string, Type[]> HandlersDictionary = new();

	public static IEnumerable<IIntegrationEventHandler> GetHandlers(
		Type integrationEventType,
		IServiceProvider serviceProvider,
		Assembly assembly)
	{
		Type[] integrationEventHandlerTypes = HandlersDictionary.GetOrAdd(
			$"{assembly.GetName().Name}-{integrationEventType.Name}",
			_ =>
			{
				Type[] integrationEventHandlers = assembly.GetTypes()
					.Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler<>).MakeGenericType(integrationEventType)))
					.ToArray();

				return integrationEventHandlers;
			});

		List<IIntegrationEventHandler> handlers = [];
		foreach (Type integrationEventHandlerType in integrationEventHandlerTypes)
		{
			object integrationEventHandler = serviceProvider.GetRequiredService(integrationEventHandlerType);

			handlers.Add((integrationEventHandler as IIntegrationEventHandler)!);
		}

		return handlers;
	}
}
