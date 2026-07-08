using MediatR;
using Shared.Application.Common.Models;
using Shared.Application.Features.Messaging;

namespace Shared.Application.Features.Handlers;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}

public interface IQueryHandlerPagedResult<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<PagedResult<TResponse>>>
    where TQuery : IQueryPagedResult<TResponse>
{
}