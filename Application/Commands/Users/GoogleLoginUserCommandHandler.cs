using AutoMapper;
using Google.Apis.Auth;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using krov_nad_glavom_api.Data.DTO.User;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class GoogleLoginUserCommandHandler : IRequestHandler<GoogleLoginUserCommand, UserToReturnDto>
    {
		private readonly IUnitofWork _unitofWork;
		private readonly IUserSessionService _userSessionService;
		private readonly IMapper _mapper;

		public GoogleLoginUserCommandHandler(IUnitofWork unitofWork, IUserSessionService userSessionService, IMapper mapper)
        {
			_unitofWork = unitofWork;
			_userSessionService = userSessionService;
			_mapper = mapper;
		}
        public async Task<UserToReturnDto> Handle(GoogleLoginUserCommand request, CancellationToken cancellationToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.GoogleAuthRequest.IdToken);
            var user = await _unitofWork.Users.GetUserByEmail(payload.Email);
            if (user == null)
                throw new Exception("User not found");

            var tokens = await _userSessionService.CreateUserSession(user);

            var userToReturn = _mapper.Map<User, UserToReturnDto>(user);
            userToReturn.AccessToken = tokens.AccessToken;
            userToReturn.RefreshToken = tokens.RefreshToken;

            return userToReturn;
        }
    }
}