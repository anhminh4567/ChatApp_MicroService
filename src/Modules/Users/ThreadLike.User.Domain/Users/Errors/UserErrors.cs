using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Domain;

namespace ThreadLike.User.Domain.Users.Errors
{
	public static class UserErrors
	{
		public static Error UserExist(string identityId) =>  Error.Conflict("409",$"User with identityId {identityId} already exists");
		public static Error UserIdExist(string id) => Error.Conflict("409", $"User with id {id} already exists");
		public static Error UserRegisterError(string email) => Error.Failure("500", $"Error registering user with email {email}");
		public static Error UserNotFound => Error.NotFound("404", "User not found");

		public static Error UserRoleNotFound => Error.NotFound("404", "User role not found");
	}
}
