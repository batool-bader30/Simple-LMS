using dalil.Core.Entities;
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
    }

}
