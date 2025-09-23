using FluentValidation;
using krov_nad_glavom_api.Data.DTO.UserAgencyFollow;

namespace krov_nad_glavom_api.Application.Validators
{
    public class UserAgencyFollowToAddValidator : AbstractValidator<UserAgencyFollowToAddDto>
    {
        public UserAgencyFollowToAddValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId je obavezan.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("UserId mora biti validan GUID.");

            RuleFor(x => x.AgencyId)
                .NotEmpty().WithMessage("AgencyId je obavezan.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("AgencyId mora biti validan GUID.");
        }
    }
}
