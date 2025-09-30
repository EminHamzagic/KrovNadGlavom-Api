using System.Security.Claims;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
		private readonly ISecurePasswordHasher _securePasswordHasher;

		public ResetPasswordCommandHandler(IUnitOfWork unitOfWork, ITokenService tokenService, ISecurePasswordHasher securePasswordHasher)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
			_securePasswordHasher = securePasswordHasher;
		}
        
        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var principal = _tokenService.ValidateAccessToken(request.UserPasswordResetDto.Token, out bool isExpired);

            if (principal == null || isExpired)
                throw new Exception("Nevalidni ili istekao token.");

            var purpose = principal.Claims.FirstOrDefault(c => c.Type == "purpose")?.Value;
            if (purpose != "passwordReset")
                throw new Exception("Nevalidna svrha tokena.");

            var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new Exception("Vevalidni podaci tokena.");

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("Korisnik nije pronađen");

            user.PasswordHash = _securePasswordHasher.Hash(request.UserPasswordResetDto.NewPassword);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.Save();

            return true;
        }
    }
}