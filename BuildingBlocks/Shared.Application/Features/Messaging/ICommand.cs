using MediatR;
using Shared.Application.Common.Models;

namespace Shared.Application.Features.Messaging;

// Dành cho Command có trả về dữ liệu (VD: Create trả về Guid)
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}

// Dành cho Command KHÔNG trả về dữ liệu (VD: Update, Delete)
public interface ICommand : IRequest<Result<object>>
{
}
