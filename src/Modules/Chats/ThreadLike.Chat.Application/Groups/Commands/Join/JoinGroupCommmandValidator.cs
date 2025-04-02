using FluentValidation;
namespace ThreadLike.Chat.Application.Groups.Commands.Join
{
	public class JoinGroupCommmandValidator : AbstractValidator<JoinGroupCommand>
	{
		public JoinGroupCommmandValidator()
		{
			RuleFor(x => x.GroupId).NotEmpty();
			RuleFor(x => x.UserId).NotEmpty();
			RuleFor(x => x.AdderId).NotEmpty();
		}
	}	

}
