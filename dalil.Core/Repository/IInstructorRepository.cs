using dalil.Core.DTOs;
using dalil.Core.Entities;
using dalil.Core.Entities.dalil.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace dalil.Core.Repository
{
    public interface IInstructorRepository
    {
        Task<List<Instructor>> GetAllAsync();
    
        Task<Instructor> CreateAsync(Instructor instructor);

        Task UpdateAsync(Instructor instructor);

        Task<bool> DeleteAsync(int id);

        Task<Instructor?> GetWithCoursesAsync(int id);
        Task<Instructor?> GetByIdAsync(int id);


    }
}
