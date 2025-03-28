using FluentValidation;
namespace ThreadLike.User.Application.Users.Commands.Register
{
	public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
	{
		public RegisterUserCommandValidator()
		{
			RuleFor(x => x.Email).NotEmpty().EmailAddress();
			RuleFor(x => x.Name).NotEmpty();
			RuleFor(x => x.IdentityId).NotEmpty();
			RuleFor(x => x.IsEmailVerified).NotNull();
		}
	}

}
