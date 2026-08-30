using DrivingSchool.Domain.Enums;
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ValidateAvailabilityForBooking([FromQuery] ValidateAvailabilityRequest request, CancellationToken cancellationToken)
    {
        var status = await _lessonService.ValidateAvailabilityAsync(request, cancellationToken);

        return Ok(new ValidateAvailabilityResponse(status.ToString()));
    }
    
    [HttpPut("book")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> BookLesson([FromBody] BookLessonRequest request, CancellationToken cancellationToken)
    {
        await _lessonService.BookLessonAsync(request, cancellationToken);

        return Ok();
    }
    
    [HttpPut("status/{studentId:int}/{lessonId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangeBookingStatus(int studentId, int lessonId, ProgressStatus status, CancellationToken cancellationToken)
    {
        await _lessonService.ChangeBookingStatusAsync(studentId, lessonId, status, cancellationToken);
        return Ok();
    }
    
    [HttpGet("instructor/{instructorId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBookingsByInstructor(int instructorId, CancellationToken cancellationToken)
    {
        var bookings = await _lessonService.GetBookingsByInstructorIdAsync(instructorId, cancellationToken);

        return Ok(bookings);
    }

    [HttpGet("student/{studentId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBookingsByStudent(int studentId, CancellationToken cancellationToken)
    {
        var bookings = await _lessonService.GetBookingsByStudentIdAsync(studentId, cancellationToken);

        return Ok(bookings);
    }
}