using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using krov_nad_glavom_api.Application.Utils.Extensions;
using Neo4j.Driver;

namespace krov_nad_glavom_api.Infrastructure.Neo4j.Repositories
{
    public class PriceListRepositoryNeo4j : RepositoryNeo4j<PriceList>, IPriceListRepository
    {
        private readonly krovNadGlavomNeo4jDbContext _context;
        private readonly string _label = nameof(PriceList);

        public PriceListRepositoryNeo4j(krovNadGlavomNeo4jDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PriceList> GetPriceListByBuildingId(string buildingId)
        {
            var query = $"MATCH (p:{_label} {{ BuildingId: $buildingId }}) RETURN p LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { buildingId });

            if (await cursor.FetchAsync())
                return cursor.Current["p"].As<INode>().ToEntity<PriceList>();

            return null;
        }

        public async Task<List<PriceList>> GetPriceListsByBuildingIds(List<string> ids)
        {
            var query = $"MATCH (p:{_label}) WHERE p.BuildingId IN $ids RETURN p";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { ids });

            var list = new List<PriceList>();
            await foreach (var record in cursor)
                list.Add(record["p"].As<INode>().ToEntity<PriceList>());

            return list;
        }
    }
}
