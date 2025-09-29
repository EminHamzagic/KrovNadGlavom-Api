using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISecurePasswordHasher _securePasswordHasher;

        public ChangePasswordCommandHandler(IUnitOfWork unitOfWork, ISecurePasswordHasher securePasswordHasher)
        {
            _unitOfWork = unitOfWork;
            _securePasswordHasher = securePasswordHasher;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user == null)
                throw new Exception("Korisnik nije pronađen");

            // Proveri staru lozinku
            if (!_securePasswordHasher.Verify(user.PasswordHash, request.ChangePasswordDto.OldPassword))
                throw new Exception("Stara lozinka nije ispravna");

            // Hesiraj novu lozinku
            user.PasswordHash = _securePasswordHasher.Hash(request.ChangePasswordDto.NewPassword);

            _unitOfWork.Users.Update(user);
            await _unitOfWork.Save();

            return true;
        }
    }
}
