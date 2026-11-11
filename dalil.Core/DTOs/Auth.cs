using System;
using System.Collections.Generic;
using System.Text;

namespace dalil.Core.DTOs
{
    public record Auth
    {
        public record LoginRequestDto(string Email, string Password);
        public record LoginResponseDto(string Token, DateTime ExpiresAt);
    }
}
