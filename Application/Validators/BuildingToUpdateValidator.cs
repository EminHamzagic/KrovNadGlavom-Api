using FluentValidation;
using krov_nad_glavom_api.Data.DTO.Building;

namespace krov_nad_glavom_api.Application.Validators
{
    public class BuildingToUpdateValidator : AbstractValidator<BuildingToUpdateDto>
    {
        public BuildingToUpdateValidator()
        {
            RuleFor(x => x.ParcelNumber)
                .NotEmpty().WithMessage("Broj parcele je obavezan.")
                .MaximumLength(50).WithMessage("Broj parcele ne sme imati više od 50 karaktera.");

            RuleFor(x => x.Area)
                .GreaterThan(0).WithMessage("Površina mora biti veća od 0.");

            RuleFor(x => x.FloorCount)
                .GreaterThan(0).WithMessage("Zgrada mora imati barem jedan sprat.");

            RuleFor(x => x.ElevatorCount)
                .GreaterThanOrEqualTo(0).WithMessage("Broj liftova ne može biti negativan.");

            RuleFor(x => x.GarageSpotCount)
                .GreaterThanOrEqualTo(0).WithMessage("Broj garažnih mesta ne može biti negativan.");

            RuleFor(x => x.StartDate)
                .LessThan(x => x.EndDate).WithMessage("Datum početka mora biti pre datuma završetka.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Grad je obavezan.")
                .MaximumLength(100).WithMessage("Grad ne sme imati više od 100 karaktera.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adresa je obavezna.")
                .MaximumLength(200).WithMessage("Adresa ne sme imati više od 200 karaktera.");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude mora biti između -180 i 180.");

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Latitude mora biti između -90 i 90.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Opis ne sme imati više od 1000 karaktera.");
        }
    }
}
