using dalil.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using static dalil.Core.DTOs.EnrollmentDto;
using static dalil.Core.DTOs.StudentDto;

namespace dalil.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController(IEnrollmentService enrollmentService)
        : ControllerBase
    {
        // Rule 1 & 2: POST /api/enrollments
        [HttpPost]
        public async Task<ActionResult<EnrollmentResponseDto>> Create(CreateEnrollmentDto dto)
        {
            var created = await enrollmentService.EnrollAsync(dto);

            return CreatedAtAction(
                nameof(Create),
                new { id = created.Id },
                created);
        }

        // Rule 4: GET /api/enrollments/me (إرجاع كورسات/تسجيلات الطالب المدفوعة Paid فقط)
        [HttpGet("me")]
        public async Task<ActionResult<List<EnrollmentResponseDto>>> GetMyPaidEnrollments()
            => Ok(await enrollmentService.GetAllStudentsEnrolledAsync());

        // Rule 3: PUT /api/enrollments/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id,CreateEnrollmentDto dto)
        {
            await enrollmentService.UpdateAsync(id, dto);
            return NoContent();
        }

        // Rule 3: DELETE /api/enrollments/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await enrollmentService.DeleteAsync(id);
            return NoContent();
        }
        [HttpGet("{courseId:int}/students")]
        public async Task<ActionResult<List<StudentResponseDto>>> GetStudentsByCourse(
    int courseId,
    [FromServices] IEnrollmentService enrollmentService)
        {
            var students = await enrollmentService.GetStudentsByCourseIdAsync(courseId);
            return Ok(students);
        }
    }
}