using dalil.Core.DTOs;
using dalil.Core.Entities;
using dalil.Core.Mappings;
using dalil.Core.Repository;
using dalil.Core.Service;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static dalil.Core.DTOs.EnrollmentDto;
using static dalil.Core.DTOs.StudentDto;
using static dalil.Core.Exceptions.AppExceptions;

namespace DalilAcademy.Infra.Service
{
    public class EnrollmentService(
        IEnrollmentRepository enrollmentRepo,
        ICourseRepository courseRepo,
        IValidator<CreateEnrollmentDto> createValidator
        )
        : IEnrollmentService
    {
        public async Task<EnrollmentResponseDto> EnrollAsync(CreateEnrollmentDto dto)
        {
            // 1. Validation (400 Bad Request)
            await createValidator.ValidateAndThrowAsync(dto);

            var course = await courseRepo.GetByIdAsync(dto.CourseId)
                ?? throw new NotFoundException($"Course {dto.CourseId} was not found."); // 404

            // 3. تطبيق Rule 2: إذا كان سعر الكورس 0 تكون الحالة Paid مباشراً وإلا Pending
            var status = course.Price == 0 ? EnrollmentStatus.Paid : EnrollmentStatus.Pending;

            var enrollment = new Enrollment
            {
                StudentId = dto.StudentId,
                CourseId = dto.CourseId,
                Status = status
            };

            // 4. الحفظ في الداتا بيز (Duplicate Index → 409 Conflict يتم التعامل معه داخل الـ Repository)
            var created = await enrollmentRepo.CreateAsync(enrollment);

            return created.ToDto();
        }

        public async Task<List<StudentResponseDto>> GetStudentsByCourseIdAsync(int courseId)
        {
            // التأكد من وجود الكورس أولاً لإرجاع 404 إذا كان غير موجود
            var course = await courseRepo.GetByIdAsync(courseId)
                ?? throw new NotFoundException($"Course {courseId} was not found."); // 404

            var enrollments = await enrollmentRepo.GetStudentsByCourseIdAsync(courseId);

            return enrollments
                .Where(e => e?.Student is not null)
                .Select(e => e!.Student!.ToDto())
                .ToList();
        }

        public async Task<List<EnrollmentResponseDto>> GetAllStudentsEnrolledAsync()
        {
            // Rule 4: إرجاع التسجيلات ذات الحالة Paid فقط
            var enrollments = await enrollmentRepo.GetAllStudentsEnrolledAsync();

            return enrollments
                .Where(e => e is not null)
                .Select(e => e!.ToDto())
                .ToList();
        }

        public async Task UpdateAsync(int id, CreateEnrollmentDto dto)
        {
            // 1. Validation (400 Bad Request)

            var existingEnrollment = await enrollmentRepo.GetByIdAsync(id)
                ?? throw new NotFoundException($"Enrollment {id} was not found."); // 404

            if (existingEnrollment.Status == EnrollmentStatus.Paid)
                throw new ConflictException("Cannot update course for a paid enrollment."); // 409

            // 4. تحديث البيانات
            existingEnrollment.CourseId = dto.CourseId;
            existingEnrollment.StudentId = dto.StudentId;

            await enrollmentRepo.UpdateAsync(id, existingEnrollment);
        }

        public async Task DeleteAsync(int id)
        {
            // 1. جلب التسجيل للتأكد من شروط الحذف
            var existingEnrollment = await enrollmentRepo.GetByIdAsync(id)
                ?? throw new NotFoundException($"Enrollment {id} was not found."); // 404

            // 2. تطبيق Rule 3: يمنع حذف التسجيل إذا كانت حالته Paid
            if (existingEnrollment.Status == EnrollmentStatus.Paid)
                throw new ConflictException("Cannot delete a paid enrollment."); // 409

            // 3. الحذف من الداتا بيز
            await enrollmentRepo.DeleteAsync(id);
        }
    }
}