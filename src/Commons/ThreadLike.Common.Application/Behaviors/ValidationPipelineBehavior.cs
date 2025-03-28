using System.Reflection;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;

namespace ThreadLike.Common.Application.Behaviors;

internal sealed class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IBaseCommand 
	// TRequest: IBaseCommand so that this pipeline only run when TRequest is implemeting IBaseCommand
	// if you want to execute this on query too => remove IBaseCommand, replace with IBaseREquest( from mediaator)
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
	{
		_validators = validators;
	}

	public async Task<TResponse> Handle(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		ValidationFailure[] validationFailures = await ValidateAsync(request);

		if (validationFailures.Length == 0)
		{
			return await next();
		}
		// this first if() : handle result if it is of type Result<>
		if (typeof(TResponse).IsGenericType &&
			typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
		{
			Type resultType = typeof(TResponse).GetGenericArguments()[0];
			MethodInfo? failureMethod = typeof(Result<>)
				.MakeGenericType(resultType)
				.GetMethod(nameof(Result<object>.ValidationFailure));
			if (failureMethod is not null)
			{
				return (TResponse)failureMethod.Invoke(null, [CreateValidationError(validationFailures)]);
			}
		}
		// this next else if() :  handle result if it is of type Result
		else if (typeof(TResponse) == typeof(Result))
		{
			return (TResponse)(object)Result.Failure(CreateValidationError(validationFailures));
		}
		throw new ValidationException(validationFailures);
	}

	private async Task<ValidationFailure[]> ValidateAsync(TRequest request)
	{
		if (!_validators.Any())
		{
			return [];
		}

		var context = new ValidationContext<TRequest>(request);

		ValidationResult[] validationResults = await Task.WhenAll(
			_validators.Select(validator => validator.ValidateAsync(context)));

		ValidationFailure[] validationFailures = validationResults
			.Where(validationResult => !validationResult.IsValid)
			.SelectMany(validationResult => validationResult.Errors)
			.ToArray();

		return validationFailures;
	}

	private static ValidationError CreateValidationError(ValidationFailure[] validationFailures) =>
		new(validationFailures.Select(f => Error.Problem(f.PropertyName, f.ErrorMessage)).ToArray());
}
