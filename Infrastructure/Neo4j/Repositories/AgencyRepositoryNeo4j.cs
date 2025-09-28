using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Application.Utils.Extensions;
using krov_nad_glavom_api.Domain.Entities;
using Neo4j.Driver;

namespace krov_nad_glavom_api.Infrastructure.Neo4j.Repositories
{
    public class AgencyRepositoryNeo4j : RepositoryNeo4j<Agency>, IAgencyRepository
    {
        private readonly krovNadGlavomNeo4jDbContext _context;
        private readonly string _label = nameof(Agency);

        public AgencyRepositoryNeo4j(krovNadGlavomNeo4jDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Agency> GetAgencyByName(string name)
        {
            var query = $"MATCH (a:{_label} {{ Name: $name }}) RETURN a LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { name });

            if (await cursor.FetchAsync())
                return cursor.Current["a"].As<INode>().ToEntity<Agency>();

            return null;
        }

        public async Task<List<Agency>> GetAgenciesByIds(List<string> ids)
        {
            var query = $"MATCH (a:{_label}) WHERE a.Id IN $ids RETURN a";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { ids });

            var list = new List<Agency>();
            await foreach (var record in cursor)
                list.Add(record["a"].As<INode>().ToEntity<Agency>());

            return list;
        }

        public async Task<(List<Agency> agenciesPage, int totalCount, int totalPages)> GetAgenciesQuery(QueryStringParameters parameters)
        {
            var all = await GetAllAsync();
            var allQuery = all.AsQueryable();

            var filtered = allQuery.Filter(parameters).Sort(parameters).ToList();

            var totalCount = filtered.Count;
            parameters.checkOverflow(totalCount);

            var agenciesPage = filtered
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);

            return (agenciesPage, totalCount, totalPages);
        }
    }
}
