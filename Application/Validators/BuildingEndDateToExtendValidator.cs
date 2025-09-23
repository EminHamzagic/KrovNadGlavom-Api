using FluentValidation;
using krov_nad_glavom_api.Data.DTO.Building;

namespace krov_nad_glavom_api.Application.Validators
{
    public class BuildingEndDateToExtendValidator : AbstractValidator<BuildingEndDateToExtendDto>
    {
        public BuildingEndDateToExtendValidator()
        {
            RuleFor(x => x.ExtendedUntil)
                .GreaterThan(DateTime.Now).WithMessage("Novi datum završetka mora biti u budućnosti.");
        }
    }
}
