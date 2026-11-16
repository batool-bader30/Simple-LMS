using dalil.Core.Entities;
using dalil.Core.Entities.dalil.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static dalil.Core.DTOs.EnrollmentDto;
using static dalil.Core.DTOs.StudentDto;

namespace dalil.Core.Service
{
    public interface IEnrollmentService
    {
        Task<EnrollmentResponseDto> EnrollAsync(CreateEnrollmentDto dto);
        Task<List<StudentResponseDto>> GetStudentsByCourseIdAsync(int courseId);
        Task<List<EnrollmentResponseDto>> GetAllStudentsEnrolledAsync();
        Task UpdateAsync(int id, CreateEnrollmentDto dto);
        Task DeleteAsync(int id);
    }
}
