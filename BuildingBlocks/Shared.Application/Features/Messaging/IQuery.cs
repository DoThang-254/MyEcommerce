using MediatR;
using Shared.Application.Common.Models;


namespace Shared.Application.Features.Messaging;

// Dành cho Query (Chỉ đọc dữ liệu)
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}

public interface IQueryPagedResult<TResponse> : IRequest<Result<PagedResult<TResponse>>>
{
}
