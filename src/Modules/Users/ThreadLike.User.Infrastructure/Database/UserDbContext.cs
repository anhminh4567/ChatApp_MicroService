using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using ThreadLike.Common.Infrastructure.Inbox;
using ThreadLike.Common.Infrastructure.Outbox;
using ThreadLike.User.Application.Abstractions;
using ThreadLike.User.Domain.Roles;
using ThreadLike.User.Domain.Users.Entities;
using ThreadLike.User.Infrastructure.Roles;
using ThreadLike.User.Infrastructure.Users;

namespace ThreadLike.User.Infrastructure.Database
{
	public class UserDbContext : DbContext , IUnitOfWork
	{
		internal const string Schema = "users";
		public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
		{
		}
		public DbSet<Domain.Users.User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<UserRoles> UserRoles { get; set; }
		internal DbSet<OutboxMessage> OutboxMessages { get; set; }
		internal DbSet<OutboxMessageConsumer> OutboxMessageStates { get; set; }
		internal DbSet<InboxMessage> InboxMessages { get; set; }
		internal DbSet<InboxMessageConsumer> InboxMessageConsumers { get; set; }

		public async Task<DbTransaction> BeginTransactionAsync(CancellationToken tokeen = default)
		{
			IDbContextTransaction transaction = await Database.BeginTransactionAsync(tokeen);
			return transaction.GetDbTransaction();
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return base.SaveChangesAsync(cancellationToken);
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
			base.OnModelCreating(modelBuilder);
			modelBuilder.HasDefaultSchema(Schema);
			modelBuilder.ApplyConfiguration(new RoleConfiguration());
			modelBuilder.ApplyConfiguration(new UsersConfiguration());
			modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
			modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
			modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());
			modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
			modelBuilder.ApplyConfiguration(new InboxMessageConsumerConfiguration());
		}
	}
}
