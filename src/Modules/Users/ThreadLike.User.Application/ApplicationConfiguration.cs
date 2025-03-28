using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.User.Application
{
	public static class ApplicationConfiguration
	{
		public static IServiceCollection AddUserApplication(this IServiceCollection services)
		{
			//most have been configured in AddApplication() in Commons
			return services;
		}
	}
}
