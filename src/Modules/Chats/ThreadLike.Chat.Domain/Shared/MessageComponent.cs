using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Chat.Domain.Shared
{
	public class MessageComponent 
	{
		public string Id { get; private set; } = Utils.GetIdGen(5);
		public string? ParentId { get; private set; }
		public string Label { get; private set; }
		public int? SelectCount { get; private set; } = null;
		public List<MessageComponent> MessageComponents { get; private set; }
		public DateTime CreatedAt { get; private set; }

		
	}

}
