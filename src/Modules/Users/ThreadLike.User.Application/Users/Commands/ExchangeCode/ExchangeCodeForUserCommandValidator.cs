using FluentValidation;

namespace ThreadLike.User.Application.Users.Commands.ExchangeCode
{
	public class ExchangeCodeForUserCommandValidator : AbstractValidator<ExchangeCodeForUserCommand>
	{
		public ExchangeCodeForUserCommandValidator()
		{
			RuleFor(x => x.code).NotEmpty();
		}
	}
}
