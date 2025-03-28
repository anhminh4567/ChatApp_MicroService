using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ThreadLike.Common.Infrastructure.Authentication;
internal static class AuthenticationExtensions
{
#pragma warning disable IDE0060
	internal static IServiceCollection AddAuthenticationInternal(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddAuthentication().AddJwtBearer(option =>
		{
			// we dont configure stuff here
			// we do it in ConfugreOption ( Options pattern )
		
		});
		services.AddHttpContextAccessor();
		services.ConfigureOptions<JwtBearerConfigureOptions>();
		services.Configure<AuthenticationOptions>(configuration.GetSection(AuthenticationOptions.SectionName));
		return services;
	}
}
