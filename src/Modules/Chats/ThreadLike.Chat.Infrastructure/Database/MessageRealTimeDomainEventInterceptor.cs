using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Messages;
using ThreadLike.Chat.Domain.Shared;
using ThreadLike.Chat.Infrastructure.Hubs;
using ThreadLike.Common.Domain;
using ThreadLike.Common.Infrastructure.Outbox;
using ThreadLike.Common.Infrastructure.Serialization;

namespace ThreadLike.Chat.Infrastructure.Database
{
	internal class MessageRealTimeDomainEventInterceptor(
		IHubContext<GroupChatHub,IGroupChatClient> groupchatHub) : SaveChangesInterceptor
	{

		public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
		{
			if (eventData.Context is not null)
			{
				Microsoft.EntityFrameworkCore.DbContext context = eventData.Context;
				var outboxMessages = context
					.ChangeTracker
					.Entries<Entity>()
					.Select(entry => entry.Entity)
					.SelectMany(entity =>
					{
						var fastDomainEvents = entity.DomainEvents
							.Where(domainEvent => domainEvent is IMessageRealTimeDomainEvent)
							.Cast<IMessageRealTimeDomainEvent>()
							.ToList();

						fastDomainEvents.ForEach(x => entity.RemoveEvent(x.Id));

						return fastDomainEvents;
					}).ToList();

				foreach (IMessageRealTimeDomainEvent fastDomainEvent in outboxMessages)
				{
					if (fastDomainEvent.GroupId != Guid.Empty)
					{
						await groupchatHub.Clients.Group(fastDomainEvent.GroupId.ToString()).ReceiveGroupMessage(
							fastDomainEvent.GroupId,
							new List<Message> { fastDomainEvent.SentMessage }
						);
					}
				}
			}
			return await base.SavingChangesAsync(eventData, result, cancellationToken);
		}
	}
}
