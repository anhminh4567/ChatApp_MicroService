using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Application.Abstracts;
using ThreadLike.Chat.Application.Messages.Dtos;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Groups.Entities;
using ThreadLike.Chat.Domain.Messages;
using ThreadLike.Chat.Domain.Messages.Entities;
using ThreadLike.Chat.Domain.Shared;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;
using ThreadLike.Common.Domain.Ultils;

namespace ThreadLike.Chat.Application.Messages.Commands.AddManyToGroup
{
	public record AddNewMessagesToGroupCommand( List<MessageDto> NewMessages, Guid GroupId  ) : ICommand<Result<List<Message>>>;
	internal class AddNewMessagesToGroupCommandHandler(
		IGroupRepository groupRepository,
		IUserRepository userRepository,
		IFilesStorageService filesStorageService,
		IMessageRepository messageRepository,
		IGroupChatService groupChatService,
		IUnitOfWork unitOfWork) : ICommandHandler<AddNewMessagesToGroupCommand,Result<List<Message>>>
	{
		public static string GetMessageAttachmentBlobPath(Guid groupId, Guid messageId,string extension)
		{
			if(extension.StartsWith("."))
			{
				extension = extension.Substring(1);
			}
			string fileName = $"{messageId.ToString().ToLower()}_{DateTime.UtcNow.Ticks}.{extension}";
			return $"{groupId}/{messageId}/attachments/{fileName}";
		}
		public async Task<Result<List<Message>>> Handle(AddNewMessagesToGroupCommand request, CancellationToken cancellationToken)
		{
			// should orderBy() beefore process
			Group group = await groupRepository.GetById(request.GroupId);
			if (group == null)
				return Result.Failure(GroupErrors.NotFound);

			List<Participant> participants = await groupRepository.GetParticipantsForGroup(group);
			List<Message> response = new();
			foreach (MessageDto requestMessage in request.NewMessages)
			{
				if( ! participants.Any(x => x.UserId == requestMessage.SenderId))
				{
					continue;
				}
				Message? referenceMessage = null;
				if (requestMessage.ReferenceMessageId != null)
				{
					referenceMessage =  messageRepository.GetById(requestMessage.ReferenceMessageId).Result;
				}

				Message message =  Message.Create(requestMessage.SenderId, group,requestMessage.Content, referenceMessage);

				if (requestMessage.Attachments != null && requestMessage.Attachments.Any())
				{
					foreach (var attachment in requestMessage.Attachments)
					{
						attachment.FileStream.Seek(0, SeekOrigin.Begin);
						string relativeFilePath = await filesStorageService.UploadFileAsync(
							IFilesStorageService.PUBLIC_CONTAINER,
							GetMessageAttachmentBlobPath(message.GroupId, message.Id, Path.GetExtension(attachment.FileName)),
							attachment.ContentType,
							attachment.FileStream,
							cancellationToken);
						
						var newAttachment = MessageAttachment.Create(message, new MediaObject(
							attachment.FileName,
							attachment.ContentType,
							relativeFilePath));

						message.SetAttachment(newAttachment);
					}
				}
					
				response.Add(message);
				

				messageRepository.Create(message);
			}
			await unitOfWork.SaveChangesAsync(cancellationToken);
			await groupChatService.SendGroupMessage(group.Id, response);
			return response;
		}
	}
	
}
