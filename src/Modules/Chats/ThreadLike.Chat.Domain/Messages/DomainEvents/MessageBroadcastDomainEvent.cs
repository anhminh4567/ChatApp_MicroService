using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Shared;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Domain.Messages.DomainEvents
{
	public record MessageBroadcastDomainEvent(Guid groupId, Message sentMessage) : DomainEvent, IMessageRealTimeDomainEvent
	{
		public Guid GroupId { get; init; } = groupId;
		public Message SentMessage { get; init; } = sentMessage;
	}
}
