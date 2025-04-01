using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Application.Messages.Commands.React
{
	public record ReactToMessageCommand(string MessageId, string ReactionId, string ReactorId) : ICommand<Result>;
	internal class ReactToMessageCommandHandler : ICommandHandler<ReactToMessageCommand, Result>
	{
		public Task<Result> Handle(ReactToMessageCommand request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
