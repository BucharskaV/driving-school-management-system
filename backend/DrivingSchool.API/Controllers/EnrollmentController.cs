using DrivingSchool.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchool.API.Controllers;

[ApiController]
[Route("api/enrollments")]
public class EnrollmentController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }
    
    [HttpPost("{courseId:int}/{studentId:int}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EnrollStudentInCourse(int courseId, int studentId, CancellationToken cancellationToken)
    {
        await _enrollmentService.EnrollStudentInCourseAsync(courseId, studentId, cancellationToken);
        return Ok();
    }
}