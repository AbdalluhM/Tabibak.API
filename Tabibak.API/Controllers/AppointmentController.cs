using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabibak.Api.Enums;
using Tabibak.API.BLL.Appointments;
using Tabibak.API.Dtos.Appoinments;

namespace Tabibak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentController : BaseController
    {

        private readonly IAppointmentBLL _appointmentBLL;

        public AppointmentController(IAppointmentBLL appointmentService)
        {
            _appointmentBLL = appointmentService;
        }

        // Create appointment
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
        {
            var appointment = await _appointmentBLL.CreateAppointmentAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = appointment.Data.AppointmentId }, appointment);
        }

        [HttpPost("create")]
        [Authorize(Roles = nameof(RoleEnum.Doctor))]
        public async Task<IActionResult> CreateAvailableAppointment([FromForm] DateTime AppointmentDate)
        {

            var appointment = await _appointmentBLL.CreateAvailableAppointmentAsync(new CreateAppointmentDto
            {
                AppointmentDate = AppointmentDate,
                DoctorId = UserId,
            });
            return Ok(appointment);
        }
        [HttpPost("book")]
        [Authorize(Roles = nameof(RoleEnum.Patient))]
        public async Task<IActionResult> Book(int appointmentId)
        {

            var appointment = await _appointmentBLL.BookAppointmentAsync(appointmentId, UserId);
            return Ok(appointment);
        }
        // Get all appointments
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable(int doctorId)
        {
            return Ok(await _appointmentBLL.GetAvailableAppointmentsAsync(doctorId));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _appointmentBLL.GetAllAppointmentsAsync());
        }

        // Get appointment by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _appointmentBLL.GetAppointmentByIdAsync(id);
            if (appointment == null)
                return NotFound();
            return Ok(appointment);
        }

        // Get appointments by doctor
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            return Ok(await _appointmentBLL.GetAppointmentsByDoctorAsync(doctorId));
        }

        // Get appointments by patient
        [HttpGet("patient-booking")]
        public async Task<IActionResult> GetByPatient()
        {
            return Ok(await _appointmentBLL.GetAppointmentsByPatientAsync(UserId));
        }

        // Update an appointment
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAppointmentDto dto)
        {
            var success = await _appointmentBLL.UpdateAppointmentAsync(id, dto);
            if (!success.IsSuccess)
                return NotFound();
            return NoContent();
        }

        // Delete an appointment
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _appointmentBLL.DeleteAppointmentAsync(id);
            if (!success.IsSuccess)
                return NotFound();
            return NoContent();
        }

        [HttpPost("cancel/{appointmentId}")]
        [Authorize(Roles = nameof(RoleEnum.Patient))]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            var result = await _appointmentBLL.CancelAppointmentAsync(appointmentId, UserId);
            return Ok(result);
        }

        [HttpPost("start/{appointmentId}")]
        [Authorize(Roles = nameof(RoleEnum.Doctor))]
        public async Task<IActionResult> StartAppointment(int appointmentId)
        {
            var result = await _appointmentBLL.StartAppointmentAsync(appointmentId);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("end/{appointmentId}")]
        [Authorize(Roles = nameof(RoleEnum.Doctor))]
        public async Task<IActionResult> EndAppointment(int appointmentId)
        {
            var result = await _appointmentBLL.EndAppointmentAsync(appointmentId, UserId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Qrcode/{appointmentId}")]
        public async Task<IActionResult> GenrateQrCode(int appointmentId)
        {
            var result = await _appointmentBLL.GenerateAppointmentQrCode(appointmentId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result.Data);
        }

    }
}

