using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Domain.Users
{
	public static class UserErrors
	{
		public static Error Exist(string identityId) =>  Error.Conflict("409",$"User with identityId {identityId} already exists");
		public static Error IdExist(string id) => Error.Conflict("409", $"User with id {id} already exists");
		public static Error NotFound => Error.NotFound("404", "User not found");
		public static Error WithIdNotFound(string id) => Error.NotFound("404", $"User with id {id} not found");
		public static Error RoleNotFound => Error.NotFound("404", "User role not found");

		public static Error Create(string message, string code) => new Error(code, message,ErrorType.Problem);
	}
}
