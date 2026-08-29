using DrivingSchool.Services.Contracts.Requests.Lesson;
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
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var lessons = await _lessonService.GetAllAsync(cancellationToken);

        return Ok(lessons);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var lesson = await _lessonService.GetByIdAsync(id, cancellationToken);

        return Ok(lesson);
    }

    [HttpPost("practical")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePractical([FromBody] CreatePracticalLessonRequest request, CancellationToken cancellationToken)
    {
        var lesson = await _lessonService.CreatePracticalAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = lesson.Id },
            lesson);
    }

    [HttpPost("theoretical")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateTheoretical([FromBody] CreateTheoreticalLessonRequest request, CancellationToken cancellationToken)
    {
        var lesson = await _lessonService.CreateTheoreticalAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = lesson.Id },
            lesson);
    }

    [HttpPut("{id:int}/practical")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePractical(int id, [FromBody] UpdatePracticalLessonRequest request, CancellationToken cancellationToken)
    {
        var lesson = await _lessonService.UpdatePracticalAsync(id, request, cancellationToken);

        return Ok(lesson);
    }

    [HttpPut("{id:int}/theoretical")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateTheoretical(int id, [FromBody] UpdateTheoreticalLessonRequest request, CancellationToken cancellationToken)
    {
        var lesson = await _lessonService.UpdateTheoreticalAsync(id, request, cancellationToken);

        return Ok(lesson);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _lessonService.DeleteAsync(id, cancellationToken);

        return NoContent();
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