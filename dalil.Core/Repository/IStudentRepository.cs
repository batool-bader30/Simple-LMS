using dalil.Core.DTOs;
using dalil.Core.Entities;
using dalil.Core.Entities.dalil.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static dalil.Core.DTOs.CourseDto;
using static dalil.Core.DTOs.StudentDto;

namespace dalil.Core.Repository
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
         Task<Student?> GetByIdWithCourses(int id);
        

        Task<Student> CreateAsync(Student student);

        Task UpdateAsync(Student student);

        Task<bool> DeleteAsync(int id);
        Task<Student?> GetWithCoursesAsync(int id);
        Task<PagedResult<StudentResponseDto>> GetPagedAsync(int page, int pageSize);
    }
}
