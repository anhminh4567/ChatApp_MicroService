using FluentValidation;
using ThreadLike.Chat.Application.Messages.Dtos;

namespace ThreadLike.Chat.Application.Messages.Commands.AddManyToGroup
{
	public class AddNewMessagesToGroupCommandValidator : AbstractValidator<AddNewMessagesToGroupCommand>
	{
		public AddNewMessagesToGroupCommandValidator()
		{
			var messageRequestDtoValidator = new MessageDtoValidator();
			RuleFor(x => x.NewMessages).NotNull();
			RuleForEach(x => x.NewMessages).SetValidator(messageRequestDtoValidator);
			RuleFor(x => x.GroupId).NotEmpty();
		}
		
	}
	
}
