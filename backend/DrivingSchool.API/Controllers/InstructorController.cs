using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Contracts.Requests;
using DrivingSchool.Services.Contracts.Requests.Instructor;
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
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var instructors = await _instructorService.GetAllAsync(cancellationToken);

        return Ok(instructors);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var instructor = await _instructorService.GetByIdAsync(id, cancellationToken);

        return Ok(instructor);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateInstructorRequest request, CancellationToken cancellationToken)
    {
        var instructor = await _instructorService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = instructor.Id },
            instructor);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateInstructorRequest request, CancellationToken cancellationToken)
    {
        var instructor = await _instructorService.UpdateAsync(id, request, cancellationToken);

        return Ok(instructor);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _instructorService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }
    
    [HttpGet("{instructorId:int}/specializations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpecializations(int instructorId, CancellationToken cancellationToken)
    {
        var specializations = await _instructorService.GetSpecializationsAsync(instructorId, cancellationToken);

        return Ok(specializations);
    }
    
    [HttpGet("{instructorId:int}/certifications")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCertifications(int instructorId, CancellationToken cancellationToken)
    {
        var certifications = await _instructorService.GetCertificationsAsync(instructorId, cancellationToken);

        return Ok(certifications);
    }
    
    [HttpPost("{instructorId:int}/specializations/practical")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddPracticalSpecialization(int instructorId, [FromBody] AddPracticalSpecializationRequest request, CancellationToken cancellationToken)
    {
        await _instructorService.AddPracticalSpecializationAsync(instructorId, request, cancellationToken);

        return Ok();
    }
    
    [HttpPost("{instructorId:int}/specializations/theoretical")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddTheoreticalSpecialization(int instructorId, CancellationToken cancellationToken)
    {
        await _instructorService.AddTheoreticalSpecializationAsync(instructorId, cancellationToken);

        return Ok();
    }
    
    [HttpDelete("{instructorId:int}/specializations/{type}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveSpecialization(int instructorId, InstructorType type, CancellationToken cancellationToken)
    {
        await _instructorService.RemoveSpecializationAsync(instructorId, type, cancellationToken);

        return NoContent();
    }
    
    [HttpPost("{instructorId:int}/certifications")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddCertification(int instructorId, [FromBody] AddCertificationRequest request, CancellationToken cancellationToken)
    {
        var certification = await _instructorService.AddCertificationAsync(instructorId, request, cancellationToken);

        return Ok(certification);
    }
    
    [HttpDelete("{instructorId:int}/certifications/{certificationId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveCertification(int instructorId, int certificationId, CancellationToken cancellationToken)
    {
        await _instructorService.RemoveCertificationAsync(instructorId, certificationId, cancellationToken);

        return NoContent();
    }
}