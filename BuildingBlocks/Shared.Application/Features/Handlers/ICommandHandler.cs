using MediatR;
using Shared.Application.Common.Models;
using Shared.Application.Features.Messaging;

namespace Shared.Application.Features.Handlers;

public interface ICommandHandler<TCommand, TResponse>
    : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}

public interface ICommandHandler<TCommand>
    : IRequestHandler<TCommand, Result<object>>
    where TCommand : ICommand
{
}
