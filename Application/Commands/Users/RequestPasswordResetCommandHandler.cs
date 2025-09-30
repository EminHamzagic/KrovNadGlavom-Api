using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class RequestPasswordResetCommandHandler : IRequestHandler<RequestPasswordResetCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
		private readonly ITokenService _tokenService;

		public RequestPasswordResetCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
			_tokenService = tokenService;
		}

        public async Task<bool> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetUserByEmail(request.UserPasswordResetRequestDto.Email);
            if (user == null)
                throw new Exception("Korisnik nije pronađen");

            var token = _tokenService.GeneratePasswordResetToken(user.Id, user.Email);
            var link = $"http://localhost:5173/reset-password?token={token}";

            await _emailService.SendEmailAsync(user.Email, "Resetovanje lozinke", "Resetovanje lozinke", _emailService.GetPasswordResetHtmlBody(link));

            return true;
        }
    }
}