using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebAppi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ControllerRecipiente:ControllerBase
    {

        private IMediator _mediador;
        protected IMediator Mediator => _mediador ?? (_mediador=HttpContext.RequestServices.GetService<IMediator>());
        
    }
}