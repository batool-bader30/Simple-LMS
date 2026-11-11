using System;
using System.Collections.Generic;
using System.Text;

namespace dalil.Core.DTOs
{
    public record CourseDto
    {
        public record CourseResponseDto(int Id, string Title, decimal Price, string? InstructorName, string? ImageUrl);
        public record CreateCourseDto(string Title, decimal Price, int InstructorId, string? ImageUrl);
        public record UpdateCourseDto(string Title, decimal Price, string? ImageUrl);
    }
}
