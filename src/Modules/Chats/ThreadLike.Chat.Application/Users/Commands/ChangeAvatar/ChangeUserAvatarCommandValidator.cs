using FluentValidation;

namespace ThreadLike.Chat.Application.Users.Commands.ChangeAvatar
{
	public class ChangeUserAvatarCommandValidator : AbstractValidator<ChangeUserAvatarCommand>
	{
		public ChangeUserAvatarCommandValidator()
		{
			RuleFor(x => x.IdentityId)
				.NotEmpty()
				.WithMessage("IdentityId is required");
			RuleFor(x => x.ImageStream)
				.NotNull()
				.WithMessage("ImageStream is required");
		}
	}

}
