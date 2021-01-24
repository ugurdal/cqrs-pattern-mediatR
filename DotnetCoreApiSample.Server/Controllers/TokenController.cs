using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetCoreApiSample.Server.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("token")]
    public class TokenController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {



            return Ok();
        }
    }
}