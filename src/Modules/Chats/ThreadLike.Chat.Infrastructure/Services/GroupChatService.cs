using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Application.Abstracts;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Messages;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Chat.Infrastructure.Hubs;

namespace ThreadLike.Chat.Infrastructure.Services
{
	internal class GroupChatService(
		IHubContext<GroupChatHub,IGroupChatClient> hubContext) : IGroupChatService
	{
		public Task Joined(Guid groupId, User joinedUser)
		{
			return hubContext.Clients.Group(groupId.ToString()).Joined(groupId, joinedUser);
		}

		public Task SendGroupMessage(Guid groupId, List<Message> newMessages)
		{
			return hubContext.Clients.Group(groupId.ToString()).ReceiveGroupMessage(groupId, newMessages);

		}

		public Task SendIsTyping(Guid groupId, string typerId)
		{
			return hubContext.Clients.Group(groupId.ToString()).ReceiveIsTyping(groupId, typerId);
		}

		public Task SendNewGroupMessage(Guid groupId, Message newMessage)
		{
			return hubContext.Clients.Group(groupId.ToString()).ReceiveNewGroupMessage(groupId, newMessage);
		}

		public Task SendStopTyping(Guid groupId, string typerId)
		{
			return hubContext.Clients.Group(groupId.ToString()).ReceiveStopTyping(groupId, typerId);
		}
	}
}
