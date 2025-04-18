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
		IUnitOfWork unitOfWork) : ICommandHandler<AddNewMessagesToGroupCommand,Result<List<Message>>>
	{
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
					List<MessageAttachment> attachments = requestMessage.Attachments
						.Select(x => MessageAttachment.Create(message, new MediaObject(x.FileName,x.ContentType,"not yet added IFileProvider function"))).ToList();
					attachments.ForEach(a => message.SetAttachment(a,false));
				}

				response.Add(message); 

				messageRepository.Create(message);
			}
			await unitOfWork.SaveChangesAsync(cancellationToken);

			return response;
		}
	}
	
}
