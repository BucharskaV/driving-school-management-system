using DrivingSchool.Services.Contracts.Requests;
using DrivingSchool.Services.Contracts.Responses;
using DrivingSchool.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchool.API.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingController : ControllerBase
{
    private readonly ILessonService _lessonService;

    public BookingController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }
    
    [HttpGet("availability")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ValidateAvailabilityForBooking([FromQuery] ValidateAvailabilityRequest request, CancellationToken cancellationToken)
    {
        var status = await _lessonService.ValidateAvailabilityAsync(request, cancellationToken);

        return Ok(new ValidateAvailabilityResponse(status.ToString()));
    }
}