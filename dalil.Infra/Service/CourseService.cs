using dalil.Core.DTOs;
using dalil.Core.Mappings;
using dalil.Core.Repository;
using dalil.Core.Service;
using FluentValidation;
using static dalil.Core.DTOs.CourseDto;
using static dalil.Core.Exceptions.AppExceptions;

namespace DalilAcademy.Infra.Service;

public class CourseService(
    ICourseRepository courseRepository,
    IValidator<CreateCourseDto> createValidator,
    IValidator<UpdateCourseDto> updateValidator)
    : ICourseService
{
    public async Task<PagedResult<CourseResponseDto>> GetPagedAsync(int page, int pageSize,int price)
    {
     return await courseRepository.GetPagedAsync( page,  pageSize,price);
        
    }

    public async Task<CourseResponseDto> GetByIdAsync(int id)
    {
        var course = await courseRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Course {id} was not found.");   // → 404

        return course.ToDto();
    }

    public async Task<CourseResponseDto> CreateAsync(CreateCourseDto dto)
    {
        await createValidator.ValidateAndThrowAsync(dto);                    // → 400

        var course = dto.ToEntity();
        var created = await courseRepository.CreateAsync(course);           // duplicate title → 409

        return created.ToDto();
    }

    public async Task UpdateAsync(int id, UpdateCourseDto dto)
    {
        await updateValidator.ValidateAndThrowAsync(dto);                    // → 400

        var course = await courseRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Course {id} was not found.");   // → 404

        course.ToEntity(dto);
        course.ImageUrl = dto.ImageUrl;

        await courseRepository.UpdateAsync(course);                         // duplicate title → 409
    }

    public async Task DeleteAsync(int id)
    {
        if (!await courseRepository.DeleteAsync(id))
            throw new NotFoundException($"Course {id} was not found.");      // → 404
    }
    public Task<int> GetCoursesCount()
    {
        return courseRepository.GetCoursesCount();
    }
}
