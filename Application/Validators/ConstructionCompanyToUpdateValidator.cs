using FluentValidation;
using krov_nad_glavom_api.Data.DTO.ConstructionCompany;

namespace krov_nad_glavom_api.Application.Validators
{
    public class ConstructionCompanyToUpdateValidator : AbstractValidator<ConstructionCompanyToUpdateDto>
    {
        public ConstructionCompanyToUpdateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Naziv firme je obavezan.")
                .MaximumLength(100).WithMessage("Naziv firme ne sme imati više od 100 karaktera.");

            RuleFor(x => x.PIB)
                .NotEmpty().WithMessage("PIB je obavezan.")
                .Length(9).WithMessage("PIB mora imati tačno 9 cifara.")
                .Matches(@"^\d+$").WithMessage("PIB može sadržati samo cifre.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adresa je obavezna.")
                .MaximumLength(200).WithMessage("Adresa ne sme imati više od 200 karaktera.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email je obavezan.")
                .EmailAddress().WithMessage("Email nije validnog formata.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon je obavezan.")
                .Matches(@"^\+?\d{7,15}$").WithMessage("Telefon mora biti u validnom formatu (7–15 cifara, opcionalno sa +).");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Grad je obavezan.")
                .MaximumLength(100).WithMessage("Grad ne sme imati više od 100 karaktera.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Opis ne sme imati više od 1000 karaktera.");
        }
    }
}
