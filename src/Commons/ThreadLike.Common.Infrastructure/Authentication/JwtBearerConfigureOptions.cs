using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ThreadLike.Common.Infrastructure.Authentication;
internal class JwtBearerConfigureOptions : IConfigureNamedOptions<JwtBearerOptions>
{
	private readonly IConfiguration _configuration;
	//private const string ConfigurationSectionName = "Authentication";

	public JwtBearerConfigureOptions(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public void Configure(string? name, JwtBearerOptions options)
	{
		Configure(options);
	}

	public void Configure(JwtBearerOptions option)
	{
		 //_configuration.GetSection(AuthenticationOptions.SectionName).Get<AuthenticationOptions>();
		var authOptions = _configuration.GetSection(AuthenticationOptions.SectionName).Get<AuthenticationOptions>();
		ArgumentNullException.ThrowIfNull(authOptions);
		
		string issuer = authOptions.Authority;

		option.RequireHttpsMetadata = false;
		
		option.Authority = issuer;
		
		option.TokenValidationParameters.ValidateIssuer = true;
		option.TokenValidationParameters.ValidateAudience = false;
		option.TokenValidationParameters.ValidateLifetime = true;
		option.TokenValidationParameters.ValidateIssuerSigningKey = true;

		option.SaveToken = true;

		option.IncludeErrorDetails = true;

		option.TokenValidationParameters.ValidIssuers = [issuer];
		option.TokenValidationParameters.ValidAudiences = [authOptions.ClientId];

		//_configuration.GetSection(ConfigurationSectionName).Bind(options);
	}
}
