using Microsoft.Extensions.DependencyInjection;

namespace ThreadLike.Chat.Application
{
	public static class ApplicationConfiguration
	{
		public static IServiceCollection AddChatApplication(this IServiceCollection services)
		{
			return services;
		}
	}
}
