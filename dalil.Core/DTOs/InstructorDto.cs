using System;
using System.Collections.Generic;
using System.Text;

namespace dalil.Core.DTOs
{
    public class InstructorDto
    {
        public record CreateInstructorDto(string FullName);
        public record UpdateInstructorDto(string FullName);
        public record InstructorResponseDto (
        int Id,
        string FullName
        
            );

    }
}
