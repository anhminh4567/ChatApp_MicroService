using MediatR;
using ThreadLike.Common.Domain;

namespace ThreadLike.Common.Application.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
	where TQuery : IQuery<TResponse>;
