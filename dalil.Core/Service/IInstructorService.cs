using System;
using System.Collections.Generic;
using System.Text;
using static dalil.Core.DTOs.CourseDto;
using static dalil.Core.DTOs.InstructorDto;
using static dalil.Core.DTOs.StudentDto;

namespace dalil.Core.Service
{
    public interface IInstructorService
    {
        Task<List<InstructorResponseDto>> GetAllAsync();
        Task<InstructorResponseDto?> CreateAsync(CreateInstructorDto dto);
        Task UpdateAsync(int id, UpdateInstructorDto dto);
        Task DeleteAsync(int id);
        Task<List<CourseResponseDto>> GetCoursesAsync(int id);
        Task<InstructorResponseDto?> GetByIdAsync(int id);
    }
}
