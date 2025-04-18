using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Chat.Infrastructure.Options
{
	public class AzureBlobStorageOptions
	{
		public const string SectionName = "BlobStorage:Azure";
		[NotNull]
		public string ContainerName { get; set; } 
		public string ConnectionString { get; set; } 
		[NotNull]
		public string Url { get; set; }
		[NotNull]
		public string[] ContainerNames { get; set; } = Array.Empty<string>();
	}
}
