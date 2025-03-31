using FluentValidation;

namespace ThreadLike.Chat.Application.Groups.Commands.CreateGroup
{
	public class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
	{
		public CreateGroupCommandValidator()
		{
			RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
			RuleFor(x => x.CreatorId).NotEmpty();
			RuleFor(x => x.Participants).NotEmpty();
		}
	}
}
