using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class UpdateUserRegistrationStatusCommandHandler : IRequestHandler<UpdateUserRegistrationStatusCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        public UpdateUserRegistrationStatusCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateUserRegistrationStatusCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
            if (user == null)
                throw new Exception("Korisnik nije pronađen");

            var agency = await _unitOfWork.Agencies.GetByIdAsync(user.AgencyId);

            user.IsAllowed = request.UserRegStatusUpdateDto.Allowed;
            if (user.IsAllowed)
            {
                await _emailService.SendEmailAsync(user.Email, "Profil odobren", "Profil odobren", _emailService.GetAllowedProfileHtmlBody(request.Origin));
                _unitOfWork.Users.Update(user);
                if (agency != null)
                {
                    agency.IsAllowed = true;
                    _unitOfWork.Agencies.Update(agency);
                }
            }
            else
            {
                await _emailService.SendEmailAsync(user.Email, "Registracija odbijena", "Registracija odbijena", _emailService.GetRejectedProfileBody(request.UserRegStatusUpdateDto.Reason));
                if (agency != null)
                {
                    agency = await _unitOfWork.Agencies.GetByIdAsync(user.AgencyId);
                    _unitOfWork.Agencies.Remove(agency);
                    _unitOfWork.Users.Remove(user);
                }
            }

            await _unitOfWork.Save();

            return true;
        }
    }
}