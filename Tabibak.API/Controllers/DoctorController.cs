using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabibak.Api.Enums;
using Tabibak.API.BLL.Doctors;

namespace Tabibak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(RoleEnum.Patient))]
    public class DoctorController : BaseController
    {
        private readonly IDoctorBLL _doctorBLL;

        public DoctorController(IDoctorBLL doctorBLL)
        {
            _doctorBLL = doctorBLL;
        }

        [HttpGet("{specialtyId}")]

        public async Task<IActionResult> GetDoctorsBySpecialty(int specialtyId,
                                                                int? locationId = null,
                                                                bool acceptPromoCode = false,
                                                                decimal? minFees = null,
                                                                decimal? maxFees = null,
                                                                DateTime? dateFilter = null,
                                                                string gender = null)
        {
            return Ok(await _doctorBLL.GetDoctorsBySpecialtyAsync(UserId, specialtyId, locationId, acceptPromoCode, minFees, maxFees, dateFilter, gender));
        }

        [HttpGet("details/{doctorId}")]
        public async Task<IActionResult> GetDoctorDetails(int doctorId)
        {
            var result = await _doctorBLL.GetDoctorDetailsAsync(doctorId, UserId);
            return Ok(result);
        }

    }
}
