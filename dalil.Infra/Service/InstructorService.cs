using dalil.Core.DTOs;
using dalil.Core.Mappings;
using dalil.Core.Repository;
using dalil.Core.Service;
using FluentValidation;
using static dalil.Core.DTOs.InstructorDto;
using static dalil.Core.Exceptions.AppExceptions;

namespace dalil.Infra.Service
{
    public class InstructorService(IInstructorRepository repo ,
        IValidator<CreateInstructorDto> createValidator,
     IValidator<UpdateInstructorDto> updateValidator
        ) : IInstructorService
    {
        public async Task<InstructorResponseDto?> CreateAsync(CreateInstructorDto dto)
        {
            await createValidator.ValidateAndThrowAsync(dto);
            var instructor = dto.ToEntity();
            var created = await repo.CreateAsync(instructor);
            return created?.ToDto();
        }

        public async Task DeleteAsync(int id)
        {
            if (!await repo.DeleteAsync(id))
                throw new NotFoundException($"Instructor {id} was not found.");
        }

        public async Task<List<InstructorResponseDto>> GetAllAsync()
        {
            var instructor = await repo.GetAllAsync();
            return instructor.Select(c => c.ToDto()).ToList();
        }

        public async Task<InstructorResponseDto?> GetByIdAsync(int id)
        {
            var instructor = await repo.GetByIdAsync(id)
              ?? throw new NotFoundException($"Instructor {id} was not found.");  // 404
            return instructor?.ToDto();
        }

        public async Task<List<CourseDto.CourseResponseDto>> GetCoursesAsync(int id)
        {
            var instructor = await repo.GetWithCoursesAsync(id)
                 ?? throw new NotFoundException($"Instructor {id} was not found.");  //  404

            return instructor.Courses.Select(c => c.ToDto()).ToList();
                
        }

        public async Task UpdateAsync(int id, UpdateInstructorDto dto)
        {
            await updateValidator.ValidateAndThrowAsync(dto);
            var instructor = await repo.GetByIdAsync(id);
            if (instructor == null)
                throw new NotFoundException($"Instructor {id} was not found.");

            instructor.ToEntity(dto);
            await repo.UpdateAsync(instructor);
        }
    }
}
