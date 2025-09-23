using FluentValidation;
using krov_nad_glavom_api.Data.DTO.Reservation;

namespace krov_nad_glavom_api.Application.Validators
{
    public class ReservationToAddValidator : AbstractValidator<ReservationToAddDto>
    {
        public ReservationToAddValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId je obavezan.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("UserId mora biti validan GUID.");

            RuleFor(x => x.ApartmentId)
                .NotEmpty().WithMessage("ApartmentId je obavezan.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("ApartmentId mora biti validan GUID.");
        }
    }
}
