using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IUserSessionRepository : IRepository<UserSession>
    {
        Task<UserSession> GetSessionByRefreshToken(string token);
    }
}