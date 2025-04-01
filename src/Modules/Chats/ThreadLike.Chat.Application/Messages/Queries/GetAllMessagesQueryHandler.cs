using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Messages;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Application.Messages.Queries
{
	public record GetAllMessagesQuery : IQuery<List<Message>>;
	internal class GetAllMessagesQueryHandler(
		IMessageRepository message) : IQueryHandler<GetAllMessagesQuery, List<Message>>
	{
		public Task<List<Message>> Handle(GetAllMessagesQuery request, CancellationToken cancellationToken)
		{ 
			return message.GetAll();
		}
    }
}
