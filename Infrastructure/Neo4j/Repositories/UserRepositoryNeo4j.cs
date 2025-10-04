using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using krov_nad_glavom_api.Application.Utils.Extensions;
using Neo4j.Driver;
using krov_nad_glavom_api.Application.Utils;

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
            var cursor = await session.RunAsync($"MATCH (u:{_label} {{ Email: $email, IsDeleted: false }}) RETURN u LIMIT 1", new { email });


            if (await cursor.FetchAsync())
            {
                return cursor.Current["u"].As<INode>().ToEntity<User>();
            }

            return null;
        }

        public async Task<List<User>> GetUsersByIds(List<string> ids)
        {
            var query = $"MATCH (u:{_label}) WHERE u.Id IN $ids AND u.IsDeleted = false RETURN u";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { ids });

            var list = new List<User>();
            await foreach (var record in cursor)
            {
                list.Add(record["u"].As<INode>().ToEntity<User>());
            }
            return list;
        }

        public async Task<User> GetUserByCompanyId(string companyId)
        {
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync($"MATCH (u:{_label} {{ ConstructionCompanyId: $companyId, IsDeleted: false }}) RETURN u LIMIT 1", new { companyId });


            if (await cursor.FetchAsync())
            {
                return cursor.Current["u"].As<INode>().ToEntity<User>();
            }

            return null;
        }

        public async Task<User> GetUserByAgencyId(string agencyId)
        {
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync($"MATCH (u:{_label} {{ AgencyId: $agencyId, IsDeleted: false }}) RETURN u LIMIT 1", new { agencyId });


            if (await cursor.FetchAsync())
            {
                return cursor.Current["u"].As<INode>().ToEntity<User>();
            }

            return null;
        }

        public async Task<(List<User> userPage, int totalCount, int totalPages)> GetUsersPage(QueryStringParameters parameters)
        {
            var query = $"MATCH (u:{_label} {{ IsDeleted: false, IsAllowed: $isAllowed }}) WHERE u.Role <> 'Admin' RETURN u";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { isAllowed = parameters.IsAllowed});

            var users = new List<User>();
            await foreach (var record in cursor)
                users.Add(record["u"].As<INode>().ToEntity<User>());

            var filtered = users.AsQueryable().Filter(parameters).Sort(parameters).ToList();
            var totalCount = filtered.Count;
            parameters.checkOverflow(totalCount);

            var usersPage = filtered
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);

            return (usersPage, totalCount, totalPages);
        }
    }
}
