using FluentValidation;
using krov_nad_glavom_api.Data.DTO.Installment;


namespace krov_nad_glavom_api.Application.Validators
{
    public class InstallmentProofToSendValidator : AbstractValidator<InstallmentProofToSendDto>
    {
        public InstallmentProofToSendValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id je obavezan.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("Id mora biti validan GUID.");

            RuleFor(x => x.File)
                .NotNull().WithMessage("Fajl je obavezan.")
                .Must(file => file.Length > 0).WithMessage("Fajl ne sme biti prazan.")
                .Must(file => file.Length <= 5 * 1024 * 1024) // max 5MB
                    .WithMessage("Fajl ne sme biti veći od 5MB.")
                .Must(file =>
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
                    return allowedExtensions.Any(ext =>
                        file.FileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
                })
                .WithMessage("Dozvoljeni formati fajla su: .jpg, .jpeg, .png, .pdf.");
        }
    }
}
