using FluentValidation;

namespace ThreadLike.Chat.Application.Users.Commands.Create
{
	public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
	{
		public CreateUserCommandValidator()
		{
			RuleFor(x => x.Name).NotEmpty();
			RuleFor(x => x.Email).NotEmpty().EmailAddress();
			RuleFor(x => x.IdentityId).NotEmpty();
		}
	}

}
