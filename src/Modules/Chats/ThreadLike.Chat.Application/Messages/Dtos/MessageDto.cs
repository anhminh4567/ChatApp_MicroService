using FluentValidation;

namespace ThreadLike.Chat.Application.Messages.Dtos
{
	public class MessageDtoValidator : AbstractValidator<MessageDto>
	{
		public MessageDtoValidator()
		{
			var attachemntValidator = new MessageAttachmentDtoValidator();
			RuleFor(x => x.SenderId).NotNull().NotEmpty();
			RuleFor(x => x.Content).NotNull().NotEmpty();
			RuleForEach(x => x.Attachments).SetValidator(attachemntValidator)
				.When(x => x.Attachments != null && (x.Attachments.Count > 0) );
		}

	}
	public record MessageDto(string SenderId, string Content, Guid? ReferenceMessageId, List<MessageAttachmentDto>? Attachments = null );
	
}
