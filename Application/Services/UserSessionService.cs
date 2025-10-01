using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using krov_nad_glavom_api.Data.DTO.User;
using krov_nad_glavom_api.Domain.Entities;
using ZstdSharp.Unsafe;

namespace krov_nad_glavom_api.Application.Services
{
    public class UserSessionService : IUserSessionService
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly ITokenService _tokenService;

		public UserSessionService(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
			_tokenService = tokenService;
		}

        public async Task<UserSession> GetSessionByRefreshToken(string refreshToken)
        {
            var session = await _unitOfWork.UserSessions.GetSessionByRefreshToken(refreshToken);
            return session;
        }

        public async Task UpdateRefreshTokenExpiry(string refreshToken, DateTime newExpiry)
        {
            var session = await _unitOfWork.UserSessions.GetSessionByRefreshToken(refreshToken);
            session.RefreshTokenExpiry = newExpiry;
            await _unitOfWork.Save();
        }

        public async Task DeleteSession(string refreshToken)
        {
            var session = await _unitOfWork.UserSessions.GetSessionByRefreshToken(refreshToken);
            _unitOfWork.UserSessions.Remove(session);
            await _unitOfWork.Save();
        }

        public async Task<UserTokensDto> CreateUserSession(User user)
        {
            var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Role);
            var refreshToken = Guid.NewGuid().ToString();
            var refreshExpiry = DateTime.UtcNow.AddHours(5);

            var existingSession = await _unitOfWork.UserSessions.GetSessionByUserId(user.Id);
            if (existingSession != null)
            {
                _unitOfWork.UserSessions.Remove(existingSession);
            }

            var session = new UserSession
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                RefreshToken = refreshToken,
                RefreshTokenExpiry = refreshExpiry,
                Role = user.Role
            };

            await _unitOfWork.UserSessions.AddAsync(session);
            await _unitOfWork.Save();

            var tokensToReturn = new UserTokensDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return tokensToReturn;
        }
    }
}