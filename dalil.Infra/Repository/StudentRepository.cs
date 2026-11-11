using dalil.Core.DTOs;
using dalil.Core.Entities;
using dalil.Core.Entities.dalil.Core.Entities;
using dalil.Core.Repository;
using dalil.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using static dalil.Core.DTOs.CourseDto;
using static dalil.Core.DTOs.StudentDto;
using static dalil.Core.Exceptions.AppExceptions;

namespace dalil.Infra.Repository
{
    public class StudentRepository(AppDbContext db)
     : IStudentRepository
    {
        public async Task<List<Student>> GetAllAsync()
        {
            return await db.Students.ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await db.Students.FindAsync(id);
        }

        // Lecture 5 bonus: Include + ThenInclude
        public async Task<Student?> GetWithCoursesAsync(int id)
        {
            return await db.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                        .ThenInclude(c => c!.Instructor)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Student> CreateAsync(Student student)
        {
            try
            {
                db.Students.Add(student);
                await db.SaveChangesAsync();

                return student;
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

        public async Task UpdateAsync(Student student)
        {
            try
            {
                db.Students.Update(student);
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

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await db.Students.FindAsync(id);

            if (student is null)
                return false;

            db.Students.Remove(student);
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<Student?> GetByIdWithCourses(int id)
        {
            return await db.Students
               .Include(s => s.Enrollments)
                   .ThenInclude(e => e.Course)
               .FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<PagedResult<StudentResponseDto>> GetPagedAsync(int page, int pageSize)
        {

            var total = await db.Students.CountAsync();
            var items = await db.Students
                .AsNoTracking()
            .OrderBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new StudentResponseDto(c.Id, c.Name,c.Email))
            .ToListAsync();
            return new PagedResult<StudentResponseDto>(items, page, pageSize, total);
        }
    }

}
