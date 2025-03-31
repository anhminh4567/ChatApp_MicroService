using FluentValidation;

namespace ThreadLike.Chat.Application.Groups.Commands.CreatePrivate
{
	public class CreatePrivateGroupCommandValidator : AbstractValidator<CreatePrivateGroupCommand>
	{
		public CreatePrivateGroupCommandValidator()
		{
			RuleFor(x => x.initiatorId).NotEmpty();
			RuleFor(x => x.friendId).NotEmpty();
		}
	}
}
