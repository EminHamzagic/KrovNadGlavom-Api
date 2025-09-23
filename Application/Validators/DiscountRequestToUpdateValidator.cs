using FluentValidation;
using krov_nad_glavom_api.Data.DTO.DiscountRequest;

namespace krov_nad_glavom_api.Application.Validators
{
    public class DiscountRequestToUpdateValidator : AbstractValidator<DiscountRequestToUpdateDto>
    {
        public DiscountRequestToUpdateValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status je obavezan.")
                .Must(status => status == "Pending" || status == "Approved" || status == "Rejected")
                .WithMessage("Status mora biti 'Pending', 'Approved' ili 'Rejected'.");

            RuleFor(x => x.Reason)
                .MaximumLength(500).WithMessage("Razlog ne sme imati više od 500 karaktera.");
        }
    }
}
