using System;
using System.Collections.Generic;
using System.Text;
using static dalil.Core.DTOs.Auth;
using static dalil.Core.DTOs.StudentDto;

namespace dalil.Core.Service
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
        Task<LoginResponseDto> RegisterAsync(RegisterDto dto);
    }
}
