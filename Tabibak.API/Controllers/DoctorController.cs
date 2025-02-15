using Microsoft.AspNetCore.Mvc;
using Tabibak.API.BLL.Doctors;

namespace Tabibak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorBLL _doctorBLL;

        public DoctorController(IDoctorBLL doctorBLL)
        {
            _doctorBLL = doctorBLL;
        }

        [HttpGet("{specialtyId}")]
        public async Task<IActionResult> GetDoctorsBySpecialty(int specialtyId)
        {
            return Ok(await _doctorBLL.GetDoctorsBySpecialtyAsync(specialtyId));
        }
    }
}
