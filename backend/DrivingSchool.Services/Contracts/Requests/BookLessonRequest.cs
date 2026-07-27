using DrivingSchool.Domain.Models;
using DrivingSchool.Services.DTOs;

namespace DrivingSchool.Services.Contracts.Requests;

public record BookLessonRequest(int LessonId, int StudentId, 
    DateTime StartTime, DateTime EndTime, int InstructorId);