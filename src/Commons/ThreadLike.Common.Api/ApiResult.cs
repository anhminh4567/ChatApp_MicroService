using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ThreadLike.Common.Domain;

namespace ThreadLike.Common.Api
{
	public static class ApiResult
	{
		public static IResult MatchError(Error error)
		{
			if (error is ValidationError validationError)
			{
				string message = "Validation error";
				Dictionary<string, string[]> errorKVP = new();
				foreach (Error err in validationError.Errors)
				{
					// for validation error, the code is the propertyName , see the pipelineBehavior
					errorKVP.Add(err.Code, [err.Description]);
				}
				return Results.ValidationProblem(errorKVP, title: message);// errors.First(x => x is ValidationError).Message
			}
			return Problem(error);
		}
		private static IResult Problem(Error error)
		{
			(int statusCode, string message) = error.Type switch
			{
				ErrorType.Conflict => (StatusCodes.Status409Conflict, error.Description),
				ErrorType.Problem => (StatusCodes.Status400BadRequest, error.Description),
				ErrorType.NotFound => (StatusCodes.Status404NotFound, error.Description),
				ErrorType.Validation => (StatusCodes.Status400BadRequest, error.Description),
				ErrorType.Failure => (StatusCodes.Status500InternalServerError, error.Description),
				_ => (StatusCodes.Status400BadRequest, error.Description),
			};

			return Results.Problem(statusCode: statusCode, detail: message);
		}
	}
}
