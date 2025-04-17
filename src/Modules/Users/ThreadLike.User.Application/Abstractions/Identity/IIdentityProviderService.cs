using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Domain;
namespace ThreadLike.User.Application.Abstractions.Identity;
public record IdentityProviderTokenResponse
{
	public string id_token { get; set; }
	public string access_token { get; set; }
	public string refresh_token { get; set; }
	public int expires_in { get; set; }
	public string token_type { get; set; }
}
public record IdentityProviderUserInfo(
	string Sub,
	bool EmailVerified,
	string Name,
	string Email,
	string? Username
);
public interface IIdentityProviderService
{
	//Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken token = default);
	//Task<IdentityProviderTokenResponse> GetTokenFromAuthorizationCode 
	Task<Result<IdentityProviderTokenResponse>> ExchangeCodeForTokenAsync(string code, CancellationToken cancellationToken = default);
	Task<Result<IdentityProviderUserInfo>> GetUserInfoAsync(string accessToken, CancellationToken cancellationToken = default);
	Task<Result<IdentityProviderUserInfo>> GetUserInforFromIdTokenAsync(string idToken, CancellationToken cancellationToken = default);
}
