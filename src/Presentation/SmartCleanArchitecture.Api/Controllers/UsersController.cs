using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCleanArchitecture.Api.Filters;
using SmartCleanArchitecture.Application.Commands;
using SmartCleanArchitecture.Application.DTOs;

namespace SmartCleanArchitecture.Api.Controllers
{
    [Route("api/v1/SSOUser")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        [HttpPost, Route("OnboardSS0User")]
        [ServiceFilter(typeof(LanguageFilter))]
        [TypeFilter(typeof(DecryptRequestDataFilter<CreateUserCommand>))]
        public async Task<IActionResult> CreateSSOUser([FromBody] BaseEncryptedRequestDTO encryptedRequestData, [FromQuery] CreateUserCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

    }
}
