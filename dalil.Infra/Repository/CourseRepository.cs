using dalil.Core.DTOs;
using dalil.Core.Entities;
using dalil.Core.Repository;
using dalil.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using static dalil.Core.DTOs.CourseDto;
namespace dalil.Infra.Repository
{
    public class CourseRepository(AppDbContext db) : ICourseRepository
    {
        public async Task<PagedResult<CourseResponseDto>> GetPagedAsync(int page, int pageSize ,int price)
        {
            var query = db.Courses.AsNoTracking();
            var total = await query.CountAsync(); // query 1: the total
            var items = await query
                .Where(c => c.Price <= price)
            .OrderBy(c => c.Id) // ALWAYS order before Skip
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CourseResponseDto(c.Id, c.Title, c.Price, c.Instructor!.FullName, c.ImageUrl))
            .ToListAsync(); // query 2: one page
            return new PagedResult<CourseResponseDto>(items, page, pageSize, total);
        }
        public Task<int> GetCoursesCount() => db.Courses.CountAsync();
        public async Task<Course?> GetByIdAsync(int id) => await db.Courses.FindAsync(id);
        public async Task<Course> CreateAsync(Course course)
        {
            try
            {
                db.Courses.Add(course);
                await db.SaveChangesAsync();
                return course;
            }
            catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                throw new ArgumentException("A course with this title already exists.");
            }
        }
        public async Task UpdateAsync(Course course)
        {
            db.Courses.Update(course); 
            await db.SaveChangesAsync(); 
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var course = await db.Courses.FindAsync(id);
            if (course is null) return false;
            db.Courses.Remove(course); 
            await db.SaveChangesAsync(); 
            return true;
        }

    }
}
