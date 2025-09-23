using FluentValidation;
using krov_nad_glavom_api.Data.DTO.Apartment;

namespace krov_nad_glavom_api.Application.Validators
{
    public class ApartmentToUpdateValidator : AbstractValidator<ApartmentToUpdateDto>
    {
        public ApartmentToUpdateValidator()
        {
            RuleFor(x => x.ApartmentNumber)
                .NotEmpty().WithMessage("Broj stana je obavezan.")
                .MaximumLength(20).WithMessage("Broj stana ne sme imati više od 20 karaktera.");

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
