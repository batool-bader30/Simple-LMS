using dalil.Core.DTOs;
using dalil.Core.Entities;
using dalil.Core.Entities.dalil.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static dalil.Core.DTOs.CourseDto;

namespace dalil.Core.Repository
{
    public interface ICourseRepository
    {
        Task<PagedResult<CourseResponseDto>> GetPagedAsync(int page, int pageSize, int price);

        Task<Course?> GetByIdAsync(int id);

        Task<Course> CreateAsync(Course course);

        Task UpdateAsync(Course course);

        Task<bool> DeleteAsync(int id);
        Task<int> GetCoursesCount();
    }

}
