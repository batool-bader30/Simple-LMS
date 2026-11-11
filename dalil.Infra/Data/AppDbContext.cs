using dalil.Core.Entities;
using dalil.Core.Entities.dalil.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
namespace dalil.Infra.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options)
: DbContext(options)
    {
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Instructor> Instructors => Set<Instructor>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();
        public DbSet<User> Users => Set<User>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.Entity<User>(e => 
            {
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.Email).HasMaxLength(256);
                e.Property(u => u.Role).HasMaxLength(20);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSeeding((context, _) =>
            {
                if (!context.Set<Instructor>().Any())
                {
                    var ahmad = new Instructor { FullName = "Ahmad Saleh" };
                    var lina = new Instructor { FullName = "Lina Haddad" };
                    context.Set<Course>().AddRange(
                    new Course { Title = "C# Fundamentals", Price = 50, Instructor = ahmad },
                    new Course { Title = "ASP.NET Core Web API", Price = 80, Instructor = ahmad },
                    new Course { Title = "PostgreSQL Essentials", Price = 60, Instructor = lina });
                    context.SaveChanges();
                }

                //if (!context.Set<Student>().Any())
                //{
                //    context.Set<Student>().AddRange(
                //        new Student { Name = "Ahmad Ali", Email = "ahmad@example.com" },
                //        new Student { Name = "Sara Mohammed", Email = "sara@example.com" }
                //    );
                //    context.SaveChanges();
                //}
                if (!context.Set<User>().Any())
                {
                    context.Set<User>().Add(new User
                    {
                        Email = "admin@dalil.jo",
                        Role = Roles.Admin,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123") // wrapped in a class later
                    });
                    context.SaveChanges();
                }

            });
        }
    }
}
