using ThreadLike.Common.Domain;

namespace ThreadLike.User.Application.Abstractions.Identity;

public static class IdentityProviderErrrors
{
	public static readonly Error EmailIsNotUnique = Error.Conflict(
	"Identity.EmailIsNotUnique",
	"The specified email is not unique.");
}
