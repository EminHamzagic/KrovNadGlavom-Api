using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Infrastructure.MySql.Repositories
{
    public class UserSessionRepository : Repository<UserSession>, IUserSessionRepository
    {
        private readonly krovNadGlavomDbContext _context;

        public UserSessionRepository(krovNadGlavomDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<UserSession> GetSessionByRefreshToken(string token)
        {
            return await _context.UserSessions.Where(u => u.RefreshToken == token).FirstOrDefaultAsync();
        }

        public async Task<UserSession> GetSessionByUserId(string userId)
        {
            return await _context.UserSessions.Where(u => u.UserId == userId).FirstOrDefaultAsync();
        }
    }
}