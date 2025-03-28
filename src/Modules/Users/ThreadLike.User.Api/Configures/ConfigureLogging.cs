using Serilog;

namespace ThreadLike.User.Api.Configures
{
	public static class ConfigureLogging
	{
		public static IServiceCollection AddAndConfigureLogging(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSerilog((services, config) =>
			{
				config.ReadFrom.Configuration(configuration);
			}, false, false);
			return services;
		}
	}
}
