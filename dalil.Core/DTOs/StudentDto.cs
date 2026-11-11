using dalil.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static dalil.Core.DTOs.CourseDto;

namespace dalil.Core.DTOs
{
    public class StudentDto
    {
        
        public record RegisterDto(string Name, string Email, string Password);
        public record CreateStudentDto(string Name, string Email);
        public record UpdateStudentDto(string Name, string Email);
        public record StudentResponseDto(
        int Id,
        string Name,
        string Email
            
            );
        
    
}
}
