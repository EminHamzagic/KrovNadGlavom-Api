using FluentValidation;
using krov_nad_glavom_api.Data.DTO.Installment;

namespace krov_nad_glavom_api.Application.Validators
{
    public class InstallmentToAddValidator : AbstractValidator<InstallmentToAddDto>
    {
        public InstallmentToAddValidator()
        {
            RuleFor(x => x.ContractId)
                .NotEmpty().WithMessage("ContractId je obavezan.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("ContractId mora biti validan GUID.");

            RuleFor(x => x.SequenceNumber)
                .GreaterThan(0).WithMessage("Redni broj rate mora biti veći od 0.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Iznos rate mora biti veći od 0.");

            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.MinValue).WithMessage("Datum dospeća mora biti validan datum.");

            RuleFor(x => x.IsConfirmed)
                .NotNull().WithMessage("IsConfirmed je obavezno polje.");
        }
    }
}
