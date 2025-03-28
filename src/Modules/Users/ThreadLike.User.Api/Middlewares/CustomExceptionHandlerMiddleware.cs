
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ThreadLike.User.Api.Middlewares;

internal class CustomExceptionHandlerMiddleware : IMiddleware
{
	private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

	public CustomExceptionHandlerMiddleware(ILogger<CustomExceptionHandlerMiddleware> logger)
	{
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unhandled exception occurred");

			var problemDetails = new ProblemDetails
			{
				Status = StatusCodes.Status500InternalServerError,
				Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
				Title = "Server failure"
			};
			context.Response.StatusCode = problemDetails.Status.Value;
			await context.Response.WriteAsJsonAsync(problemDetails);
			//_logger.LogError("exception catched in middleware with messager: {message}", ex.Message);
			//_logger.LogError(ex.StackTrace);
			//var problemDetailResponse = _problemDetailsFactory.CreateProblemDetails(context, StatusCodes.Status500InternalServerError, detail: ex.Message);
			//context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			//await context.Response.WriteAsJsonAsync(problemDetailResponse);
		}
	}
}
