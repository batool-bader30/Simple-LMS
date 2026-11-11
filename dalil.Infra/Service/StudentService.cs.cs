using dalil.Core.DTOs;
using dalil.Core.Entities;
using dalil.Core.Entities.dalil.Core.Entities;
using dalil.Core.Mappings;
using dalil.Core.Repository;
using dalil.Core.Service;
using dalil.Infra.Data;
using dalil.Infra.Repository;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using static dalil.Core.DTOs.CourseDto;
using static dalil.Core.DTOs.StudentDto;
using static dalil.Core.Exceptions.AppExceptions;

namespace dalil.Infra.Service
{
    public class StudentService(
     IStudentRepository repo,
     IValidator<CreateStudentDto> createValidator,
     IValidator<UpdateStudentDto> updateValidator)
     : IStudentService
    {
        public async Task<StudentResponseDto?> CreateAsync(CreateStudentDto dto)
        {
            await createValidator.ValidateAndThrowAsync(dto);
            var student = dto.ToEntity();
            var created= await repo.CreateAsync(student);
            return created?.ToDto();
        }

        public async Task DeleteAsync(int id)
        {
            if (!await repo.DeleteAsync(id))
                throw new NotFoundException($"Student {id} was not found.");     //  404
        }

        public async Task<List<StudentResponseDto>> GetAllAsync()
        {
           var student = await repo.GetAllAsync();
            return student.Select(c => c.ToDto()).ToList();
        }


        public async Task<StudentResponseDto?> GetByIdAsync(int id)
        {
            var student= await repo.GetByIdAsync(id)
               ?? throw new NotFoundException($"Student {id} was not found.");  // 404
            return student?.ToDto();
        }

       

        public async Task UpdateAsync(int id, UpdateStudentDto dto)
        {
            await updateValidator.ValidateAndThrowAsync(dto);                    //  400

            var student = await repo.GetByIdAsync(id)
                ?? throw new NotFoundException($"Student {id} was not found.");  //  404

            student.ToEntity(dto);

            await repo.UpdateAsync(student);                       // duplicate email → 409
        }
        public async Task<List<CourseResponseDto>> GetCoursesAsync(int id)
        {
            var student = await repo.GetWithCoursesAsync(id)
                ?? throw new NotFoundException($"Student {id} was not found.");  //  404

            return student.Enrollments
                .Where(e => e.Course is not null)
                .Select(e => e.Course!.ToDto())
                .ToList();
        }

        public async Task<List<CourseResponseDto?>> GetByIdWithCourses(int id)
        {
            var student = await repo.GetByIdWithCourses(id)
              ?? throw new NotFoundException($"Student {id} was not found.");  // 404
            return student?.Enrollments
        .Select(e => e.Course!)
        .Select(c => c.ToDto())
        .ToList();
        }

        public async Task<PagedResult<StudentResponseDto>> GetPagedAsync(int page, int pageSize)
        {
            return await repo.GetPagedAsync(page,pageSize);
        }

        
    }
}
