using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using krov_nad_glavom_api.Application.Utils.Extensions;
using Neo4j.Driver;

namespace krov_nad_glavom_api.Infrastructure.Neo4j.Repositories
{
    public class UserAgencyFollowRepositoryNeo4j : RepositoryNeo4j<UserAgencyFollow>, IUserAgencyFollowRepository
    {
        private readonly krovNadGlavomNeo4jDbContext _context;
        private readonly string _label = nameof(UserAgencyFollow);

        public UserAgencyFollowRepositoryNeo4j(krovNadGlavomNeo4jDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAgencyFollowers(string agencyId)
        {
            var query = @"
                MATCH (uf:UserAgencyFollow { AgencyId: $agencyId })
                WITH DISTINCT uf.UserId AS userId
                MATCH (u:User { Id: userId })
                RETURN u";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { agencyId });

            var users = new List<User>();
            await foreach (var record in cursor)
                users.Add(record["u"].As<INode>().ToEntity<User>());

            return users;
        }

        public async Task<List<Agency>> GetUserFollowing(string userId)
        {
            var query = @"
                MATCH (uf:UserAgencyFollow { UserId: $userId })
                WITH DISTINCT uf.AgencyId AS agencyId
                MATCH (a:Agency { Id: agencyId })
                RETURN a";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { userId });

            var agencies = new List<Agency>();
            await foreach (var record in cursor)
                agencies.Add(record["a"].As<INode>().ToEntity<Agency>());

            return agencies;
        }

        public async Task<UserAgencyFollow> IsUserFollowing(string userId, string agencyId)
        {
            var query = $"MATCH (uf:{_label} {{ UserId: $userId, AgencyId: $agencyId }}) RETURN uf LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { userId, agencyId });

            if (await cursor.FetchAsync())
                return cursor.Current["uf"].As<INode>().ToEntity<UserAgencyFollow>();

            return null;
        }

        public async Task<List<UserAgencyFollow>> GetByUserId(string userId)
		{
			var query = $"MATCH (d:{_label} {{ UserId: $userId }}) RETURN d";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { userId });

            var list = new List<UserAgencyFollow>();
            await foreach (var record in cursor)
                list.Add(record["d"].As<INode>().ToEntity<UserAgencyFollow>());

            return list;
		}
    }
}
