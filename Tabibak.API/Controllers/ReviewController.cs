using Microsoft.AspNetCore.Mvc;
using Tabibak.API.BLL.Reviews;
using Tabibak.API.Dtos.Reviews;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IReviewBLL _reviewBLL;

    public ReviewController(IReviewBLL reviewService)
    {
        _reviewBLL = reviewService;
    }

    // ✅ Create a new review
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReviewDto dto)
    {

        var review = await _reviewBLL.CreateReviewAsync(dto);
        return Ok(review);

    }

    // ✅ Get all reviews
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _reviewBLL.GetAllReviewsAsync());
    }

    // ✅ Get reviews by doctor
    [HttpGet("doctor/{doctorId}")]
    public async Task<IActionResult> GetByDoctor(int doctorId)
    {
        return Ok(await _reviewBLL.GetReviewsByDoctorAsync(doctorId));
    }

    // ✅ Get reviews by patient
    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetByPatient(int patientId)
    {
        return Ok(await _reviewBLL.GetReviewsByPatientAsync(patientId));
    }

    // ✅ Update a review
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReviewDto dto)
    {
        var success = await _reviewBLL.UpdateReviewAsync(id, dto);
        if (!success.Data)
            return NotFound();
        return NoContent();
    }

    // ✅ Delete a review
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _reviewBLL.DeleteReviewAsync(id);
        if (!success.Data)
            return NotFound();
        return NoContent();
    }
}
