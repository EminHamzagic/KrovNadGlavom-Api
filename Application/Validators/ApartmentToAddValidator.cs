using FluentValidation;
using krov_nad_glavom_api.Data.DTO.Apartment;

namespace krov_nad_glavom_api.Application.Validators
{
    public class ApartmentToAddValidator : AbstractValidator<ApartmentToAddDto>
    {
        public ApartmentToAddValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id je obavezan.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("Id mora biti validan GUID.");

            RuleFor(x => x.BuildingId)
                .NotEmpty().WithMessage("BuildingId je obavezan.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("BuildingId mora biti validan GUID.");

            RuleFor(x => x.ApartmentNumber)
                .NotEmpty().WithMessage("Broj stana je obavezan.")
                .MaximumLength(15).WithMessage("Broj stana ne sme imati više od 15 karaktera.");

            RuleFor(x => x.Area)
                .GreaterThan(0).WithMessage("Površina mora biti veća od 0.");

            RuleFor(x => x.RoomCount)
                .GreaterThanOrEqualTo(0).WithMessage("Broj soba ne može biti negativan.");

            RuleFor(x => x.BalconyCount)
                .GreaterThanOrEqualTo(0).WithMessage("Broj terasa ne može biti negativan.");

            RuleFor(x => x.Orientation)
                .NotEmpty().WithMessage("Orijentacija je obavezna.")
                .MaximumLength(20).WithMessage("Orijentacija ne sme imati više od 20 karaktera.");

            RuleFor(x => x.Floor)
                .GreaterThanOrEqualTo(0).WithMessage("Sprat ne može biti negativan.");
        }
    }
}
