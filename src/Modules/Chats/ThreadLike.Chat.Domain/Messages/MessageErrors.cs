using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Chat.Domain.Messages
{
    public static class MessageErrors
    {
		public static class ReactionErrors
		{
			public static string NotFound => "Reaction not found";
		}
		public static string NotFound => "Message not found";
		public static string NotBelongToGroup => "Message not belong to group";
		public static string NotBelongToUser => "Message not belong to user";
		public static string NotBelongToSender => "Message not belong to sender";
		public static string NotBelongToGroupOrSender => "Message not belong to group or sender";
	}
}
