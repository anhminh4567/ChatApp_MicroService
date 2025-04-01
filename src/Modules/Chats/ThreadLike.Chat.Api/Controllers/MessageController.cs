using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using ThreadLike.Chat.Application.Messages.Commands.AddManyToGroup;
using ThreadLike.Chat.Application.Messages.Commands.Remove;
using ThreadLike.Chat.Application.Messages.Dtos;
using ThreadLike.Chat.Application.Messages.Queries;
using ThreadLike.Chat.Domain.Messages;
using ThreadLike.Chat.Domain.Messages.Entities;
using ThreadLike.Common.Api;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Api.Controllers
{
	[Route("api/messages")]
	[ApiController]
	public class MessageController : ControllerBase
	{
		private readonly IMediator _mediator;

		public MessageController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("all")]
		public async Task<IResult> GetAll()
		{
			Result<List<Message>> result = await _mediator.Send(new GetAllMessagesQuery());
			if (result.IsFailure)
			{
				return ApiResult.MatchError(result.Error);
			}
			return Results.Ok(result.Value);
		}

		[HttpGet("paging")]
		public async Task<IResult> GetPaging([FromQuery] GetMessagesPagingQuery query)
		{
			Result<List<Message>> result = await _mediator.Send(query);
			if (result.IsFailure)
			{
				return ApiResult.MatchError(result.Error);
			}
			return Results.Ok(result.Value);
		}
		[HttpPost()]
		[Consumes("multipart/form-data")]
		public async Task<IResult> AddMessage([FromForm] AddMessagesToGroupRequestDto commandDto)
		{
			MessageDto commandMessage = new MessageDto(
				commandDto.message.SenderId,
				commandDto.message.Content,
				commandDto.message.ReferenceMessageId,
				new List<MessageAttachmentDto>());
			if (commandDto.attachments != null)
			{
				foreach (IFormFile attachment in commandDto.attachments)
				{
					using Stream stream = attachment.OpenReadStream();

					commandMessage.Attachments!.Add(new MessageAttachmentDto(attachment.FileName, attachment.ContentType, stream));
				}
			}

			Result<List<Message>> result = await _mediator.Send(new AddNewMessagesToGroupCommand([commandMessage], commandDto.groupId));
			if (result.IsFailure)
			{
				return ApiResult.MatchError(result.Error);
			}
			return Results.Ok(result.Value);
		}
		[HttpDelete()]
		public async Task<IResult> RemoveMessage([FromBody] RemoveMessageFromGroupCommand command)
		{
			Result result = await _mediator.Send(command);
			if (result.IsFailure)
			{
				return ApiResult.MatchError(result.Error);
			}
			return Results.Ok();
		}
		public record AddMessagesToGroupRequestDto(Guid groupId, MessageRequestDto message, List<IFormFile>? attachments = null);
		public record MessageRequestDto(string SenderId, string Content, Guid? ReferenceMessageId);
	}
}
