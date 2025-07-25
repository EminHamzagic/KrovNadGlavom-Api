using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using krov_nad_glavom_api.Data.DTO.User;
using krov_nad_glavom_api.Domain.Entities;
using ZstdSharp.Unsafe;

namespace krov_nad_glavom_api.Application.Services
{
    public class UserSessionService : IUserSessionService
    {
        private readonly IUnitofWork _unitofWork;
		private readonly ITokenService _tokenService;

		public UserSessionService(IUnitofWork unitofWork, ITokenService tokenService)
        {
            _unitofWork = unitofWork;
			_tokenService = tokenService;
		}

        public async Task<UserSession> GetSessionByRefreshToken(string refreshToken)
        {
            var session = await _unitofWork.UserSessions.GetSessionByRefreshToken(refreshToken);
            return session;
        }

        public async Task UpdateRefreshTokenExpiry(string refreshToken, DateTime newExpiry)
        {
            var session = await _unitofWork.UserSessions.GetSessionByRefreshToken(refreshToken);
            session.RefreshTokenExpiry = newExpiry;
            await _unitofWork.Save();
        }

        public async Task DeleteSession(string refreshToken)
        {
            var session = await _unitofWork.UserSessions.GetSessionByRefreshToken(refreshToken);
            _unitofWork.UserSessions.Remove(session);
            await _unitofWork.Save();
        }

        public async Task<UserTokensDto> CreateUserSession(User user)
        {
            var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Role);
            var refreshToken = Guid.NewGuid().ToString();
            var refreshExpiry = DateTime.UtcNow.AddMinutes(30);

            var session = new UserSession
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                RefreshToken = refreshToken,
                RefreshTokenExpiry = refreshExpiry,
                Role = user.Role
            };

            _unitofWork.UserSessions.AddAsync(session);
            await _unitofWork.Save();

            var tokensToReturn = new UserTokensDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return tokensToReturn;
        }
    }
}