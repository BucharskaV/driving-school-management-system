using DrivingSchool.Services.Contracts.Requests.Lesson;
using DrivingSchool.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var lessons = await _lessonService.GetAllAsync(cancellationToken);

        return Ok(lessons);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "StudentOrAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var lesson = await _lessonService.GetByIdAsync(id, cancellationToken);

        return Ok(lesson);
    }

    [HttpPost("practical")]
    [Authorize(Policy = "AdminOnly")]
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
    [Authorize(Policy = "AdminOnly")]
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
    [Authorize(Policy = "AdminOnly")]
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
    [Authorize(Policy = "AdminOnly")]
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
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _lessonService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }
    
    [HttpGet("instructor/{instructorId:int}")]
    [Authorize(Policy = "InstructorOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllLessonsByInstructorId(int instructorId, CancellationToken cancellationToken)
    {
        var lessons = await _lessonService.GetLessonsWithProgressByInstructorIdAsync(instructorId, cancellationToken);
        return Ok(lessons);
    }
    
    [HttpPut("note/{studentId:int}/{lessonId:int}")]
    [Authorize(Policy = "InstructorOnly")]
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