﻿using MediatR;
using ThreadLike.Common.Domain;

namespace ThreadLike.Common.Application.Messaging;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
	where TCommand : ICommand;


public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
	where TCommand : ICommand<TResponse>;
