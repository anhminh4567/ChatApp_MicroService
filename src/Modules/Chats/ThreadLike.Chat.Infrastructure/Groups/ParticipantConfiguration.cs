using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.GroupRoles;
using ThreadLike.Chat.Domain.Groups.Entities;
using ThreadLike.Chat.Domain.Users;

namespace ThreadLike.Chat.Infrastructure.Groups
{
	internal class ParticipantConfiguration : IEntityTypeConfiguration<Participants>
	{
		public void Configure(EntityTypeBuilder<Participants> builder)
		{
			builder.HasKey(x => new { x.UserId, x.GroupId, x.RoleName });

			builder.HasOne<User>()
				.WithMany()
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		
			builder.HasOne<GroupRole>()
				.WithMany()
				.HasForeignKey(x => x.RoleName)
				.OnDelete(DeleteBehavior.SetNull);

		}
	}
}
