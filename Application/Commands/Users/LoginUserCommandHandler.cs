using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using krov_nad_glavom_api.Data.DTO.User;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserToReturnDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISecurePasswordHasher _securePasswordHasher;
        private readonly IUserSessionService _userSessionService;
		private readonly IContractService _contractService;

		public LoginUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ISecurePasswordHasher securePasswordHasher, IUserSessionService userSessionService, IContractService contractService)
        {
            _userSessionService = userSessionService;
			_contractService = contractService;
			_securePasswordHasher = securePasswordHasher;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<UserToReturnDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetUserByEmail(request.UserToLoginDto.Email);
            if (user == null)
                throw new Exception("Korisnik nije pronađen");

            if (!_securePasswordHasher.Verify(user.PasswordHash, request.UserToLoginDto.Password))
                throw new Exception("Pogrešna lozinka");

            if (!user.IsVerified)
                throw new Exception("Profil nije verifikovan. Molimo vas varifikujte vaš profil");

            await _contractService.CheckUserContracts(user);

            var tokens = await _userSessionService.CreateUserSession(user);

            var userToReturn = _mapper.Map<User, UserToReturnDto>(user);
            userToReturn.AccessToken = tokens.AccessToken;
            userToReturn.RefreshToken = tokens.RefreshToken;

            return userToReturn;

        }
    }
}