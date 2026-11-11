using dalil.Core.DTOs;
using dalil.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static dalil.Core.DTOs.EnrollmentDto;

namespace dalil.Core.Mappings
{
    public static class EnrollmentMappings
    {
        public static EnrollmentResponseDto ToDto(this Enrollment enrollment) => new(
            enrollment.Id,
            enrollment.StudentId,
            enrollment.Student?.Name ?? string.Empty,
            enrollment.CourseId,
            enrollment.Course?.Title ?? string.Empty,
            enrollment.EnrolledAt);
    }

}
