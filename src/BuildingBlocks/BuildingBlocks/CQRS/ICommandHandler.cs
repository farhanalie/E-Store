﻿using ErrorOr;
using MediatR;

namespace BuildingBlocks.CQRS;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> where TCommand : ICommand;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : IErrorOr;