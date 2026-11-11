using System;
using System.Collections.Generic;
using System.Text;

namespace dalil.Core.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int InstructorId { get; set; }
        public int Level { get; set; }
        public string? ImageUrl { get; set; }

        public Instructor? Instructor { get; set; }
        public List<Enrollment> Enrollments { get; set; } = [];
    }
}
