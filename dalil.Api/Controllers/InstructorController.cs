using dalil.Core.DTOs;
using dalil.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static dalil.Core.DTOs.CourseDto;
using static dalil.Core.DTOs.InstructorDto;
using static dalil.Core.DTOs.StudentDto;

namespace dalil.Api.Controllers
{
   // [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController(IInstructorService instructorService)
     : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<InstructorResponseDto>>> GetAll()
            => Ok(await instructorService.GetAllAsync());
    

        [HttpGet("{id:int}")]
        public async Task<ActionResult<InstructorResponseDto>> GetById(int id)
            => Ok(await instructorService.GetByIdAsync(id));


        [HttpPost]
        public async Task<ActionResult<InstructorResponseDto>> Create(CreateInstructorDto dto)
        {
            var created = await instructorService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateInstructorDto dto)
        {
            await instructorService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await instructorService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id:int}/courses")]
        public async Task<ActionResult<List<CourseResponseDto>>> GetCourses(int id)
            => Ok(await instructorService.GetCoursesAsync(id));
    }
}
