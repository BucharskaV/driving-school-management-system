using DrivingSchool.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchool.API.Controllers;

[ApiController]
[Route("api/courses")]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }
    
    [HttpGet("{categoryId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllCoursesByCategoryId(int categoryId, CancellationToken cancellationToken)
    {
        var courses = await _courseService.GetAllCoursesByCategoryIdAsync(categoryId, cancellationToken);
        return Ok(courses);
    }
}