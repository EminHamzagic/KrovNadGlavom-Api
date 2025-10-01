using FluentValidation;
using krov_nad_glavom_api.Data.DTO.PriceList;

namespace krov_nad_glavom_api.Application.Validators
{
    public class PriceListToAddValidator : AbstractValidator<PriceListToAddDto>
    {
        public PriceListToAddValidator()
        {
            RuleFor(x => x.PricePerM2)
                .GreaterThan(0).WithMessage("Cena po m² mora biti veća od 0.");

            RuleFor(x => x.PenaltyPerM2)
                .GreaterThanOrEqualTo(0).WithMessage("Kazna po m² mora biti 0 ili veća.");

            RuleFor(x => x.GaragePrice)
                .GreaterThanOrEqualTo(0).WithMessage("Cena garažnog mesta mora biti veća od 0.");
        }
    }
}
