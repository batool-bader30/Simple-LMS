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
    public class InstructorRepository (AppDbContext db): IInstructorRepository
    {
        public async Task<Instructor> CreateAsync(Instructor instructor)
        {
            try
            {
                db.Instructors.Add(instructor);
                await db.SaveChangesAsync();
                return instructor;
            }
            catch (DbUpdateException ex)
                when (ex.InnerException is PostgresException
                {
                    SqlState: PostgresErrorCodes.UniqueViolation
                })
            {
                throw new ConflictException(
                    "An instructor with this Name already exists.");
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var instructor = await db.Instructors.FindAsync(id);

            if (instructor is null)
                return false;

            db.Instructors.Remove(instructor);
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<List<Instructor>> GetAllAsync()
        {
            return await db.Instructors.ToListAsync();
        }

        public Task<Instructor?> GetByIdAsync(int id)
        {
            return db.Instructors.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Instructor?> GetWithCoursesAsync(int id)
        {
            return await db.Instructors
                 .Include(i => i.Courses)
                 .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task UpdateAsync(Instructor instructor)
        {
            try
            {
                db.Instructors.Update(instructor);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
                when (ex.InnerException is PostgresException
                {
                    SqlState: PostgresErrorCodes.UniqueViolation
                })
            {
                throw new ConflictException(
                    "An instructor with this Name already exists.");
            }
        }
    }
}
