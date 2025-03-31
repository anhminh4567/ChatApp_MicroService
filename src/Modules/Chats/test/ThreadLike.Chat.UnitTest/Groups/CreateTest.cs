using Bogus;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Application.Abstracts;
using ThreadLike.Chat.Application.Groups.Commands.CreateGroup;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Users;

namespace ThreadLike.Chat.UnitTest.Groups
{
	public class CreateTest
	{
		private readonly Mock<IUserRepository> _userRepositoryMock;
		private readonly Mock<IGroupRepository> _groupRepositoryMock;
		private readonly Mock<IUnitOfWork> _unitOfWorkMock;
		private readonly CreateGroupCommandHandler _handler;
		private readonly Faker _faker;

		public CreateTest()
		{
			_userRepositoryMock = new Mock<IUserRepository>();
			_groupRepositoryMock = new Mock<IGroupRepository>();
			_unitOfWorkMock = new Mock<IUnitOfWork>();
			_handler = new CreateGroupCommandHandler(_userRepositoryMock.Object, _groupRepositoryMock.Object, _unitOfWorkMock.Object);
			_faker = new Faker();
		}

		[Fact]
		public async Task Handle_ShouldCreateGroupSuccessfully()
		{
			// Arrange
			var creatorId = _faker.Random.Guid().ToString();
			var participantIds = new List<string> { _faker.Random.Guid().ToString(), _faker.Random.Guid().ToString() };
			var command = new CreateGroupCommand(_faker.Company.CompanyName(), creatorId, participantIds);

			var creator = User.Create(_faker.Person.FullName, _faker.Internet.Email(), creatorId);
			var participants = new List<User>
			{
				User.Create(_faker.Person.FullName, _faker.Internet.Email(), participantIds[0]),
				User.Create(_faker.Person.FullName, _faker.Internet.Email(), participantIds[1])
			};

			_userRepositoryMock.Setup(repo => repo.GetById(creatorId)).ReturnsAsync(creator);
			_userRepositoryMock.Setup(repo => repo.Exist(participantIds, It.IsAny<CancellationToken>())).ReturnsAsync(true);
			_userRepositoryMock.Setup(repo => repo.GetByManyIds(participantIds, It.IsAny<CancellationToken>())).ReturnsAsync(participants);

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.NotNull(result.Value);
			_groupRepositoryMock.Verify(repo => repo.Create(It.IsAny<Group>()), Times.Once);
			_unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenCreatorNotFound()
		{
			// Arrange
			var creatorId = _faker.Random.Guid().ToString();
			var participantIds = new List<string> { _faker.Random.Guid().ToString(), _faker.Random.Guid().ToString() };
			var command = new CreateGroupCommand(_faker.Company.CompanyName(), creatorId, participantIds);

			_userRepositoryMock.Setup(repo => repo.GetById(creatorId)).ReturnsAsync((User)null);

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.False(result.IsSuccess);
			Assert.Equal(UserErrors.NotFound, result.Error);
		}

		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenParticipantsNotFound()
		{
			// Arrange
			var creatorId = _faker.Random.Guid().ToString();
			var participantIds = new List<string> { _faker.Random.Guid().ToString(), _faker.Random.Guid().ToString() };
			var command = new CreateGroupCommand(_faker.Company.CompanyName(), creatorId, participantIds);

			var creator = User.Create(_faker.Person.FullName, _faker.Internet.Email(), creatorId);

			_userRepositoryMock.Setup(repo => repo.GetById(creatorId)).ReturnsAsync(creator);
			_userRepositoryMock.Setup(repo => repo.Exist(participantIds, It.IsAny<CancellationToken>())).ReturnsAsync(false);

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.False(result.IsSuccess);
			Assert.Equal("participants not found", result.Error.Description);
		}
	}
}
