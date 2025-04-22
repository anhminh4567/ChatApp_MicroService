using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Groups.Enums;
using ThreadLike.Chat.Domain.Messages;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Chat.Infrastructure.Database;
using ThreadLike.Chat.Infrastructure.Hubs;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.IntegrationTest.Messages
{
	public class MessageRealTimeDomainEventInterceptorTest
	{
		private readonly Mock<IHubContext<GroupChatHub, IGroupChatClient>> _hubContextMock;
		private readonly Mock<IGroupChatClient> _groupChatClientMock;
		private readonly MessageRealTimeDomainEventInterceptor _interceptor;

		public MessageRealTimeDomainEventInterceptorTest()
		{
			_groupChatClientMock = new Mock<IGroupChatClient>();
			_hubContextMock = new Mock<IHubContext<GroupChatHub, IGroupChatClient>>();
			_hubContextMock
				.Setup(hub => hub.Clients.Group(It.IsAny<string>()))
				.Returns(_groupChatClientMock.Object);

			_interceptor = new MessageRealTimeDomainEventInterceptor(_hubContextMock.Object);
		}

		[Fact]
		public async Task Interceptor_ShouldRemoveDomainEvent_AfterProcessing()
		{
			// Arrange
			var faker = new Bogus.Faker();
			var userCreator = User.Create(
				faker.Name.FullName(),
				faker.Internet.Email(),
				faker.Random.Guid().ToString(),
				faker.Internet.Avatar(),
				faker.Date.Past()
				);

			var group = Group.Create(
				"Test Group",
				userCreator,
				GroupType.Group,
				new List<User> { userCreator }
			);

			var message = Message.Create(
				userCreator.Id,
				group,
				"Test Content"
			);

			var domainEvent = message.DomainEvents.FirstOrDefault();

			Assert.NotNull(domainEvent); // Ensure the domain event exists before processing

			var mockHubContext = new Mock<IHubContext<GroupChatHub, IGroupChatClient>>();
			var mockGroupChatClient = new Mock<IGroupChatClient>();
			mockHubContext
				.Setup(hub => hub.Clients.Group(It.IsAny<string>()))
				.Returns(mockGroupChatClient.Object);

			var options = new DbContextOptionsBuilder<ChatDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.AddInterceptors(new MessageRealTimeDomainEventInterceptor(mockHubContext.Object) )
				.Options;

			await using var dbContext = new ChatDbContext(options);

			dbContext.Add(message);
			await dbContext.SaveChangesAsync();
			//var interceptor = new MessageRealTimeDomainEventInterceptor(mockHubContext.Object);

			// Act
			//await _interceptor.SavingChangesAsync(eventData, default);

			// Assert
			Assert.Empty(message.DomainEvents); // Ensure domain events are removed
			mockGroupChatClient.Verify(client =>
			   client.ReceiveGroupMessage(
				   It.Is<Guid>(id => id == group.Id),
				   It.Is<List<Message>>(messages => messages.Count == 1 && messages.Contains(message))
			   ),
			   Times.Once); // Ensure SignalR hub is called
		}
	}
}
