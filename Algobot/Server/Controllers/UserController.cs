using BlazorAlgoBot.Server.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Algobot.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Authorize(Policy = ApiKeySchemeOptions.PolicyName)]
        [HttpGet("{password}")]
        public async Task<IActionResult> Login([FromRoute] string password) 
        {
            var singlePassword = _configuration.GetValue<string>("ApiKeyOptions:ApiKey");
            if (singlePassword == password)
            {
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
