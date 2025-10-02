using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MongoDB.Driver;

namespace krov_nad_glavom_api.Infrastructure.MongoDB.Repositories
{
    public class UserSessionRepositoryMongo : RepositoryMongo<UserSession>, IUserSessionRepository
    {
        private readonly IMongoCollection<UserSession> _userSessions;

        public UserSessionRepositoryMongo(krovNadGlavomMongoDbContext context) : base(context)
        {
            _userSessions = context.UserSessions;
        }

        public async Task<UserSession> GetSessionByRefreshToken(string token)
        {
            return await _userSessions
                .Find(u => u.RefreshToken == token)
                .FirstOrDefaultAsync();
        }

        public async Task<UserSession> GetSessionByUserId(string userId)
        {
            return await _userSessions
                .Find(u => u.UserId == userId)
                .FirstOrDefaultAsync();
        }
    }
}