using Microsoft.AspNetCore.Mvc;
using Tabibak.API.BLL.Specialties;

namespace Tabibak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialtyController : ControllerBase
    {
        private readonly ISpecialtyBLL _specialtyBLL;

        public SpecialtyController(ISpecialtyBLL specialtyBLL)
        {
            _specialtyBLL = specialtyBLL;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _specialtyBLL.GetSpecialties());
        }


    }
}
