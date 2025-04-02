using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Domain.Groups
{
    public static class GroupErrors
    {
		public static class ParticipantErrors
		{
			public static Error NotFound => Error.NotFound("404", "Participant not found");
			public static Error AlreadyExists => Error.Conflict("409", "Participant already exists");
			public static Error IsNotGroupLeader(string id) => Error.Conflict( "403", $"User id {id} is not group leader");
		}
		public static Error Exist(string name) => Error.Conflict("409", $"Group with name {name} already exists");
		public static Error IdExist(string id) => Error.Conflict("409", $"Group with id {id} already exists");
		public static Error NotFound => Error.NotFound("404", "Group not found");
		public static Error WithIdNotFound(string id) => Error.NotFound("404", $"Group with id {id} not found");
		public static Error UserNotFound => Error.NotFound("404", "User not found");
		public static Error Create(string message, string code) => new Error(code, message, ErrorType.Problem);
	}
}
