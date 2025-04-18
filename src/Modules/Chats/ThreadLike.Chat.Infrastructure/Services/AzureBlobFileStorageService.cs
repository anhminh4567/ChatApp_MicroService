using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThreadLike.Chat.Application.Abstracts;
using ThreadLike.Chat.Infrastructure.Options;

namespace ThreadLike.Chat.Infrastructure.Services
{
	internal class AzureBlobFileStorageService : IFilesStorageService
	{
		protected readonly BlobServiceClient _blobServiceClient;
		private readonly ILogger<AzureBlobFileStorageService> _logger;
		protected readonly IOptions<AzureBlobStorageOptions> _azureBlobStorageOptions;

		public AzureBlobFileStorageService(BlobServiceClient blobServiceClient, ILogger<AzureBlobFileStorageService> logger, IOptions<AzureBlobStorageOptions> externalUrlsOptions)
		{
			_blobServiceClient = blobServiceClient;
			_logger = logger;
			_azureBlobStorageOptions = externalUrlsOptions;
		}

		public async Task DeleteFileAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
		{
			BlobContainerClient blobContainerClient = GetCorrectBlobClient(containerName);
			BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
			Azure.Response deleteResult = await blobClient.DeleteAsync(cancellationToken: cancellationToken);
			if(deleteResult.IsError)
			{
				_logger.LogError("Error deleting blob {BlobName} from container {ContainerName}", blobName, containerName);
				throw new Exception($"Error deleting blob {blobName} from container {containerName}");
			}
		}

		public async Task<BlobFileResponseDto> DownloadFileAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
		{
			BlobContainerClient blobContainerClient = GetCorrectBlobClient(containerName);
			BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
			Azure.Response<BlobDownloadStreamingResult> response = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);
			return new BlobFileResponseDto(response.Value.Content, response.Value.Details.ContentType, response.Value.Details.ContentLength);
		}

		public async Task<bool> IsAnyFileExist(string containerName,string folderPath, CancellationToken cancellationToken = default)
		{
			BlobContainerClient blobContainerClient = GetCorrectBlobClient(containerName);
			var blobItems = blobContainerClient.GetBlobClient(folderPath);
			if (blobItems.Exists())
				return true;
			else
				return false;
		}
		public async Task<IEnumerable<BlobFileDetails>> GetFolders(string containerName, string folderPath, CancellationToken cancellationToken = default)
		{
			BlobContainerClient blobContainerClient = GetCorrectBlobClient(containerName);
			Azure.AsyncPageable<BlobHierarchyItem> blobItems = blobContainerClient.GetBlobsByHierarchyAsync(prefix: folderPath, cancellationToken: cancellationToken);
			List<BlobFileDetails> result = new List<BlobFileDetails>();

			await foreach (var blobItem in blobItems)
			{
				if (blobItem.IsBlob)
				{
					var media = new BlobFileDetails(blobItem.Blob.Name, blobItem.Blob.Properties.ContentType);
					result.Add(media);
				}
			}

			return result;
		}
		public async Task<string> UploadFileAsync(string containerName, string blobName, string contentType, Stream fileStream, CancellationToken cancellationToken = default)
		{
			BlobContainerClient blobContainerClient = GetCorrectBlobClient(containerName);
			BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
			Azure.Response<BlobContentInfo> uploadResult = await blobClient.UploadAsync(
				fileStream,
				new BlobHttpHeaders { ContentType = contentType, },
				cancellationToken: cancellationToken);
			BlobContentInfo? tryGetBlobResult = uploadResult.Value;
			if (tryGetBlobResult is null)
				throw new NullReferenceException();
			return blobName;
		}

		public string ToAbsolutePath(string relativePath)
		{
			return $"{_azureBlobStorageOptions.Value.Url}/{relativePath}";
		}

		public string ToRelativePath(string absolutePath)
		{
			return absolutePath.Replace(_azureBlobStorageOptions.Value.Url + "/", string.Empty);
		}

		
		private BlobContainerClient GetCorrectBlobClient(string containerName )
		{
			return _blobServiceClient.GetBlobContainerClient(containerName);
		}
	}
}
