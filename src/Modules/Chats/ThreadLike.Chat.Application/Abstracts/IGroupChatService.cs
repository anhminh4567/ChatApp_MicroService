using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Messages;
using ThreadLike.Chat.Domain.Users;

namespace ThreadLike.Chat.Application.Abstracts
{
    public interface IGroupChatService
    {
		Task Joined(Guid groupId, User joinedUser);
		Task SendGroupMessage(Guid groupId, List<Message> newMessages);
		Task SendIsTyping(Guid groupId, string typerId);
		Task SendStopTyping(Guid groupId, string typerId);
	}
}
