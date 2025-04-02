using FluentValidation;

namespace ThreadLike.Chat.Application.Messages.Commands.React
{
	public class ReactToMessageCommandValidator : AbstractValidator<ReactToMessageCommand>
	{
		public ReactToMessageCommandValidator()
		{
			RuleFor(x => x.MessageId).NotEmpty();
			RuleFor(x => x.ReactionId).NotEmpty();
			RuleFor(x => x.ReactorId).NotEmpty();
			RuleFor(x => x.GroupId).NotEmpty();
		}
	}
}
