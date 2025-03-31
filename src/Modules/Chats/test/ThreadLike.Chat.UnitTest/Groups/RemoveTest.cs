using Bogus;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Application.Groups.Commands.Remove;
using ThreadLike.Chat.Domain.Groups.Enums;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Common.Domain;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Chat.Application.Abstracts;
using ThreadLike.Chat.Domain.Groups.Entities;
using ThreadLike.Chat.Domain.GroupRoles;

namespace ThreadLike.Chat.UnitTest.Groups
{
	public class RemoveTest
	{
		private readonly Mock<IGroupRepository> _groupRepositoryMock;
		private readonly Mock<IUserRepository> _userRepositoryMock;
		private readonly Mock<IUnitOfWork> _unitOfWorkMock;
		private readonly RemoveGroupCommandHandler _handler;
		private readonly Faker _faker;

		public RemoveTest()
		{
			_groupRepositoryMock = new Mock<IGroupRepository>();
			_userRepositoryMock = new Mock<IUserRepository>();
			_unitOfWorkMock = new Mock<IUnitOfWork>();
			_handler = new RemoveGroupCommandHandler(_groupRepositoryMock.Object, _userRepositoryMock.Object, _unitOfWorkMock.Object);
			_faker = new Faker();
		}

		[Fact]
		public async Task Handle_ShouldRemoveGroupSuccessfully()
		{
			// Arrange
			var groupId = _faker.Random.Guid();
			var creatorId = _faker.Random.Guid().ToString();
			var command = new RemoveGroupCommand(groupId, creatorId);

			var user = User.Create(_faker.Person.FullName, _faker.Internet.Email(), creatorId);

			var participant = Participant.Create(user.Id, groupId, GroupRole.GroupLeaderName);
			var group = Group.Create(_faker.Company.CompanyName(), User.Create(_faker.Person.FullName, _faker.Internet.Email(), creatorId), GroupType.Group, new List<User>() { user });
			
			group.Participants.Add(participant);

			_groupRepositoryMock.Setup(repo => repo.GetById(groupId)).ReturnsAsync(group);
			_userRepositoryMock.Setup(repo => repo.GetById(creatorId)).ReturnsAsync(user);
			_groupRepositoryMock.Setup(repo => repo.Delete(group));
			_unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()));

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			_groupRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Group>()), Times.Once);
			_unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenGroupNotFound()
		{
			// Arrange
			var groupId = _faker.Random.Guid();
			var creatorId = _faker.Random.Guid().ToString();
			var command = new RemoveGroupCommand(groupId, creatorId);

			_groupRepositoryMock.Setup(repo => repo.GetById(groupId)).ReturnsAsync((Group)null);

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.False(result.IsSuccess);
			Assert.Equal(GroupErrors.NotFound, result.Error);
		}

		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenCreatorNotFound()
		{
			// Arrange
			var groupId = _faker.Random.Guid();
			var creatorId = _faker.Random.Guid().ToString();
			var command = new RemoveGroupCommand(groupId, creatorId);

			var group = Group.Create(_faker.Company.CompanyName(), User.Create(_faker.Person.FullName, _faker.Internet.Email(), creatorId), GroupType.Group, new List<User>());

			_groupRepositoryMock.Setup(repo => repo.GetById(groupId)).ReturnsAsync(group);
			_userRepositoryMock.Setup(repo => repo.GetById(creatorId)).ReturnsAsync((User)null);

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.False(result.IsSuccess);
			Assert.Equal(UserErrors.NotFound, result.Error);
		}

		
	}
}
