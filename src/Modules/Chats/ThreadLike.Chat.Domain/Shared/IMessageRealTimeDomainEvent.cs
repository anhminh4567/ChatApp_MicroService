using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Messages;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Domain.Shared
{
	public interface IMessageRealTimeDomainEvent : IFastDomainEvent
	{
		Guid GroupId { get; }
		Message SentMessage { get; }
	}
}
