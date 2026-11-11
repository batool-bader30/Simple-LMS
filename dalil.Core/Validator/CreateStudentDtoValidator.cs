using FluentValidation;
using static dalil.Core.DTOs.StudentDto;

namespace DalilAcademy.Core.Validators;

// Lab 6: required name, valid email, max lengths (match StudentConfiguration)
public class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
{
    public CreateStudentDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(254);
    }
}
