using Microsoft.AspNetCore.Mvc;

namespace Tabibak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        [HttpGet]

        public IActionResult Get()
        {
            return Ok("asdasda");
        }
    }
}
