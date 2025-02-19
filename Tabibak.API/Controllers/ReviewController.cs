using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabibak.Api.BLL.BaseReponse;
using Tabibak.API.BLL.Reviews;
using Tabibak.API.Controllers;
using Tabibak.API.Core.Models;
using Tabibak.API.Dtos;
using Tabibak.API.Dtos.Reviews;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = nameof(Patient))]
public class ReviewController : BaseController
{
    private readonly IReviewBLL _reviewBLL;

    public ReviewController(IReviewBLL reviewService)
    {
        _reviewBLL = reviewService;
    }

    // ✅ Create a new review
    [HttpPost]
    [Produces<IResponse<ReviewResponseDto>>]
    public async Task<IActionResult> Create([FromBody] CreateReviewDto dto)
    {
        dto.PatientId = UserId;

        var review = await _reviewBLL.CreateReviewAsync(dto);
        return Ok(review);

    }

    // ✅ Get all reviews
    [HttpGet]
    [Produces<IResponse<List<ReviewResponseDto>>>]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _reviewBLL.GetAllReviewsAsync());
    }

    // ✅ Get reviews by doctor
    [HttpGet("doctor/{doctorId}")]
    [Produces<PagedResult<ReviewResponsePagedDto>>]
    public async Task<IActionResult> GetDoctorReviews(int doctorId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1)
            return BadRequest("Page number and size must be greater than zero.");

        var result = await _reviewBLL.GetDoctorReviewsAsync(doctorId, pageNumber, pageSize);
        return Ok(result);
    }

    // ✅ Get reviews by patient
    [HttpGet("patient/{doctorId}")]
    [Produces<PagedResult<ReviewResponsePagedDto>>]
    public async Task<IActionResult> GetPatientReviews(int patientId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1)
            return BadRequest("Page number and size must be greater than zero.");

        var result = await _reviewBLL.GetPatientReviewsAsync(patientId, pageNumber, pageSize);
        return Ok(result);
    }

    // ✅ Update a review
    [HttpPut("{id}")]
    [Produces<IResponse<bool>>]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReviewDto dto)
    {
        var success = await _reviewBLL.UpdateReviewAsync(id, dto);
        if (!success.Data)
            return NotFound();
        return NoContent();
    }

    // ✅ Delete a review
    [HttpDelete("{id}")]
    [Produces<IResponse<bool>>]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _reviewBLL.DeleteReviewAsync(id);
        if (!success.Data)
            return NotFound();
        return NoContent();
    }
}
