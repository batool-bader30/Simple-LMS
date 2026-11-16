using System;
using System.Collections.Generic;
using System.Text;

namespace dalil.Core.DTOs
{
    public class EnrollmentDto
    {
        public record CreateEnrollmentDto(int StudentId, int CourseId);
        public record UpdateEnrollmentStatusDto(int StudentId, int CourseId, string Status);

        public record EnrollmentResponseDto(
    int Id,
    int StudentId,
    string StudentName,
    int CourseId,
    string Status,
    string CourseTitle,
    DateTime EnrolledAt);
    }
}
