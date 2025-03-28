using ThreadLike.Common.Domain;

namespace ThreadLike.Common.Application.Exceptions;

public sealed class TheadlikeApplicationException : Exception
{
	public TheadlikeApplicationException(string requestName, Error? error = default, Exception? innerException = default)
		: base("Application exception", innerException)
	{
		RequestName = requestName;
		Error = error;
	}

	public string RequestName { get; }
	public Error? Error { get; }
}
