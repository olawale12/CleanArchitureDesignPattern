using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCleanArchitecture.Application.Commands;
using SmartCleanArchitecture.Infrastructure.IRepository;
using SmartCleanArchitecture.Infrastructure.Model;
using SmartCleanArchitecture.Infrastructure.Repository;

namespace SmartCleanArchitecture.Api.Controllers
{
    [Route("api/v1/SSOUser")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        IConfiguration _configuration;

        public UsersController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        [HttpPost, Route("OnboardSS0User")]
        public async Task<IActionResult> CreateSSOUser([FromBody] CreateUserCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }


        [HttpGet]
        public async Task<IEnumerable<UsersTest>> GetBrands()
        {
            return await unitOfWork.Users.Get("SELECT * FROM USERSTEST");
        }

        [HttpGet, Route("FindUser")]
        public async Task<IEnumerable<UsersTest>> FindBrands(string ID)
        {
            return await unitOfWork.Users.Get($"SELECT * FROM USERSTEST where id ={ID}");
        }
    }
}
