using dalil.Core.Entities;
using dalil.Core.Entities.dalil.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static dalil.Core.DTOs.CourseDto;
using static dalil.Core.DTOs.StudentDto;

namespace dalil.Core.Mappings
{
    public static class CourseMappings
    {
        public static CourseResponseDto ToDto(this Course course)
        {
            return new CourseResponseDto(course.Id, course.Title, course.Price, course.Instructor?.FullName, course.ImageUrl);
        }
        public static Course ToEntity(this CreateCourseDto dto) => new()
        {
            Title = dto.Title,
            Price = dto.Price,
            InstructorId = dto.InstructorId,
            ImageUrl = dto.ImageUrl
        };
        public static void ToEntity(this Course course,
        UpdateCourseDto dto)
         {
            course.Title = dto.Title;
            course.Price = dto.Price;
            course.ImageUrl = dto.ImageUrl;
        }
        }
}
