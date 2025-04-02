using FluentValidation;

namespace ThreadLike.Chat.Application.Groups.Commands.Leave
{
	public class LeaveGroupCommandValidator : AbstractValidator<LeaveGroupCommand>
	{
		public LeaveGroupCommandValidator()
		{
			RuleFor(x => x.GroupId).NotEmpty();
			RuleFor(x => x.UserId).NotEmpty();
		}
	}
}
