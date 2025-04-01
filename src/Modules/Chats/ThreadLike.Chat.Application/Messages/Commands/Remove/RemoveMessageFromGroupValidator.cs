using FluentValidation;

namespace ThreadLike.Chat.Application.Messages.Commands.Remove
{
	public class RemoveMessageFromGroupValidator : AbstractValidator<RemoveMessageFromGroupCommand>
	{
		public RemoveMessageFromGroupValidator()
		{
			RuleFor(x => x.MessageId).NotEmpty();
			RuleFor(x => x.GroupId).NotEmpty();
			RuleFor(x => x.SenderId).NotEmpty();
		}
	}
}
