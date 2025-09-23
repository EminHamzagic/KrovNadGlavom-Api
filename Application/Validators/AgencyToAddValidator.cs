using FluentValidation;
using krov_nad_glavom_api.Data.DTO.Agency;

namespace krov_nad_glavom_api.Application.Validators
{
    public class AgencyToAddValidator : AbstractValidator<AgencyToAddDto>
    {
        public AgencyToAddValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ime agencije je obavezno.")
                .MaximumLength(100).WithMessage("Ime agencije ne sme imati više od 100 karaktera.");

            RuleFor(x => x.PIB)
                .NotEmpty().WithMessage("PIB je obavezan.")
                .Matches(@"^\d{9}$").WithMessage("PIB mora imati tačno 9 cifara.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adresa je obavezna.")
                .MaximumLength(200).WithMessage("Adresa ne sme imati više od 200 karaktera.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email je obavezan.")
                .EmailAddress().WithMessage("Email nije validan.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon je obavezan.")
                .Matches(@"^\+?\d{6,15}$").WithMessage("Telefon nije validan.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Grad je obavezan.")
                .MaximumLength(50).WithMessage("Grad ne sme imati više od 50 karaktera.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Opis ne sme imati više od 500 karaktera.");
        }
    }
}