using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using ThreadLike.Common.Domain;

namespace ThreadLike.Common.Application.Behaviors;

internal sealed class RequestLoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> _logger;

	public RequestLoggingPipelineBehavior(ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
	{
		_logger = logger;
	}

	public async Task<TResponse> Handle(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		string moduleName = GetModuleName(typeof(TRequest).FullName!);
		string requestName = typeof(TRequest).Name;
		//------------------------------for tracing Otlp Purpose ------------------------------------------
		Activity.Current?.AddTag("request.module", moduleName);
		Activity.Current?.AddTag("request.name", requestName);
		// include these 2 tags when traces to Otpl
		//------------------------------for tracing Otlp Purpose ------------------------------------------


		using (LogContext.PushProperty("Module", moduleName))
		{
			_logger.LogInformation("Processing request {RequestName}", requestName);
			TResponse result = await next();
			// this type check is unnecessary, but still put here just in case, if TResponse: is not specified
			if (result is Result resultTyped)
			{
				if (resultTyped.IsSuccess)
				{
					_logger.LogInformation("Completed request {RequestName}", requestName);
				}
				else
				{
					using (LogContext.PushProperty("Error", resultTyped.Error, true))
					{
						_logger.LogError("Completed request {RequestName} with error", requestName);
					}
				}
			}
			else
			{
				_logger.LogInformation("Completed request {RequestName}", requestName);
			}
			return result;
		}
	}

	private static string GetModuleName(string requestName) => requestName.Split('.')[2];
}
