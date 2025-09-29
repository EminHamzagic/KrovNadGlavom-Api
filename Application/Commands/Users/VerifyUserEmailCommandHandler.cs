using System.Security.Claims;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class VerifyUserEmailCommandHandler : IRequestHandler<VerifyUserEmailCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public VerifyUserEmailCommandHandler(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }
        
        public async Task<bool> Handle(VerifyUserEmailCommand request, CancellationToken cancellationToken)
        {
            var principal = _tokenService.ValidateAccessToken(request.Token, out bool isExpired);

            if (principal == null || isExpired)
                throw new Exception("Nevalidni ili istekao token.");

            var purpose = principal.Claims.FirstOrDefault(c => c.Type == "purpose")?.Value;
            if (purpose != "emailVerification")
                throw new Exception("Nevalidna svrha tokena.");

            var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new Exception("Vevalidni podaci tokena.");

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("Korisnik nije pronađen");

            user.IsVerified = true;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.Save();

            return true;
        }
    }
}