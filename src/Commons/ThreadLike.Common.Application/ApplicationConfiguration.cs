using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ThreadLike.Common.Application.Behaviors;
using ThreadLike.Common.Application.Messaging;

namespace ThreadLike.Common.Application
{
	public static class ApplicationConfiguration
	{
		public static IServiceCollection AddApplication(this IServiceCollection services, Assembly[] modulesAssemblies)
		{
			services.AddMediatR((config) =>
			{
				config.RegisterServicesFromAssemblies(modulesAssemblies);
				config.AddOpenBehavior(typeof(ExceptionHandlingPipelineBehavior<,>));
				config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
				config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
			});
			services.AddValidatorsFromAssemblies(modulesAssemblies, includeInternalTypes: true);

			
			return services;
		}
	}
}


