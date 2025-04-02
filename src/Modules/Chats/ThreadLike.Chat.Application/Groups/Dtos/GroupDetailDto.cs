using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Users;

namespace ThreadLike.Chat.Application.Groups.Dtos
{
    public class GroupDetailDto
	{
		public Group Group { get; set; }
		public List<User> ParticipantsDetail { get; set; } = new();

		// should have whos online
		
	}
    
}
