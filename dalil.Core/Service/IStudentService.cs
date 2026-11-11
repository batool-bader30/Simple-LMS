using dalil.Core.DTOs;
using dalil.Core.Entities;
using dalil.Core.Entities.dalil.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static dalil.Core.DTOs.CourseDto;
using static dalil.Core.DTOs.StudentDto;

namespace dalil.Core.Service
{
    public interface IStudentService
    {
        Task<List<StudentResponseDto>> GetAllAsync();
        
        Task<StudentResponseDto?> GetByIdAsync(int id);
        Task<List<CourseResponseDto?>> GetByIdWithCourses(int id); 
        Task<StudentResponseDto?> CreateAsync(CreateStudentDto dto);
        Task UpdateAsync(int id, UpdateStudentDto dto);
        Task DeleteAsync(int id);
        Task<List<CourseResponseDto>> GetCoursesAsync(int id);
        Task<PagedResult<StudentResponseDto>> GetPagedAsync(int page, int pageSize);

    }
}
