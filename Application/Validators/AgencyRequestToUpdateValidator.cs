using FluentValidation;
using krov_nad_glavom_api.Data.DTO.AgencyRequest;

namespace krov_nad_glavom_api.Application.Validators
{
    public class AgencyRequestToUpdateValidator : AbstractValidator<AgencyRequestToUpdateDto>
    {
        public AgencyRequestToUpdateValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status je obavezan.")
                .Must(s => new[] { "Pending", "Approved", "Rejected" }.Contains(s))
                .WithMessage("Status može biti samo Pending, Approved ili Rejected.");

            RuleFor(x => x.RejectionReason)
                .MaximumLength(500).WithMessage("Razlog odbijanja ne sme imati više od 500 karaktera.")
                .When(x => x.Status == "Rejected");
        }
    }
}
