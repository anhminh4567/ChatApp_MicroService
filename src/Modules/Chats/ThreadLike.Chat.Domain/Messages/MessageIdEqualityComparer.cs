using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Chat.Domain.Messages
{
    public class MessageIdEqualityComparer : IEqualityComparer<Message>
    {
		public bool Equals(Message? x, Message? y)
		{
			if (x == null && y == null) return true;
			if (x == null || y == null) return false;
			return x.Id == y.Id;
		}

		public int GetHashCode(Message obj)
		{
			if (obj == null) return 0;
			return obj.Id.GetHashCode();
		}
	}

}
