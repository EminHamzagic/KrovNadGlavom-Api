using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MongoDB.Driver;

namespace krov_nad_glavom_api.Infrastructure.MongoDB.Repositories
{
    public class UserAgencyFollowRepositoryMongo : RepositoryMongo<UserAgencyFollow>, IUserAgencyFollowRepository
    {
        private readonly IMongoCollection<UserAgencyFollow> _userAgencyFollows;
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Agency> _agencies;

        public UserAgencyFollowRepositoryMongo(krovNadGlavomMongoDbContext context) : base(context)
        {
            _userAgencyFollows = context.UserAgencyFollows;
            _users = context.Users;
            _agencies = context.Agencies;
        }

        public async Task<List<User>> GetAgencyFollowers(string agencyId)
        {
            var userIds = await _userAgencyFollows
                .Find(uf => uf.AgencyId == agencyId)
                .Project(uf => uf.UserId)
                .ToListAsync();

            userIds = userIds.Distinct().ToList();

            var users = await _users
                .Find(u => userIds.Contains(u.Id))
                .ToListAsync();

            return users;
        }

        public async Task<List<Agency>> GetUserFollowing(string userId)
        {
            var agencyIds = await _userAgencyFollows
                .Find(uf => uf.UserId == userId)
                .Project(uf => uf.AgencyId)
                .ToListAsync();

            agencyIds = agencyIds.Distinct().ToList();

            var agencies = await _agencies
                .Find(a => agencyIds.Contains(a.Id))
                .ToListAsync();

            return agencies;
        }

        public async Task<UserAgencyFollow> IsUserFollowing(string userId, string agencyId)
        {
            return await _userAgencyFollows
                .Find(u => u.UserId == userId && u.AgencyId == agencyId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<UserAgencyFollow>> GetByUserId(string userId)
		{
			return await _userAgencyFollows
                .Find(d => d.UserId == userId)
                .ToListAsync();
		}
    }
}