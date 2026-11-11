using System;
using System.Collections.Generic;
using System.Text;
using static dalil.Core.DTOs.EnrollmentDto;

namespace dalil.Core.Service
{
    public interface IEnrollmentService
    {
        Task<EnrollmentResponseDto> EnrollAsync(
            int studentId, int courseId);
    }
}
