using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Contracts.Requests;
using DrivingSchool.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchool.API.Controllers;

[ApiController]
[Route("api/instructors")]
public class InstructorController : ControllerBase
{
    private readonly IInstructorService _instructorService;

    public InstructorController(IInstructorService instructorService)
    {
        _instructorService = instructorService;
    }

    [HttpGet("lesson/{lessonId}")]
    [ProducesResponseType(typeof(List<Instructor>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetInstructorsByLessonId(int lessonId, CancellationToken cancellationToken)
    {
        var instructors = await _instructorService.GetInstructorsByLessonIdAsync(lessonId,cancellationToken);

        return Ok(instructors);
    }

    [HttpGet("availability")]
    public async Task<IActionResult> ValidateAvailability(
        [FromQuery] ValidateInstructorAvailabilityRequest request,
        CancellationToken cancellationToken)
    {
        var available = await _instructorService.ValidateAvailabilityAsync(request, cancellationToken);

        return Ok(available);
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableInstructors(
        [FromQuery] GetAvailableInstructorsRequest request,
        CancellationToken cancellationToken)
    {
        var instructors = await _instructorService.GetAvailableInstructorsAsync(request, cancellationToken);

        return Ok(instructors);
    }
    
    [HttpGet("salary/{instructorId:int}")]
    [ProducesResponseType(typeof(List<Instructor>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSalaryInfo(int instructorId, CancellationToken cancellationToken)
    {
        var salaryInfo = await _instructorService.GetSalaryInfoAsync(instructorId, cancellationToken);

        return Ok(salaryInfo);
    }
}