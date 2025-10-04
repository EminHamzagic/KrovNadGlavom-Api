using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
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
            return await _context.Users.Where(u => u.Email == email && !u.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetUsersByIds(List<string> ids)
        {
            return await _context.Users.Where(u => ids.Contains(u.Id) && !u.IsDeleted).ToListAsync();
        }

        public async Task<User> GetUserByCompanyId(string companyId)
        {
            return await _context.Users.Where(u => u.ConstructionCompanyId == companyId && !u.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByAgencyId(string agencyId)
        {
            return await _context.Users.Where(u => u.AgencyId == agencyId && !u.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<(List<User> userPage, int totalCount, int totalPages)> GetUsersPage(QueryStringParameters parameters)
        {
            var usersQuery = _context.Users.Where(u => !u.IsDeleted && u.IsAllowed == parameters.IsAllowed && u.Role != "Admin").AsQueryable();

            usersQuery = usersQuery.Filter(parameters).Sort(parameters);

            var totalCount = usersQuery.Count();
            parameters.checkOverflow(totalCount);

            var usersPage = await usersQuery
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);

            return (usersPage, totalCount, totalPages);
        }
    }
}