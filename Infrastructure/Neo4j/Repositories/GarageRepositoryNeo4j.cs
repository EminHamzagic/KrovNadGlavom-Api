using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using krov_nad_glavom_api.Application.Utils.Extensions;
using Neo4j.Driver;

namespace krov_nad_glavom_api.Infrastructure.Neo4j.Repositories
{
    public class GarageRepositoryNeo4j : RepositoryNeo4j<Garage>, IGarageRepository
    {
        private readonly krovNadGlavomNeo4jDbContext _context;
        private readonly string _label = nameof(Garage);

        public GarageRepositoryNeo4j(krovNadGlavomNeo4jDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Garage> GetGarageById(string id)
        {
            var query = $"MATCH (g:{_label} {{ Id: $id }}) RETURN g LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { id });

            if (await cursor.FetchAsync())
                return cursor.Current["g"].As<INode>().ToEntity<Garage>();

            return null;
        }

        public async Task<List<Garage>> GetGaragesByBuildingId(string buildingId)
        {
            var query = $"MATCH (g:{_label} {{ BuildingId: $buildingId }}) RETURN g";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { buildingId });

            var list = new List<Garage>();
            await foreach (var record in cursor)
                list.Add(record["g"].As<INode>().ToEntity<Garage>());

            return list;
        }

        public async Task<int> GetBuildingGarageCount(string buildingId)
        {
            var query = $"MATCH (g:{_label} {{ BuildingId: $buildingId }}) RETURN count(g) AS cnt";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { buildingId });

            var record = await cursor.SingleAsync();
            return record["cnt"].As<int>();
        }

        public async Task<bool> IsSpotNumberFree(string spotNumber, Garage garage)
        {
            var query = $"MATCH (g:{_label} {{ BuildingId: $buildingId, SpotNumber: $spotNumber }}) WHERE g.Id <> $garageId RETURN g LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { buildingId = garage.BuildingId, spotNumber, garageId = garage.Id });

            return !await cursor.FetchAsync();
        }

        public async Task<List<Garage>> GetGaragesByApartmentId(string apartmentId)
        {
            var query = $"MATCH (g:{_label} {{ ApartmentId: $apartmentId }}) RETURN g";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { apartmentId });

            var list = new List<Garage>();
            await foreach (var record in cursor)
                list.Add(record["g"].As<INode>().ToEntity<Garage>());

            return list;
        }
    }
}
