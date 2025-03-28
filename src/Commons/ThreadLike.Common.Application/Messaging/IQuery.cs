using MediatR;
using ThreadLike.Common.Domain;

namespace ThreadLike.Common.Application.Messaging;

public interface IQuery<TResponse> : IRequest<TResponse>;
