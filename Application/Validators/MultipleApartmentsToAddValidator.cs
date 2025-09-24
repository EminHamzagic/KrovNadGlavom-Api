using FluentValidation;
using krov_nad_glavom_api.Data.DTO.Apartment;

namespace krov_nad_glavom_api.Application.Validators
{
    public class MultipleApartmentsToAddValidator : AbstractValidator<MultipleApartmentsToAddDto>
    {
        public MultipleApartmentsToAddValidator()
        {
            RuleForEach(x => x.Apartments)
                .SetValidator(new ApartmentToAddValidator());
        }
    }
}