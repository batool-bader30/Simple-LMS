using dalil.Core.Entities;
using dalil.Core.Entities.dalil.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace dalil.Core.Repository
{

    public interface IEnrollmentRepository
    {
        Task<List<Enrollment?>> GetStudentsByCourseIdAsync(int courseId);
        Task<List<Enrollment?>> GetAllStudentsEnrolledAsync();
        Task<Enrollment> CreateAsync(Enrollment enrollment);
        Task UpdateAsync(int id,Enrollment enrollment);

        Task<bool> DeleteAsync(int id);
        Task<Enrollment?> GetByIdAsync(int id);

    }
}
