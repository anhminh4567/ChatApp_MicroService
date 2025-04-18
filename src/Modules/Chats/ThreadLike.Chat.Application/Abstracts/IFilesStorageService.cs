using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Chat.Application.Abstracts
{
	public enum BlobDirectoryType
	{
		Public, PaidContent, Private
	}
	public record BlobFileResponseDto(Stream Stream, string ContenType, long SizeByte);
	public record BlobFileDetails(string Name, string ContentType);
	public interface IFilesStorageService
    {
		public const string PUBLIC_CONTAINER = "public";
		public const string PRIVATE_CONTAINER = "private";
		/// <summary>
		/// return relative path if upload succeess
		/// </summary>
		/// <param name="containerName"></param>
		/// <param name="blobName"></param>
		/// <param name="fileStream"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<string> UploadFileAsync(string containerName, string blobName, string contentType, Stream fileStream, CancellationToken cancellationToken = default);
		Task<BlobFileResponseDto> DownloadFileAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
		Task DeleteFileAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
		Task<bool> IsAnyFileExist(string containerName,string folderPath, CancellationToken cancellationToken = default);
		Task<IEnumerable<BlobFileDetails>> GetFolders(string containerName, string folderPath, CancellationToken cancellationToken = default);

		string ToAbsolutePath(string relativePath);
		string ToRelativePath(string absolutePath);
	}
}
