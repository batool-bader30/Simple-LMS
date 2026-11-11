using dalil.Core.DTOs;
using dalil.Core.Service;
using dalil.Infra.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static dalil.Core.DTOs.Auth;
using static dalil.Core.DTOs.StudentDto;

namespace dalil.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
        [Route("api/[controller]")]
        public class AuthController(IAuthService auth) : ControllerBase
        {
            [HttpPost("login")]
            [AllowAnonymous]
            public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto dto)
            => Ok(await auth.LoginAsync(dto));

        [HttpPost("register")]
        public async Task<ActionResult<LoginResponseDto>> Register([FromBody] RegisterDto dto)
        {
            var result = await auth.RegisterAsync(dto);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        


    }

}
