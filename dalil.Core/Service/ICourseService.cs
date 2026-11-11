using dalil.Core.DTOs;
using dalil.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static dalil.Core.DTOs.CourseDto;

namespace dalil.Core.Service
{
    public interface ICourseService
    {
        Task<PagedResult<CourseResponseDto>> GetPagedAsync(int page, int pageSize, int price);
        Task<int> GetCoursesCount();

        Task<CourseResponseDto> GetByIdAsync(int id);

        Task<CourseResponseDto> CreateAsync(CreateCourseDto dto);

        Task UpdateAsync(int id, UpdateCourseDto dto);

        Task DeleteAsync(int id);
    }
}
