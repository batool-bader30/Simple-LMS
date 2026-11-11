using dalil.Core.Entities;
using dalil.Core.Entities.dalil.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static dalil.Core.DTOs.StudentDto;

namespace dalil.Core.Mappings
{
    public static class StudentMappings
    {
        public static StudentResponseDto ToDto(this Student student) => new(
            student.Id,
            student.Name,
            student.Email
      );
        public static Student ToEntity(this CreateStudentDto dto) => new()
        {
            Name = dto.Name,
            Email = dto.Email,
        };
        public static void ToEntity(
           this Student student,
           UpdateStudentDto dto)
        {
            student.Name = dto.Name;
            student.Email = dto.Email;
        }
    }
}
