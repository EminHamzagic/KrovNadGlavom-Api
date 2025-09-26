using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly ISecurePasswordHasher _securePasswordHasher;

        public ChangePasswordCommandHandler(IUnitofWork unitofWork, ISecurePasswordHasher securePasswordHasher)
        {
            _unitofWork = unitofWork;
            _securePasswordHasher = securePasswordHasher;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitofWork.Users.GetByIdAsync(request.UserId);
            if (user == null)
                throw new Exception("Korisnik nije pronađen");

            // Proveri staru lozinku
            if (!_securePasswordHasher.Verify(user.PasswordHash, request.ChangePasswordDto.OldPassword))
                throw new Exception("Stara lozinka nije ispravna");

            // Hesiraj novu lozinku
            user.PasswordHash = _securePasswordHasher.Hash(request.ChangePasswordDto.NewPassword);

            _unitofWork.Users.Update(user);
            await _unitofWork.Save();

            return true;
        }
    }
}
