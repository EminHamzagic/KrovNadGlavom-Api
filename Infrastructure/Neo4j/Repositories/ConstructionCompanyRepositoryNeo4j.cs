using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using krov_nad_glavom_api.Application.Utils.Extensions;
using Neo4j.Driver;

namespace krov_nad_glavom_api.Infrastructure.Neo4j.Repositories
{
    public class ConstructionCompanyRepositoryNeo4j : RepositoryNeo4j<ConstructionCompany>, IConstructionCompanyRepository
    {
        private readonly krovNadGlavomNeo4jDbContext _context;
        private readonly string _label = nameof(ConstructionCompany);

        public ConstructionCompanyRepositoryNeo4j(krovNadGlavomNeo4jDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ConstructionCompany> GetCompanyByName(string name)
        {
            var query = $"MATCH (c:{_label} {{ Name: $name }}) RETURN c LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { name });

            if (await cursor.FetchAsync())
                return cursor.Current["c"].As<INode>().ToEntity<ConstructionCompany>();

            return null;
        }

        public async Task<List<ConstructionCompany>> GetCompaniesByIds(List<string> ids)
        {
            var query = $"MATCH (c:{_label}) WHERE c.Id IN $ids RETURN c";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { ids });

            var list = new List<ConstructionCompany>();
            await foreach (var record in cursor)
                list.Add(record["c"].As<INode>().ToEntity<ConstructionCompany>());

            return list;
        }

        public async Task<ConstructionCompany> GetCompanyById(string id)
        {
            var query = $"MATCH (c:{_label} {{ Id: $id }}) RETURN c LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { id });

            if (await cursor.FetchAsync())
                return cursor.Current["c"].As<INode>().ToEntity<ConstructionCompany>();

            return null;
        }
    }
}
