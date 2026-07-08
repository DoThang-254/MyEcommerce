using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        private IMediator? _mediator;

        protected IMediator Mediator => _mediator ??= this.HttpContext.RequestServices.GetRequiredService<IMediator>();
    }
}
