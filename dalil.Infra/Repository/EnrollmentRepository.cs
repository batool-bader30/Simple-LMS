using dalil.Core.Entities;
using dalil.Core.Entities.dalil.Core.Entities;
using dalil.Core.Repository;
using dalil.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using static dalil.Core.Exceptions.AppExceptions;

namespace dalil.Infra.Repository
{

    public class EnrollmentRepository(AppDbContext db)
        : IEnrollmentRepository
    {
        public async Task<Enrollment> CreateAsync(
            Enrollment enrollment)
        {
            try
            {
                db.Enrollments.Add(enrollment);
                await db.SaveChangesAsync();

                return enrollment;
            }
            catch (DbUpdateException ex)
                when (ex.InnerException is PostgresException
                {
                    SqlState: PostgresErrorCodes.UniqueViolation
                })
            {
                // unique index (StudentId, CourseId) → 409
                throw new ConflictException(
                    "This student is already enrolled in this course.");
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var enrollment = await db.Enrollments.FindAsync(id);

            if (enrollment is null)
                return false;
            db.Enrollments.Remove(enrollment);
                await db.SaveChangesAsync();
                return true;
            
        }

        public async Task<List<Enrollment?>> GetAllStudentsEnrolledAsync()
        {
            return await db.Enrollments
                .Where(e => e.Status == EnrollmentStatus.Paid)
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToListAsync();
        }



        public async Task<List<Enrollment?>> GetStudentsByCourseIdAsync (int courseId)
        {
           
            return await db.Enrollments
                .Where(e => e.CourseId == courseId && e.Status == EnrollmentStatus.Paid)
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToListAsync();
        }

        public async Task UpdateAsync(int id,Enrollment enrollment)
        {
             try
            {

                db.Enrollments.Update(enrollment);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
                when (ex.InnerException is PostgresException
                {
                    SqlState: PostgresErrorCodes.UniqueViolation
                })
            {
                throw new ConflictException(
                    "A student with this email already exists.");
            }

        }
        public async Task<Enrollment?> GetByIdAsync(int id)
        {
            return await db.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == id);
        }


    }

}
