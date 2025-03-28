using System.Diagnostics;
using Serilog.Context;

namespace ThreadLike.Chat.Api.Middlewares;

internal sealed class LogContextTraceLoggingMiddleware : IMiddleware
{
	public Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		string traceId = Activity.Current?.TraceId.ToString();


		// push trace id to a property 
		// include traceId value for all of logs inside current API request
		// allow to see the trace in Jaeger, Seq, etc.
		using (LogContext.PushProperty("TraceId", traceId))
		{
			return next.Invoke(context);
		}
	}
}
