using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Infrastructure.MySql.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly krovNadGlavomDbContext _context;

        public UserRepository(krovNadGlavomDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetUsersByIds(List<string> ids)
        {
            return await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync();
        }

        public async Task<User> GetUserByCompanyId(string companyId)
        {
            return await _context.Users.Where(u => u.ConstructionCompanyId == companyId).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByAgencyId(string agencyId)
        {
            return await _context.Users.Where(u => u.AgencyId == agencyId).FirstOrDefaultAsync();
        }
    }
}