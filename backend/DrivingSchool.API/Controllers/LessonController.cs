using DrivingSchool.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchool.API.Controllers;

[ApiController]
[Route("api/lessons")]
public class LessonController : ControllerBase
{
    private readonly ILessonService _lessonService;

    public LessonController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }
    
    [HttpGet("instructor/{instructorId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllCoursesByInstructorId(int instructorId, CancellationToken cancellationToken)
    {
        var lessons = await _lessonService.GetLessonsWithProgressByInstructorIdAsync(instructorId, cancellationToken);
        return Ok(lessons);
    }
    
    [HttpPut("note/{studentId:int}/{lessonId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddNoteToLesson(int studentId, int lessonId, string input, CancellationToken cancellationToken)
    {
        await _lessonService.AddNoteToLessonAsync(studentId, lessonId, input, cancellationToken);
        return Ok();
    }
}