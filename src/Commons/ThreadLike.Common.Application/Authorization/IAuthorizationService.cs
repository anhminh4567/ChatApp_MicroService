using ThreadLike.Common.Application.Authorization.Response;
using ThreadLike.Common.Domain;

namespace ThreadLike.Common.Application.Authorization;

// this interface is for each module to immplement their own services
// not for Common.Infra, BUT It is used in Infra ( Authorization, CustomClaimTransformation
public interface IAuthorizationService
{
	public const string RolePrefix = "roles_from_user";
	Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId);
	Task<Result<RoleResponse>> GetResultRolesAsync(string identityId);
	 string GetCacheKey(string prefix, string identityId) => $"{prefix}:{identityId}";


}
