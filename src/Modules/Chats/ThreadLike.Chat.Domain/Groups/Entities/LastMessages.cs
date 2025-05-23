﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Groups.Options;
using ThreadLike.Chat.Domain.Users;

namespace ThreadLike.Chat.Domain.Groups.Entities
{
	public class LastMessages
	{
		
		public string? SenderId { get; private set; }
		public bool IsAutoGenerated { get; private set; } 
		public string? Content { get; private set; }
		public string[] ReaderIds { get; private set; } = [];
		protected LastMessages()
		{
			// Required by EF
		}

		public static LastMessages Create(User sender, string content, GroupOptions.LastMessageOptions options)
		{
			string correctContent = content.Length > options.MaxContentLength
				? content.Substring(0, options.MaxContentLength)
				: content;
			return new LastMessages
			{
				SenderId = sender.Id,
				Content = content,
				IsAutoGenerated = false,
				ReaderIds = new[] { sender.Id },
			};
		}
		public static LastMessages CreateNewGroupMessage(string content, GroupOptions.LastMessageOptions options)
		{
			string correctContent = content.Length > options.MaxContentLength
				? content.Substring(0, options.MaxContentLength)
				: content;
			return new LastMessages
			{
				SenderId = null,
				Content = correctContent,
				IsAutoGenerated = true,
				ReaderIds = Array.Empty<string>(),
			};
		}
		public static LastMessages CreateMemberMessage(string content, GroupOptions.LastMessageOptions options)
		{
			string correctContent = content.Length > options.MaxContentLength
				? content.Substring(0, options.MaxContentLength)
				: content;
			return new LastMessages
			{
				SenderId = null,
				Content = correctContent,
				IsAutoGenerated = true,
				ReaderIds = Array.Empty<string>(),
			};
		}
		public void Read(User reader)
		{
			if (!ReaderIds.Contains(reader.Id))
			{
				ReaderIds = ReaderIds.Append(reader.Id).ToArray();
			}
		}
	}
}
