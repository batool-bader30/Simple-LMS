using dalil.Core.Entities;
using dalil.Core.Entities.dalil.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static dalil.Core.DTOs.InstructorDto;
using static dalil.Core.DTOs.StudentDto;

namespace dalil.Core.Mappings
{
    public static class InstructorMappings
    {
        public static InstructorResponseDto ToDto(this Instructor instructor) => new(
           instructor.Id,
           instructor.FullName
     );
        public static Instructor ToEntity(this CreateInstructorDto dto) => new()
        {
            FullName = dto.FullName,
        };
        public static void ToEntity(
           this Instructor instructor,
           UpdateInstructorDto dto)
        {
            instructor.FullName = dto.FullName;
        }
       
       
    }
}
