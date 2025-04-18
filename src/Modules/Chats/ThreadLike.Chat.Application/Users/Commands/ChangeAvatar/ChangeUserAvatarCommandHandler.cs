using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Application.Abstracts;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Application.Users.Commands.ChangeAvatar
{
	public record ChangeUserAvatarCommand(string IdentityId, Stream ImageStream, string ContentType) : ICommand<Result<string>>;

	internal class ChangeUserAvatarCommandHandler(
		IFilesStorageService filesStorageService,
		IUnitOfWork unitOfWork,
		IUserRepository userRepository) : ICommandHandler<ChangeUserAvatarCommand, Result<string>>
	{
		private const int IMG_WIDTH = 512;
		private const int IMG_HEIGHT = 512;
		private const string AVATAR_CONTENT_TYPE = "image/jpeg";
		private const string AVATAR_FOLDER_NAME = "avatars";
		private const string AVATAR_EXTENSION = "jpg";
		public async Task<Result<string>> Handle(ChangeUserAvatarCommand request, CancellationToken cancellationToken)
		{
			if (request.ContentType.StartsWith("image/") == false)
				return Result.Failure("Invalid image type");

			User? user = await userRepository.GetByIdentityIdAsync(request.IdentityId, cancellationToken);

			if (user == null)
			{
				return Result.Failure(UserErrors.NotFound);
			}

			// init the value if the file is success resize
			string blobName = $"{AVATAR_FOLDER_NAME}/{request.IdentityId}.{AVATAR_EXTENSION}";

			using Image image = await Image.LoadAsync(request.ImageStream, cancellationToken);
			// Resize the image to a maximum of 512x512 and maintain ratio
			image.Mutate(x => x.Resize(new ResizeOptions
			{
				Mode = ResizeMode.Max,
				Size = new Size(IMG_WIDTH, IMG_HEIGHT)
			}));
			string getImageType = request.ContentType.Split("/")[1];
			using var outputStream = new MemoryStream();

			var jpegEncoder = new JpegEncoder
			{
				Quality = 75 // default is still 75 
			};
			await image.SaveAsync(outputStream, jpegEncoder, cancellationToken);

			outputStream.Seek(0, SeekOrigin.Begin);

			// Upload the resized JPEG image to the file storage service
			string avatarUrl = await filesStorageService.UploadFileAsync(
				IFilesStorageService.PUBLIC_CONTAINER,
				blobName,
				AVATAR_CONTENT_TYPE,
				outputStream,
				cancellationToken
			);

			user.ChangeAvatarUri(avatarUrl);

			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Ok(avatarUrl);
		}

	}

}
