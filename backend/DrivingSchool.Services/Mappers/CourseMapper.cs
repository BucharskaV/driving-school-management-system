using DrivingSchool.Domain.Models;
using DrivingSchool.Services.DTOs;

namespace DrivingSchool.Services.Mappers;

public class CourseMapper
{
    public static CourseWithCategoryDto MapToDtoWithCategory(Course course)
    {
        return new CourseWithCategoryDto(
            course.Id,
            course.Title,
            course.Price,
            course.CategoryId,
            course.Category.Name);
    }
    
    public static CourseDto MapToDto(Course course)
    {
        return new CourseDto(
            course.Id,
            course.Title,
            course.Price,
            course.Lessons
                .OrderBy(l => l.SequenceNumber)
                .Select(LessonMapper.MapToDto)
                .ToList());
    }
    
    public static EnrolledCourseDto MapToEnrolledDto(Course course)
    {
        return new EnrolledCourseDto(
            course.Id,
            course.Title,
            course.Price,
            course.Lessons
                .OrderBy(l => l.SequenceNumber)
                .Select(LessonMapper.MapToDto)
                .ToList());
    }
}