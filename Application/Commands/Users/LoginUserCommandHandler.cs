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
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;
        private readonly ISecurePasswordHasher _securePasswordHasher;
        private readonly IUserSessionService _userSessionService;

        public LoginUserCommandHandler(IUnitofWork unitofWork, IMapper mapper, ISecurePasswordHasher securePasswordHasher, IUserSessionService userSessionService)
        {
            _userSessionService = userSessionService;
            _securePasswordHasher = securePasswordHasher;
            _unitofWork = unitofWork;
            _mapper = mapper;
        }
        
        public async Task<UserToReturnDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitofWork.Users.GetUserByEmail(request.UserToLoginDto.Email);
            if (user == null)
                throw new Exception("Korisnik nije pronađen");

            if (!_securePasswordHasher.Verify(user.PasswordHash, request.UserToLoginDto.Password))
                throw new Exception("Pogrešna lozinka");

            var tokens = await _userSessionService.CreateUserSession(user);

            var userToReturn = _mapper.Map<User, UserToReturnDto>(user);
            userToReturn.AccessToken = tokens.AccessToken;
            userToReturn.RefreshToken = tokens.RefreshToken;

            return userToReturn;
        }
    }
}