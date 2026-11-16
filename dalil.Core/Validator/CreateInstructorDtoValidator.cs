using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using static dalil.Core.DTOs.InstructorDto;

namespace dalil.Core.Validator
{
    public class CreateInstructorDtoValidator:AbstractValidator<CreateInstructorDto>
    {
        public CreateInstructorDtoValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("FullName is required");
        }
    }
}
