using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;

namespace ThreadLike.Common.Domain;

public abstract class ResultBase
{
	public bool IsSuccess { get; init; }

	public bool IsFailure => !IsSuccess;
	public string? Reasons { get; init; }
	public Error? Error { get; protected set; }
}
public class Result : ResultBase
{
	//public Error Error { get; init; }
	public Result(bool isSuccess, Error? error)
	{
		if (isSuccess && error != null ||
			!isSuccess && error == null)
		{
			throw new ArgumentException("Invalid error", nameof(error));
		}
		IsSuccess = isSuccess;
		Error = error;
	}
	public static Result Ok() => new(true, null);
	public static Result<TValue> Ok<TValue>(TValue value) => new(value, true, null);
	public static Result Failure(Error error) => new(false, error);
	public static Result Failure(string message) => new(false, new Error("400",message,ErrorType.Failure));
	public static Result<TValue> Failure<TValue>(Error error) => new Result<TValue>(default, false, error);


	public static Result<TValue> Success<TValue>(TValue value) =>
		new(value, true, null);
	public Result<TValue> ToResult<TValue>(TValue value)
	{
		return new Result<TValue>(value, IsSuccess, Error);
	}


}

public class Result<TValue> : ResultBase
{
	private readonly TValue? _value;

	public Result(TValue? value, bool isSuccess, Error error)
	//: base(isSuccess, error)
	{
		_value = value;
		if (isSuccess && error != null || !isSuccess && error == null)
		{
			throw new ArgumentException("Invalid error", nameof(error));
		}
		IsSuccess = isSuccess;
		Error = error;
	}

	//[NotNull]
	public TValue Value => IsSuccess
		? _value!
		: throw new InvalidOperationException("The value of a failure result can't be accessed.");

	public static implicit operator Result<TValue>(TValue? value) =>
		value is not null ? Ok(value) : Failure<TValue>(Error.NullValue);

	public static implicit operator Result<TValue>(Result result)
	{
		if (result.IsSuccess)
			return Ok(default(TValue));
		else
			return Failure<TValue>(result.Error!);

	}

	public static Result<TValue> Ok<TValue>(TValue value) =>
		new(value, true, null);
	public Result<TValue> ToResult<TValue>(TValue value)
	{
		return new Result<TValue>(value, IsSuccess, Error);
	}
	public static Result<TValue> Failure<TValue>(Error error) =>
		new(default, false, error);
	public static Result<TValue> ValidationFailure(Error error) =>
		new(default, false, error);
}