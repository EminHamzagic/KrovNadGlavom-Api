using FluentValidation;
using krov_nad_glavom_api.Data.DTO.User;

namespace krov_nad_glavom_api.Application.Validators
{
    public class UserToAddValidator : AbstractValidator<UserToAddDto>
    {
        public UserToAddValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ime je obavezno.")
                .MaximumLength(50).WithMessage("Ime ne sme imati više od 50 karaktera.");

            RuleFor(x => x.Lastname)
                .NotEmpty().WithMessage("Prezime je obavezno.")
                .MaximumLength(50).WithMessage("Prezime ne sme imati više od 50 karaktera.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Korisničko ime je obavezno.")
                .MaximumLength(50).WithMessage("Korisničko ime ne sme imati više od 50 karaktera.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email je obavezan.")
                .EmailAddress().WithMessage("Email nije validan.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Lozinka je obavezna.")
                .MinimumLength(6).WithMessage("Lozinka mora imati najmanje 6 karaktera.")
                .MaximumLength(100).WithMessage("Lozinka ne sme imati više od 100 karaktera.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Uloga je obavezna.")
                .Must(role => role == "User" || role == "Admin" || role == "Manager")
                .WithMessage("Uloga mora biti 'User', 'Admin' ili 'Manager'.");

            RuleFor(x => x.ConstructionCompanyId)
                .Must(id => string.IsNullOrEmpty(id) || Guid.TryParse(id, out _))
                .WithMessage("ConstructionCompanyId mora biti validan GUID.");

            RuleFor(x => x.AgencyId)
                .Must(id => string.IsNullOrEmpty(id) || Guid.TryParse(id, out _))
                .WithMessage("AgencyId mora biti validan GUID.");
        }
    }
}
