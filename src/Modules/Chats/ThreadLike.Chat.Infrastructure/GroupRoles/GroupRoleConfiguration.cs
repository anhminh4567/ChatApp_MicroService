using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.GroupRoles;

namespace ThreadLike.Chat.Infrastructure.GroupRoles
{
	internal class GroupRoleConfiguration : IEntityTypeConfiguration<GroupRole>
	{
		public void Configure(EntityTypeBuilder<GroupRole> builder)
		{
			builder.HasKey(x => x.Role);

			builder.HasData(GroupRole.Guest, GroupRole.GroupLeader, GroupRole.Member);

		}
	}
}
