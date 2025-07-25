using krov_nad_glavom_api.Data.DTO.User;
using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Services.Interfaces
{
    public interface IUserSessionService
    {
        Task<UserSession> GetSessionByRefreshToken(string refreshToken);
        Task UpdateRefreshTokenExpiry(string refreshToken, DateTime newExpiry);
        Task DeleteSession(string refreshToken);
        Task<UserTokensDto> CreateUserSession(User user);
    }
}