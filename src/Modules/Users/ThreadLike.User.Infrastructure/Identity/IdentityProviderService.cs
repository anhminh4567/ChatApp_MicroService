using Microsoft.Extensions.Options;
using ThreadLike.User.Application.Abstractions.Identity;
using ThreadLike.Common.Infrastructure.Authentication;
using ThreadLike.User.Infrastructure.Constant;
using System.Net.Http.Json;
using ThreadLike.Common.Domain;

namespace ThreadLike.User.Infrastructure.Identity
{
	internal class IdentityProviderService : IIdentityProviderService
	{
		private readonly IOptions<AuthenticationOptions> _authOptions;
		private readonly IHttpClientFactory _httpClientFactory;

		public IdentityProviderService(IOptions<AuthenticationOptions> authOptions, IHttpClientFactory httpClientFactory)
		{
			_authOptions = authOptions;
			_httpClientFactory = httpClientFactory;
		}

		public async Task<Result<IdentityProviderTokenResponse>> ExchangeCodeForTokenAsync(string code, CancellationToken cancellationToken = default)
		{
			try
			{
				var request = new HttpRequestMessage(HttpMethod.Post, "");
				request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
				{
					{ "grant_type", "authorization_code" },
					{ "client_id", _authOptions.Value.ClientId },
					{ "client_secret", _authOptions.Value.ClientSecret },
					{ "code", code },
					{ "redirect_uri", _authOptions.Value.RedirectUri }
				});
				HttpClient client = _httpClientFactory.CreateClient(NamedHttpClients.AWSCognitoTokenHttpClient);
				HttpResponseMessage response = await client.SendAsync(request,cancellationToken);
				response.EnsureSuccessStatusCode();

				var content = await response.Content.ReadFromJsonAsync<IdentityProviderTokenResponse>(cancellationToken);
				return Result.Ok(content)!;
			}
			catch (HttpRequestException ex)
			{
				return Result.Failure(ex.Message);
			}


		}

		public async Task<Result<IdentityProviderUserInfo>> GetUserInfoAsync(string accessToken, CancellationToken cancellationToken = default)
		{
			try
			{
				var request = new HttpRequestMessage(HttpMethod.Get,"");

				HttpClient client = _httpClientFactory.CreateClient(NamedHttpClients.AWSCognitoMetadataHttpClient);
				var response = await client.SendAsync(request, cancellationToken);
				response.EnsureSuccessStatusCode();

				var content = await response.Content.ReadFromJsonAsync<IdentityProviderUserInfo>(cancellationToken);
				return Result.Ok(content)!;
			}
			catch (HttpRequestException ex)
			{
				return Result.Failure(ex.Message);
			}
		}
	}
}
