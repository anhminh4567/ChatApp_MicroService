using MediatR;
using ThreadLike.Common.Domain;

namespace ThreadLike.Common.Application.Messaging;

public interface ICommand : IRequest, IBaseCommand;

public interface ICommand<TResponse> : IRequest<TResponse>, IBaseCommand;

public interface IBaseCommand;
