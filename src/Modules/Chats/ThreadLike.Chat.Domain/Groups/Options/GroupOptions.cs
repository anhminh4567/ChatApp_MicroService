using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Chat.Domain.Groups.Options
{
	public class GroupOptions
	{
		public string DefaultNewGroupMessageContent { get; set; } = "Say hello to everyone";
		public string DefaultNewMemberMessageContent { get; set; } = "Welcome to the group";
		public LastMessageOptions LastMessageOption { get; set; } = new();
		public class LastMessageOptions 
		{ 
			public int MaxContentLength { get; set; } = 100;
		}

	}
}
