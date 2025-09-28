using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using krov_nad_glavom_api.Application.Utils.Extensions;
using Neo4j.Driver;

namespace krov_nad_glavom_api.Infrastructure.Neo4j.Repositories
{
    public class UserRepositoryNeo4j : RepositoryNeo4j<User>, IUserRepository
    {
        private readonly krovNadGlavomNeo4jDbContext _context;
        private readonly string _label = nameof(User);

        public UserRepositoryNeo4j(krovNadGlavomNeo4jDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync($"MATCH (u:{_label} {{ Email: $email }}) RETURN u LIMIT 1", new { email });


            if (await cursor.FetchAsync())
            {
                return cursor.Current["u"].As<INode>().ToEntity<User>();
            }

            return null;
        }

        public async Task<List<User>> GetUsersByIds(List<string> ids)
        {
            var query = $"MATCH (u:{_label}) WHERE u.Id IN $ids RETURN u";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { ids });

            var list = new List<User>();
            await foreach (var record in cursor)
            {
                list.Add(record["u"].As<INode>().ToEntity<User>());
            }
            return list;
        }
    }
}
