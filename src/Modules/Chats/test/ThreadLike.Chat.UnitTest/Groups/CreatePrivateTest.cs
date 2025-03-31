using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Application.Abstracts;
using ThreadLike.Chat.Application.Groups.Commands.CreatePrivate;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Chat.Domain.Users.Entities;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.UnitTest.Groups
{
    public class CreatePrivateTest
    {
		private readonly Mock<IUserRepository> _userRepositoryMock;
		private readonly Mock<IGroupRepository> _groupRepositoryMock;

		private readonly Mock<IUnitOfWork> _unitOfWorkMock;
		private readonly CreatePrivateGroupCommandHandler _handler;

		public CreatePrivateTest()
		{
			_userRepositoryMock = new Mock<IUserRepository>();
			_groupRepositoryMock= new Mock<IGroupRepository>();

			_unitOfWorkMock = new Mock<IUnitOfWork>();

			_handler = new CreatePrivateGroupCommandHandler(_userRepositoryMock.Object, _groupRepositoryMock.Object, _unitOfWorkMock.Object);
		}

		[Fact]
		public async Task Handle_ShouldCreatePrivateGroup_WhenUsersExist()
		{
			// Arrange
			User initiator = User.Create("Initiator", "initiator@example.com", "initiatorId");
			User friend = User.Create("Friend", "friend@example.com", "friendId");

			_userRepositoryMock.Setup(repo => repo.GetById("initiatorId")).ReturnsAsync(initiator);
			_userRepositoryMock.Setup(repo => repo.GetById("friendId")).ReturnsAsync(friend);

			var command = new CreatePrivateGroupCommand("initiatorId", "friendId");

			// Act
			Result<Group> result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.NotNull(result.Value);
			Assert.Equal(Group.PRIVATE_GROUP_NAME, result.Value.Name);
			_unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
		{
			// Arrange
			_userRepositoryMock.Setup(repo => repo.GetById("initiatorId")).ReturnsAsync(() => null);
			_userRepositoryMock.Setup(repo => repo.GetById("friendId")).ReturnsAsync(() => null );

			var command = new CreatePrivateGroupCommand("initiatorId", "friendId");

			// Act
			Result<Group> result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.False(result.IsSuccess);
			Assert.Equal(UserErrors.NotFound, result.Error);
			_unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
		}
	}
}
