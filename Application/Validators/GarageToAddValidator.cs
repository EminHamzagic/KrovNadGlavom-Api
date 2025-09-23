using FluentValidation;
using krov_nad_glavom_api.Data.DTO.Garage;

namespace krov_nad_glavom_api.Application.Validators
{
    public class GarageToAddValidator : AbstractValidator<GarageToAddDto>
    {
        public GarageToAddValidator()
        {
            RuleFor(x => x.BuildingId)
                .NotEmpty().WithMessage("BuildingId je obavezan.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("BuildingId mora biti validan GUID.");

            RuleFor(x => x.SpotNumber)
                .NotEmpty().WithMessage("Broj parking mesta je obavezan.")
                .MaximumLength(10).WithMessage("Broj parking mesta ne sme imati više od 10 karaktera.");

            RuleFor(x => x.IsAvailable)
                .NotNull().WithMessage("IsAvailable je obavezno polje.");
        }
    }
}
