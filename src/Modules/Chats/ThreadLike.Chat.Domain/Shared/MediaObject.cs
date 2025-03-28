using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Chat.Domain.Shared
{
	public class MediaObject
	{
		public string? Name { get; init; }
		public string? MimeType { get; init; }
		public string FileUrl { get; init; }

		public MediaObject(string? name, string? mimeType, string fileUrl)
		{
			Name = name;
			MimeType = mimeType;
			FileUrl = fileUrl;
		}
	}
}
