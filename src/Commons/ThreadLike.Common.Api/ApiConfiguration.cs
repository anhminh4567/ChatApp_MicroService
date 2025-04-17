using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Common.Api
{
	public static class ApiConfiguration
	{
		
		public static IServiceCollection AddApiModule(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddCors(options =>
			{
				options.AddPolicy(CorsPolicy.AllowAll, builder =>
				{
					builder.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader();
				});
				options.AddPolicy(CorsPolicy.AllowClientSPA, policy =>
				{
					policy.WithOrigins("http://localhost:3100")
						.AllowAnyHeader()
						.AllowAnyMethod()
						.AllowCredentials();
				});
			});
			return services;
		}
	}
}
