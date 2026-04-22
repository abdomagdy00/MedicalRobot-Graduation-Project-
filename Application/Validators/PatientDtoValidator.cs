using Application.DTOs;
using FluentValidation;

namespace Application.Validators
{
    public class PatientDtoValidator : AbstractValidator<PatientDto>
    {
        public PatientDtoValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("The name cannot be empty")
                                    .MaximumLength(100);

            RuleFor(x => x.Age).InclusiveBetween(1, 120).WithMessage("Age should be reasonable.");
        }
    }
}