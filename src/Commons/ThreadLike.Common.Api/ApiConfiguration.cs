using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Common.Api
{
	public static class ApiConfiguration
	{
		
		public static IServiceCollection AddApiModule(this IServiceCollection services, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
		{
			services.AddCors(options =>
			{
				options.AddPolicy(CorsPolicy.AllowAll, builder =>
				{
					builder.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader();
				});
				string origin = configuration.GetSection("Client:Web").Value;
			
				options.AddPolicy(CorsPolicy.AllowClientSPA, policy =>
				{
					policy.WithOrigins(origin)
						.AllowAnyHeader()
						.AllowAnyMethod()
						.AllowCredentials();
				});
			});
			return services;
		}
	}
}
