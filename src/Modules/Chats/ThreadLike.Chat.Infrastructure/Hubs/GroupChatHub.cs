using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Application.Authentication;
using ThreadLike.Common.Application.Exceptions;
using ThreadLike.Common.Infrastructure.Authorization.RolePolicy;

namespace ThreadLike.Chat.Infrastructure.Hubs
{
	[Authorize]
	public class GroupChatHub(
		IUserRepository userRepository,
		IGroupRepository groupRepository,
		ILogger<GroupChatHub> logger) : Hub<IGroupChatClient>
	{
		public override async Task OnConnectedAsync()
		{
			string? userIdentityId = Context.User?.GetIdentityId();
			if (userIdentityId == null)
			{
				await Clients.Caller.ReceiveError(404, "User not found in context");
				Context.Abort();
				return;
			}
			User? user = await userRepository.GetByIdentityIdAsync(userIdentityId);
			if (user is null)
			{
				await Clients.Caller.ReceiveError(404, "User not found in context");
				Context.Abort();
				return;
			}

			logger.LogInformation("User with connect {connectionid} connected", Context.ConnectionId);
		}
		public override Task OnDisconnectedAsync(Exception? exception)
		{
			logger.LogInformation("User with connect {connectionid} disconnected", Context.ConnectionId);
			if(exception != null)
			{
				logger.LogError(exception, "Error on disconnect");
			}
			return Task.CompletedTask;
		}
		public async Task JoinGroup(string userId, Guid groupId)
		{
			string? userIdentityId = Context.User?.GetIdentityId();
			if (userIdentityId == null)
			{
				await Clients.Caller.ReceiveError(404, "User not found in context");
				return;
			}
			User? user = await userRepository.GetByIdentityIdAsync(userIdentityId);
			if (user is null)
			{
				await Clients.Caller.ReceiveError(404, "User not found in context");
				return;
			}
			
			Group? group = (await groupRepository.GetGroupsForUser(user)).FirstOrDefault(g => g.Id == groupId);
			
			if (group is null)
			{
				await Clients.Caller.ReceiveError(404, "Group not found");
				return;
			}

			await Groups.AddToGroupAsync(Context.ConnectionId, groupId.ToString());
			await Clients.Group(groupId.ToString()).Joined(groupId, user);
		}
		public async Task<bool> SendIsTypeing(string typerId, Guid groupId)
		{
			string? userId = Context.User?.GetUserId();
			if (userId == null)
			{
				await Clients.Caller.ReceiveError(404, "user not found in context");
				return false;
			}
			await Clients.Group(groupId.ToString()).ReceiveIsTyping(groupId, userId);
			return true;
		}
		public async Task<bool> SendStopTyping(string typerId, Guid groupId)
		{
			string? userId = Context.User?.GetUserId();
			if (userId == null)
			{
				await Clients.Caller.ReceiveError(404, "user not found in context");
				return false;
			}
			await Clients.Group(groupId.ToString()).ReceiveStopTyping(groupId, userId);
			return true;
		}
	}
}

