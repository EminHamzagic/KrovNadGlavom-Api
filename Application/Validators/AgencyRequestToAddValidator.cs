using FluentValidation;
using krov_nad_glavom_api.Data.DTO.AgencyRequest;

namespace krov_nad_glavom_api.Application.Validators
{
    public class AgencyRequestToAddValidator : AbstractValidator<AgencyRequestToAddDto>
    {
        public AgencyRequestToAddValidator()
        {
            RuleFor(x => x.AgencyId)
                .NotEmpty().WithMessage("AgencyId je obavezan.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("AgencyId mora biti validan GUID.");

            RuleFor(x => x.BuildingId)
                .NotEmpty().WithMessage("BuildingId je obavezan.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("BuildingId mora biti validan GUID.");

            RuleFor(x => x.CommissionPercentage)
                .InclusiveBetween(0, 100).WithMessage("Procenat provizije mora biti između 0 i 100.");

           
        }
    }
}
