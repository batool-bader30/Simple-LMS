using dalil.Core.Entities.dalil.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace dalil.Core.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        public int CourseId { get; set; }
        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Pending;
        public Course? Course { get; set; }
        public DateTime EnrolledAt { get; set; }
    }
}
