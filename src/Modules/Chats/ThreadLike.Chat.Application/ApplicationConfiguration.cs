using Dapper;
using Microsoft.Extensions.DependencyInjection;
using ThreadLike.Chat.Domain.Shared;
using ThreadLike.Common.Application.DapperTypeHandlers;

namespace ThreadLike.Chat.Application
{
	public static class ApplicationConfiguration
	{
		public static IServiceCollection AddChatApplication(this IServiceCollection services)
		{
			SqlMapper.AddTypeHandler(new JsonTypeHandler<MediaObject>());
			return services;
		}
	}
}
