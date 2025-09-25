using FluentValidation;
using krov_nad_glavom_api.Data.DTO.DiscountRequest;

namespace krov_nad_glavom_api.Application.Validators
{
    public class DiscountRequestToAddValidator : AbstractValidator<DiscountRequestToAddDto>
    {
        public DiscountRequestToAddValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId je obavezan.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("UserId mora biti validan GUID.");

            RuleFor(x => x.AgencyId)
                .NotEmpty().WithMessage("AgencyId je obavezan.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("AgencyId mora biti validan GUID.");

            RuleFor(x => x.ApartmentId)
                .NotEmpty().WithMessage("ApartmentId je obavezan.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("ApartmentId mora biti validan GUID.");

            RuleFor(x => x.Percentage)
                .InclusiveBetween(1, 100).WithMessage("Procenat popusta mora biti između 1 i 100.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status je obavezan.")
                .Must(status => status == "Pending" || status == "Approved" || status == "Rejected")
                .WithMessage("Status mora biti 'Pending', 'Approved' ili 'Rejected'.");
        }
    }
}
