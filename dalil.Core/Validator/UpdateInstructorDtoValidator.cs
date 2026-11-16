using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using static dalil.Core.DTOs.InstructorDto;

namespace dalil.Core.Validator
{
    public class UpdateInstructorDtoValidator: AbstractValidator<UpdateInstructorDto> 
    {
        public UpdateInstructorDtoValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("FullName is required");
        }
    }
}
