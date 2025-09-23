using FluentValidation;
using krov_nad_glavom_api.Data.DTO.Contract;

namespace krov_nad_glavom_api.Application.Validators
{
    public class ContractToAddValidator : AbstractValidator<ContractToAddDto>
    {
        public ContractToAddValidator()
        {
            // Obavezna polja ID-jeva
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId je obavezan.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("UserId mora biti validan GUID.");

            RuleFor(x => x.AgencyId)
                .NotEmpty().WithMessage("AgencyId je obavezan.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("AgencyId mora biti validan GUID.");

            RuleFor(x => x.ApartmentId)
                .NotEmpty().WithMessage("ApartmentId je obavezan.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("ApartmentId mora biti validan GUID.");

            // Cena i rate
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Cena mora biti veća od 0.");

            RuleFor(x => x.InstallmentAmount)
                .GreaterThan(0).WithMessage("Iznos rate mora biti veći od 0.");

            RuleFor(x => x.InstallmentCount)
                .GreaterThan(0).WithMessage("Broj rata mora biti veći od 0.")
                .LessThanOrEqualTo(120).WithMessage("Broj rata ne sme biti veći od 120."); // max 10 godina

            
        }
    }
}
