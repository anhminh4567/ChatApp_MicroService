using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Shared;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Domain.Reactions
{
	public class Reaction : Entity
	{
		public string Id { get; private set; } = Utils.GetIdGen(10);
		public string ReactionDescription { get; private set; }
		public string MimeType { get; set; }
		public string Value { get; private set; }
		public bool IsSystem { get; private set; } = true;
		protected Reaction(string emojiValue, string description, string mimeType)
		{
			Value = emojiValue;
			ReactionDescription = description;
			MimeType = mimeType;
		}
		public static Reaction Create(string emojiValue, string description,string mimeType)
		{
			return new Reaction(emojiValue, description,mimeType);
		}

		private Reaction()
		{
		}
	}
}
