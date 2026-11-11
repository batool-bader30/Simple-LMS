using dalil.Core.Service;
using dalil.Infra.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static dalil.Core.DTOs.EnrollmentDto;

namespace dalil.Api.Controllers
{
   
        [Authorize(Roles = "Student")]
        [ApiController]
        [Route("api/[controller]")]
        public class EnrollmentsController(IEnrollmentService enrollmentService, IStudentService studentService)
       : ControllerBase
        {
            // 201 Created · 404 student/course not found · 409 already enrolled
            [HttpPost]
            public async Task<ActionResult<EnrollmentResponseDto>> Enroll(CreateEnrollmentDto dto)
            {
                var enrollment = await enrollmentService.EnrollAsync(dto.StudentId, dto.CourseId);

                return StatusCode(StatusCodes.Status201Created, enrollment);
            }

            [HttpGet("me")]
            public async Task<IActionResult> GetMyCourses()
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                var courses = await studentService.GetByIdWithCourses(userId); 
                return Ok(courses);
            }
        }

    }

