﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.GroupRoles;
using ThreadLike.Chat.Domain.Groups.Entities;
using ThreadLike.Chat.Domain.Groups.Enums;
using ThreadLike.Chat.Domain.Groups.Options;
using ThreadLike.Chat.Domain.Messages;
using ThreadLike.Chat.Domain.Shared;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Domain.Groups
{
	public class Group : Entity
	{
		public const string PRIVATE_GROUP_NAME = "PrivateV01";
		public Guid Id { get; private set; }
		public string Name { get; private set; }
		public string CreatorId { get; private set; }
		public User? Creator { get; private set; }
		public DateTime CreatedAt { get; private set; }
		public DateTime? DeleteAt { get; private set; }
		public GroupType GroupType { get; private set; }
		public MediaObject? ThumbDetail { get; private set; }
		public LastMessages? LastMessage { get; private set; } 
		public List<Message> Messages { get; private set; } = new();
		public List<Participant> Participants { get; private set; } = new();

		private Group(Guid id, string name, string creatorId, DateTime createdAt, GroupType type, List<Participant> participants)
		{
			Id = id;
			Name = name;
			CreatorId = creatorId;
			CreatedAt = createdAt;
			Participants = participants;
			GroupType = type;
		}
		/// <summary>
		/// Create a group, if create self reference then remember to add 2 createor and userInGroup with type Peer
		/// </summary>
		/// <param name="name"></param>
		/// <param name="creator"></param>
		/// <param name="type"></param>
		/// <param name="userInGroup"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static Group Create(string name, User creator, GroupType type, List<User> userInGroup)
		{
			var group = new Group(Guid.NewGuid(), name, creator.Id, DateTime.UtcNow,type, new List<Participant>());


			if (!userInGroup.Any(x => x.Id == creator.Id))
				userInGroup.Add(creator);

			foreach (User user in userInGroup)
			{
				if (user.Id == creator.Id)
					group.Participants.Add(Entities.Participant.Create(user.Id, group.Id, GroupRole.GroupLeaderName));

				else
					group.Participants.Add(Entities.Participant.Create(user.Id, group.Id, GroupRole.MemberName));
			}

			switch (type)
			{
				case GroupType.Peer:
					if(group.Participants.Count != 2)
						throw new Exception("peer group must have at 2 users");
					break;
				case GroupType.Group:
					if (group.Participants.Count < 1)
						throw new Exception("group must have atleast 1, the leader users");
					break;
				default:
					throw new Exception("group typ unfit");
			}
			GroupOptions groupOptions = new GroupOptions();
			group.LastMessage = LastMessages.CreateNewGroupMessage(groupOptions.DefaultNewGroupMessageContent,groupOptions.LastMessageOption);
			return group;
		}
		public Result AddToGroup(User user, GroupRole roleType)
		{
			if (Participants.Any(x => x.UserId == user.Id))
				return Result.Failure("user already in group");

			Participants.Add(Entities.Participant.Create(user.Id, Id, roleType.Role));
			GroupOptions groupOptions = new GroupOptions();

			this.LastMessage = LastMessages.CreateMemberMessage(groupOptions.DefaultNewMemberMessageContent, groupOptions.LastMessageOption);

			return Result.Ok();
		}
		public Result RemoveFromGroup(User user)
		{
			var userGroupRole = Participants.FirstOrDefault(x => x.UserId == user.Id);
			if (userGroupRole is null)
				return Result.Failure("user is not in group");

			Participants.Remove(userGroupRole);
			return Result.Ok();
		}
		public Result Delete(User deleter)
		{
			if( ! Participants.Any(x => x.UserId == deleter.Id && x.RoleName == GroupRole.GroupLeaderName))
				return Result.Failure(Name + " group can only be deleted by group leader");
			DeleteAt = DateTime.UtcNow;
			return Result.Ok();
		}

		public void SetGroupThumb(MediaObject thumb)
		{
			ThumbDetail = thumb;
		}
		private Group()
		{
		}
	}
}
