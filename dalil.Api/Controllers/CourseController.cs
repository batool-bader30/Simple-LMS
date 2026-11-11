using dalil.Core.Entities;
using dalil.Core.Service;
using dalil.Core.Storage;
using dalil.Infra.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static dalil.Core.DTOs.CourseDto;
using static System.Net.Mime.MediaTypeNames;

namespace dalil.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Admin)]
    public class CoursesController(ICourseService courseService ,IImageService images) : ControllerBase
    {
        [HttpGet]
       
        public async Task<ActionResult<List<CourseResponseDto>>> getpaged([FromQuery] int page = 1, [FromQuery] int pageSize = 1, [FromQuery] int maxprice = 50)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 50);
            return Ok(await courseService.GetPagedAsync(page, pageSize,maxprice));
        }

        [HttpPost("upload-image")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult> UploadImage(IFormFile file)
        {
            await using var stream = file.OpenReadStream();
            var url = await images.SaveCourseImageAsync(stream, file.FileName, file.Length);
            return Ok(new { imageUrl = url });
        }

        [HttpGet("/CourseCount")]
        public async Task<ActionResult<int>> GetCount()
        => Ok(await courseService.GetCoursesCount());


        [HttpGet("{id:int}")]
        public async Task<ActionResult<CourseResponseDto>> GetById( int id)
        => Ok(await courseService.GetByIdAsync(id));


        [HttpPost]
        public async Task<ActionResult<CourseResponseDto>> Create(CreateCourseDto dto)
        {
            var created = await courseService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), 
                new { id = created.Id }, created);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateCourseDto dto)
        { 
            await courseService.UpdateAsync(id, dto);
            return NoContent(); 
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        { 
            await courseService.DeleteAsync(id); 
            return NoContent();
        }
    }
}
