using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Shared;
using ThreadLike.Common.Domain.Ultils;

namespace ThreadLike.Chat.Domain.Messages.Entities
{
	public class MessageAttachment
	{
		public string Id { get; private set; }
		public Guid MessageId { get; private set; }
		public MediaObject AttachmentDetail { get; private set; }
		public MediaObject? ThumbDetail { get; private set; }

		private MessageAttachment(string id, Guid messageId, MediaObject attachmentDetail, MediaObject? thumbDetail)
		{
			Id = id;
			MessageId = messageId;
			AttachmentDetail = attachmentDetail;
			ThumbDetail = thumbDetail;
		}
		public static MessageAttachment Create(Message Message, MediaObject attachmentDetail, MediaObject? thumbDetail = null)
		{
			return new MessageAttachment(IdGenUltils.GetIdGen(9), Message.Id, attachmentDetail, thumbDetail);
		}
		public void ChangethumbDetail(MediaObject? thumbDetail)
		{
			ThumbDetail = thumbDetail;
		}

		private MessageAttachment()
		{
		}
	}
}
