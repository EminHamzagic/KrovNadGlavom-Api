using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Infrastructure.MySql.Repositories
{
	public class UserAgencyFollowRepository : Repository<UserAgencyFollow>, IUserAgencyFollowRepository
	{
		private readonly krovNadGlavomDbContext _context;

		public UserAgencyFollowRepository(krovNadGlavomDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<User>> GetAgencyFollowers(string agencyId)
		{
			var userIds = await _context.UserAgencyFollows.Where(uf => uf.AgencyId == agencyId).Select(uf => uf.UserId).Distinct().ToListAsync();
			var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();

			return users;
		}

		public async Task<List<Agency>> GetUserFollowing(string userId)
		{
			var agencyIds = await _context.UserAgencyFollows.Where(uf => uf.UserId == userId).Select(uf => uf.AgencyId).Distinct().ToListAsync();
			var agencies = await _context.Agencies.Where(u => agencyIds.Contains(u.Id)).ToListAsync();

			return agencies;
		}
    }
}