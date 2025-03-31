using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Application.Abstracts;

using ThreadLike.Chat.Domain.GroupRoles;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Groups.Entities;
using ThreadLike.Chat.Domain.Messages;
using ThreadLike.Chat.Domain.Messages.Entities;
using ThreadLike.Chat.Domain.Reactions;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Chat.Domain.Users.Entities;
using ThreadLike.Chat.Infrastructure.GroupRoles;
using ThreadLike.Chat.Infrastructure.Groups;
using ThreadLike.Chat.Infrastructure.Messages;
using ThreadLike.Chat.Infrastructure.Reactions;
using ThreadLike.Chat.Infrastructure.Users;
using ThreadLike.Common.Infrastructure.Inbox;
using ThreadLike.Common.Infrastructure.Outbox;

namespace ThreadLike.Chat.Infrastructure.Database
{
	public class ChatDbContext : DbContext, IUnitOfWork
	{
		public const string Schema = "chat";
		public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<UserLetter> UserLetters { get; set; }
		public DbSet<UserFriend> UserFriends { get; set; }

		public DbSet<GroupRole> GroupRoles { get; set; }

		public DbSet<Group> Groups { get; set; }
		public DbSet<Participant> Participants { get; set; }

		public DbSet<Reaction> Reactions { get; set; }
		
		public DbSet<Message> Messages { get; set; }
		public DbSet<MessageAttachment> MessagesAttachments { get; set; }
		public DbSet<MessageReaction> MessageReactions { get; set; }



		internal DbSet<InboxMessageConsumer> InboxMessageConsumers { get; set; }
		internal DbSet<InboxMessage> InboxMessages { get; set; }
		internal DbSet<OutboxMessageConsumer> OutboxMessageConsumers { get; set; }
		internal DbSet<OutboxMessage> OutboxMessages { get; set; }

		public async Task<DbTransaction> BeginTransactionAsync(CancellationToken tokeen = default)
		{
			IDbContextTransaction dbtransaction = await Database.BeginTransactionAsync(tokeen);
			return dbtransaction.GetDbTransaction();
		}

		public Task CommitAsync(CancellationToken token = default)
		{
			return Database.CommitTransactionAsync(token);
		}

		public Task RollbackAsync(CancellationToken token = default)
		{
			return Database.RollbackTransactionAsync(token);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasDefaultSchema(Schema);

			modelBuilder.ApplyConfiguration(new UserConfiguration());
			modelBuilder.ApplyConfiguration(new UserLetterConfiguration());
			modelBuilder.ApplyConfiguration(new UserFriendConfiguration());

			modelBuilder.ApplyConfiguration(new GroupRoleConfiguration());

			modelBuilder.ApplyConfiguration(new GroupConfiguration());
			modelBuilder.ApplyConfiguration(new ParticipantConfiguration());

			modelBuilder.ApplyConfiguration(new ReactionConfiguration());

			modelBuilder.ApplyConfiguration(new MessageConfiguration());
			modelBuilder.ApplyConfiguration(new MessageAttachmentConfiguration());
			modelBuilder.ApplyConfiguration(new MessageReactionConfiguration());




			modelBuilder.ApplyConfiguration(new InboxMessageConsumerConfiguration());
			modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
			modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());
			modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
		}
	}
}
