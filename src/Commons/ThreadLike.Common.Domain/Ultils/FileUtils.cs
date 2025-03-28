using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Common.Domain.Ultils
{
	public static class FileUtils
	{

		public static readonly Dictionary<string, string> MimeTypes = new()
		{
			{ ".txt", "text/plain" },
			{ ".pdf", "application/pdf" },
			{ ".doc", "application/msword" },
			{ ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
			{ ".xls", "application/vnd.ms-excel" },
			{ ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
			{ ".png", "image/png" },
			{ ".jpg", "image/jpeg" },
			{ ".jpeg", "image/jpeg" },
			{ ".gif", "image/gif" },
			{ ".csv", "text/csv" },
			{ ".svg", "image/svg+xml" },
            // Add more MIME types as needed
        };


		public static string? GetMimeTypeFromExtension(string extension)
		{
			if (string.IsNullOrEmpty(extension))
				throw new ArgumentException("Extension cannot be null or empty.", nameof(extension));

			if (!extension.StartsWith("."))
				extension = "." + extension;

			return MimeTypes.TryGetValue(extension, out string? mimeType) ? mimeType : null;
		}
	}
}



