using FluentValidation;
using ThreadLike.Chat.Domain.Reactions;

namespace ThreadLike.Chat.Application.Messages.Dtos
{
	public class MessageAttachmentDtoValidator : AbstractValidator<MessageAttachmentDto>
	{
		public MessageAttachmentDtoValidator()
		{
			RuleFor(x => x.FileName).NotNull().NotEmpty();
			RuleFor(x => x.FileStream).NotNull();
		}
	}
	public record MessageAttachmentDto(string FileName, string ContentType, Stream FileStream);
	public record MessageReactionDto(  string MesssageId , string ReactionId, string ReactorId);

}
