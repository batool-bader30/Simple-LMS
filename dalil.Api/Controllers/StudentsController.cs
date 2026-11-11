using dalil.Core.DTOs;
using dalil.Core.Entities;
using dalil.Core.Entities.dalil.Core.Entities;
using dalil.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static dalil.Core.DTOs.CourseDto;
using static dalil.Core.DTOs.StudentDto;

namespace dalil.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController(IStudentService studentService)
     : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<StudentResponseDto>>> GetAll()
            => Ok(await studentService.GetAllAsync());

        [HttpGet("/GetPaged")]
        public async Task<ActionResult<PagedResult<StudentResponseDto>>> GetPaged([FromQuery]int page=1, [FromQuery] int pageSize=1)
        {
            page=Math.Max(page, 1);
            pageSize=Math.Clamp(pageSize, 1,50);
          return  Ok(await studentService.GetPagedAsync(page ,pageSize));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<StudentResponseDto>> GetById(int id)
            => Ok(await studentService.GetByIdAsync(id));

       
        [HttpPost]
        public async Task<ActionResult<StudentResponseDto>> Create(CreateStudentDto dto)
        {
            var created = await studentService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateStudentDto dto)
        {
            await studentService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await studentService.DeleteAsync(id);
            return NoContent();
        }

       // Lecture 5 bonus: all courses a student is enrolled in (Include + ThenInclude)
        [HttpGet("{id:int}/courses")]
        public async Task<ActionResult<List<CourseResponseDto>>> GetCourses(int id)
            => Ok(await studentService.GetByIdWithCourses(id));
    }
}