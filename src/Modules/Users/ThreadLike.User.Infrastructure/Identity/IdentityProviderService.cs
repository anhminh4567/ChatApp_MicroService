using Microsoft.Extensions.Options;
using ThreadLike.User.Application.Abstractions.Identity;
using ThreadLike.Common.Infrastructure.Authentication;
using ThreadLike.User.Infrastructure.Constant;
using System.Net.Http.Json;
using ThreadLike.Common.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;
using ThreadLike.Common.Application.Authentication;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
namespace ThreadLike.User.Infrastructure.Identity
{
	internal class IdentityProviderService : IIdentityProviderService
	{
		private readonly IOptions<AuthenticationOptions> _authOptions;
		private readonly IOptionsMonitor<JwtBearerOptions> _jwtOptions;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ILogger<IdentityProviderService> _logger;

		public IdentityProviderService(IOptions<AuthenticationOptions> authOptions, IOptionsMonitor<JwtBearerOptions> jwtOptions, IHttpClientFactory httpClientFactory, ILogger<IdentityProviderService> logger)
		{
			_authOptions = authOptions;
			_jwtOptions = jwtOptions;
			_httpClientFactory = httpClientFactory;
			_logger = logger;
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
				HttpResponseMessage response = await client.SendAsync(request, cancellationToken);
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
				var request = new HttpRequestMessage(HttpMethod.Get, "");

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
		/// <summary>
		/// some major shit happen heere, 5 hours of debug
		/// </summary>
		/// <param name="idToken"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<Result<IdentityProviderUserInfo>> GetUserInforFromIdTokenAsync(string idToken, CancellationToken cancellationToken = default)
		{
			TokenValidationParameters tokenValidationParameters = _jwtOptions.CurrentValue.TokenValidationParameters.Clone();
			IConfigurationManager<OpenIdConnectConfiguration>? config = _jwtOptions.CurrentValue.ConfigurationManager;

			//tokenValidationParameters.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
			//	_jwtOptions.CurrentValue.TokenValidationParameters.ValidIssuers.First(),
			//	new OpenIdConnectConfigurationRetriever(),
			//	new HttpDocumentRetriever
			//		{
			//			RequireHttps = true,
			//		}
			//	);
			tokenValidationParameters.ConfigurationManager = config as BaseConfigurationManager;
			var tokenHandler = new JwtSecurityTokenHandler();

			try
			{

				ClaimsPrincipal principal = tokenHandler.ValidateToken(idToken, tokenValidationParameters, out SecurityToken validatedToken);
				var jwtToken = (JwtSecurityToken)validatedToken;
				string sub = jwtToken.Claims.First(c => c.Type == Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub).Value;
				string email = jwtToken.Claims.First(c => c.Type == Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Email).Value;
				bool emailVerified = bool.Parse(jwtToken.Claims.First(c => c.Type == CustomClaims.EmailVerified).Value);
				string name = jwtToken.Claims.First(c => c.Type == Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Name).Value;
				return Result.Ok(new IdentityProviderUserInfo(sub, emailVerified, name, email, null));
			}
			catch (ArgumentNullException ex)
			{
				_logger.LogError(ex, "Error validating token, some field is null after decode ID_TOKEN with value Param:{ParamName}; Message:{Message}", ex.ParamName, ex.Message);
				return Result.Failure(ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error validating token: {Message}", ex.Message);
				return Result.Failure(ex.Message);
			}
		}
	}
}
