using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCleanArchitecture.Application.Commands;

namespace SmartCleanArchitecture.Api.Controllers
{
    [Route("api/v1/SSOUser")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        [HttpPost, Route("OnboardSS0User")]
        public async Task<IActionResult> CreateSSOUser([FromBody] CreateUserCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

    }
}
