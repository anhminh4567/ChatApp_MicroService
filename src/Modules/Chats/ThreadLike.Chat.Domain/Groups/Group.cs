using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Group.Entities;
using ThreadLike.Chat.Domain.GroupMessages;
using ThreadLike.Chat.Domain.GroupRoles;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Domain.Groups
{
	public class Group : Entity
	{
		public Guid Id { get; private set; }
		public string Name { get; private set; }
		public string CreatorId { get; private set; }
		public User? Creator { get; private set; }
		public DateTime CreatedAt { get; private set; }
		public List<GroupMessage> Messages { get; private set; } = new();
		public List<UserGroupRole> UserGroupRoles { get; private set; } = new();

		private Group(Guid id, string name, string creatorId, DateTime createdAt, List<UserGroupRole> userGroupRoles)
		{
			Id = id;
			Name = name;
			CreatorId = creatorId;
			CreatedAt = createdAt;
			UserGroupRoles = userGroupRoles;
		}
		public static Group Create(string name, User creator, List<User> userInGroup)
		{
			var group = new Group(Guid.NewGuid(), name, creator.Id, DateTime.UtcNow, new List<UserGroupRole>());


			if (!userInGroup.Any(x => x.Id == creator.Id))
				userInGroup.Add(creator);

			foreach (User user in userInGroup)
			{
				if (user.Id == creator.Id)
					group.UserGroupRoles.Add(UserGroupRole.Create(user.Id, group.Id, GroupRole.GroupLeaderName));

				else
					group.UserGroupRoles.Add(UserGroupRole.Create(user.Id, group.Id, GroupRole.MemberName));
			}
			return group;
		}
		public Result AddToGroup(User user, GroupRole roleType)
		{
			if (UserGroupRoles.Any(x => x.UserId == user.Id))
				return Result.Failure("user already in group");

			UserGroupRoles.Add(UserGroupRole.Create(user.Id, Id, roleType.Role));
			return Result.Ok();
		}
		public Result RemoveFromGroup(User user)
		{
			var userGroupRole = UserGroupRoles.FirstOrDefault(x => x.UserId == user.Id);
			if (userGroupRole is null)
				return Result.Failure("user is not in group");

			UserGroupRoles.Remove(userGroupRole);
			return Result.Ok();
		}

		private Group()
		{
		}
	}
}
