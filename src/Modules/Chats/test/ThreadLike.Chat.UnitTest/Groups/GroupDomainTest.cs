using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.GroupRoles;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Groups.Entities;
using ThreadLike.Chat.Domain.Groups.Enums;
using ThreadLike.Chat.Domain.Shared;
using ThreadLike.Chat.Domain.Users;

namespace ThreadLike.Chat.UnitTest.Groups
{
    public class GroupDomainTest
    {
		private readonly Faker _faker = new();
		[Fact]
		public void Create_ShouldCreatePeerGroup_WhenValidUsersProvided()
		{
			// Arrange
			var creator = User.Create("Creator", "creator@example.com", "creatorId");
			var friend = User.Create("Friend", "friend@example.com", "friendId");
			var users = new List<User> { creator, friend };

			// Act
			var group = Group.Create(Group.PRIVATE_GROUP_NAME, creator, GroupType.Peer, users);

			// Assert
			Assert.NotNull(group);
			Assert.Equal(Group.PRIVATE_GROUP_NAME, group.Name);
			Assert.Equal(2, group.Participants.Count);
		}

		[Fact]
		public void Create_ShouldThrowException_WhenInvalidPeerGroup()
		{
			// Arrange
			var creator = User.Create("Creator", "creator@example.com", "creatorId");
			var users = new List<User> { creator };

			// Act & Assert
			Assert.Throws<Exception>(() => Group.Create(Group.PRIVATE_GROUP_NAME, creator, GroupType.Peer, users));
		}

		[Fact]
		public void AddToGroup_ShouldAddUser_WhenUserNotInGroup()
		{
			// Arrange
			var creator = User.Create("Creator", "creator@example.com", "creatorId");
			var newUser = User.Create("NewUser", "newuser@example.com", "newUserId");
			var newUser2 = User.Create("NewUser2", "newuser2@example.com", "newUserId2");

			var group = Group.Create(Group.PRIVATE_GROUP_NAME, creator, GroupType.Group, new List<User> { newUser });

			// Act
			var result = group.AddToGroup(newUser2, GroupRole.Member);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.Contains(group.Participants, p => p.UserId == newUser.Id);
		}

		[Fact]
		public void AddToGroup_ShouldReturnFailure_WhenUserAlreadyInGroup()
		{
			// Arrange
			var creator = User.Create("Creator", "creator@example.com", "creatorId");
			var group = Group.Create(Group.PRIVATE_GROUP_NAME, creator, GroupType.Peer, new List<User> { creator,creator });

			// Act
			var result = group.AddToGroup(creator, GroupRole.Member);

			// Assert
			Assert.False(result.IsSuccess);
			//Assert.Equal("user already in group", result.Error.Description);
		}

		[Fact]
		public void AddToGroup_ShouldContainCreator_WhenCreatorNotInList()
		{
			// Arrange
			var creator = User.Create("Creator", "creator@example.com", "creatorId");
			var participant1 = User.Create(_faker.Name.FullName(),_faker.Internet.Email(),_faker.Random.Guid().ToString());
			var participant2 = User.Create(_faker.Name.FullName(), _faker.Internet.Email(), _faker.Random.Guid().ToString());


			// Act
			var group = Group.Create(Group.PRIVATE_GROUP_NAME, creator, GroupType.Group, new List<User> { participant1, participant2 });

			// Assert
			Assert.True(group.Participants.Any(x => x.UserId == creator.Id));
		}

		[Fact]
		public void RemoveFromGroup_ShouldRemoveUser_WhenUserInGroup()
		{
			// Arrange
			var creator = User.Create("Creator", "creator@example.com", "creatorId");
			var group = Group.Create(Group.PRIVATE_GROUP_NAME, creator, GroupType.Group, new List<User> { creator });
			var newUser = User.Create("NewUser", "newuser@example.com", "newUserId");
			group.AddToGroup(newUser, GroupRole.Member);

			// Act
			var result = group.RemoveFromGroup(newUser);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.DoesNotContain(group.Participants, p => p.UserId == newUser.Id);
		}

		[Fact]
		public void RemoveFromGroup_ShouldReturnFailure_WhenUserNotInGroup()
		{
			// Arrange
			var creator = User.Create("Creator", "creator@example.com", "creatorId");
			var group = Group.Create(Group.PRIVATE_GROUP_NAME, creator, GroupType.Group, new List<User> { creator });
			var newUser = User.Create("NewUser", "newuser@example.com", "newUserId");

			// Act
			var result = group.RemoveFromGroup(newUser);

			// Assert
			Assert.False(result.IsSuccess);
			//Assert.Equal("user is not in group", result.Error.Description);
		}

		[Fact]
		public void Delete_ShouldDeleteGroup_WhenGroupLeaderDeletes()
		{
			// Arrange
			var creator = User.Create("Creator", "creator@example.com", "creatorId");
			var group = Group.Create(Group.PRIVATE_GROUP_NAME, creator, GroupType.Group, new List<User> { creator });

			// Act
			var result = group.Delete(creator);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.NotNull(group.DeleteAt);
		}

		[Fact]
		public void Delete_ShouldReturnFailure_WhenNonGroupLeaderDeletes()
		{
			// Arrange
			var creator = User.Create("Creator", "creator@example.com", "creatorId");
			var group = Group.Create(Group.PRIVATE_GROUP_NAME, creator, GroupType.Group, new List<User> { creator });
			var newUser = User.Create("NewUser", "newuser@example.com", "newUserId");
			group.AddToGroup(newUser, GroupRole.Member);

			// Act
			var result = group.Delete(newUser);

			// Assert
			Assert.False(result.IsSuccess);
			//Assert.Equal(Group.PRIVATE_GROUP_NAME + " group can only be deleted by group leader", result.Error.Message);
		}

		[Fact]
		public void SetGroupThumb_ShouldSetThumbnail()
		{
			// Arrange
			var creator = User.Create("Creator", "creator@example.com", "creatorId");
			var group = Group.Create(Group.PRIVATE_GROUP_NAME, creator, GroupType.Group, new List<User> { creator });
			var thumb = new MediaObject("thumb.jpg", "image/jpeg", "http://example.com/thumb.jpg");

			// Act
			group.SetGroupThumb(thumb);

			// Assert
			Assert.NotNull(group.ThumbDetail);
			Assert.Equal(thumb, group.ThumbDetail);
		}
	}
}
