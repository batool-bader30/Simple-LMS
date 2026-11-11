using System;
using System.Collections.Generic;
using System.Text;

namespace dalil.Core.Entities
{
    namespace dalil.Core.Entities
    {
        public class Student
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public int UserId { get; set; }
            public User User { get; set; }

            public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        }
    }
}
