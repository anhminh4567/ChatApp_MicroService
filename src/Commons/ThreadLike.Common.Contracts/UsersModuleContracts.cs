using ThreadLike.Common.Contracts.Abstracts;

namespace ThreadLike.Common.Contracts
{
	public static class UsersModuleContracts
	{
		public record UserCreatedIntegrationEvent(string Id, DateTime OccurredOnUtc, string UserId, string Email, string Name, string IdentityId, bool IsVerified)
			: IntegrationEvent(Id, OccurredOnUtc);
		
		public record UserUpdatedIntegrationEvent(string Id, DateTime OccurredOnUtc, string UserId, string Email, string Name, string IdentityId, bool IsVerified)
			: IntegrationEvent(Id, OccurredOnUtc);

		public record GetUserRolesRequest(string IdentityId);
		public record GetUserRolesResponse(string? userId, List<string> Roles);
	}
}
