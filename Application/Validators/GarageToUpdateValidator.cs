using FluentValidation;
using krov_nad_glavom_api.Data.DTO.Garage;

namespace krov_nad_glavom_api.Application.Validators
{
    public class GarageToUpdateValidator : AbstractValidator<GarageToUpdateDto>
    {
        public GarageToUpdateValidator()
        {
            RuleFor(x => x.SpotNumber)
                .NotEmpty().WithMessage("Broj parking mesta je obavezan.")
                .MaximumLength(10).WithMessage("Broj parking mesta ne sme imati više od 10 karaktera.");

            RuleFor(x => x.IsAvailable)
                .NotNull().WithMessage("IsAvailable je obavezno polje.");
        }
    }
}
