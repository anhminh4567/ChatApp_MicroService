using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Messages;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Infrastructure.Hubs
{
    public interface IGroupChatClient
    {
		Task Joined(Guid groupId, User joinedUser);
		Task ReceiveGroupMessage(Guid groupId, List<Message> newMessages);
		Task ReceiveIsTyping(Guid groupId, string typerId);
		Task ReceiveStopTyping(Guid groupId, string typerId);
		Task ReceiveError(int code, string errorMessage);
	}
}
