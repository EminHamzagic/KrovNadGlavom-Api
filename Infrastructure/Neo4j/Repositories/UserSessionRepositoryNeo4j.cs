using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using krov_nad_glavom_api.Application.Utils.Extensions;
using Neo4j.Driver;

namespace krov_nad_glavom_api.Infrastructure.Neo4j.Repositories
{
    public class UserSessionRepositoryNeo4j : RepositoryNeo4j<UserSession>, IUserSessionRepository
    {
        private readonly krovNadGlavomNeo4jDbContext _context;
        private readonly string _label = nameof(UserSession);

        public UserSessionRepositoryNeo4j(krovNadGlavomNeo4jDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<UserSession> GetSessionByRefreshToken(string token)
        {
            var query = $"MATCH (u:{_label} {{ RefreshToken: $token }}) RETURN u LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { token });

            if (await cursor.FetchAsync())
                return cursor.Current["u"].As<INode>().ToEntity<UserSession>();

            return null;
        }
    }
}
