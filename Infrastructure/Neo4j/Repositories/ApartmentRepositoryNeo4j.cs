using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Application.Utils.Extensions;
using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Domain.Entities;
using Neo4j.Driver;

namespace krov_nad_glavom_api.Infrastructure.Neo4j.Repositories
{
    public class ApartmentRepositoryNeo4j : RepositoryNeo4j<Apartment>, IApartmentRepository
    {
        private readonly krovNadGlavomNeo4jDbContext _context;
        private readonly string _label = nameof(Apartment);

        public ApartmentRepositoryNeo4j(krovNadGlavomNeo4jDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Apartment>> GetApartmentsByBuildingId(string buildingId)
        {
            var query = $"MATCH (a:{_label} {{ BuildingId: $buildingId, IsAvailable: true }}) RETURN a";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { buildingId });

            var list = new List<Apartment>();
            await foreach (var record in cursor)
                list.Add(record["a"].As<INode>().ToEntity<Apartment>());

            return list;
        }

        public async Task<Apartment> GetApartmentById(string id)
        {
            var query = $"MATCH (a:{_label} {{ Id: $id, IsDeleted: false }}) RETURN a LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { id });

            if (await cursor.FetchAsync())
                return cursor.Current["a"].As<INode>().ToEntity<Apartment>();

            return null;
        }

        public async Task<List<Apartment>> GetApartmentsByIds(List<string> ids)
        {
            var query = $"MATCH (a:{_label}) WHERE a.Id IN $ids AND a.IsDeleted = false RETURN a";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { ids });

            var list = new List<Apartment>();
            await foreach (var record in cursor)
                list.Add(record["a"].As<INode>().ToEntity<Apartment>());

            return list;
        }

        public async Task<(List<ApartmentWithBuildingDto> apartmentsPage, int totalCount, int totalPages)> GetAllAvailableApartmentsWithBuildings(QueryStringParameters parameters)
        {
             await using var session = _context.Driver.AsyncSession();

            // 1. Get reserved apartment ids
            var reservedCursor = await session.RunAsync(
                "MATCH (r:Reservation) WHERE r.ToDate > $now RETURN r.ApartmentId AS id",
                new { now = DateTime.Now }
            );
            var reservedApartmentIds = (await reservedCursor.ToListAsync())
                .Select(r => r["id"].As<string>())
                .ToHashSet();

            // 2. Get approved building ids
            var approvedCursor = await session.RunAsync(
                "MATCH (ar:AgencyRequest { Status: 'Approved' }) RETURN ar.BuildingId AS id"
            );
            var approvedBuildingIds = (await approvedCursor.ToListAsync())
                .Select(r => r["id"].As<string>())
                .ToHashSet();

            // 3. Get apartments
            var apartmentsCursor = await session.RunAsync("MATCH (a:Apartment) RETURN a");
            var apartments = (await apartmentsCursor.ToListAsync())
                .Select(r => r["a"].As<INode>().ToEntity<Apartment>())
                .Where(a => !a.IsDeleted && a.IsAvailable)
                .Where(a => !reservedApartmentIds.Contains(a.Id))
                .Where(a => approvedBuildingIds.Contains(a.BuildingId))
                .ToList();

            // 4. Fetch all needed buildings in one query
            var buildingIds = apartments.Select(a => a.BuildingId).Distinct().ToList();
            var buildingsCursor = await session.RunAsync(
                "MATCH (b:Building) WHERE b.Id IN $ids RETURN b",
                new { ids = buildingIds }
            );
            var buildings = (await buildingsCursor.ToListAsync())
                .Select(r => r["b"].As<INode>().ToEntity<Domain.Entities.Building>())
                .ToDictionary(b => b.Id, b => b);

            // 5. Combine apartments with buildings
            var apartmentsWithBuildings = apartments
                .Where(a => buildings.ContainsKey(a.BuildingId))
                .Select(a => new ApartmentWithBuildingDto
                {
                    Apartment = a,
                    Building = buildings[a.BuildingId]
                })
                .AsQueryable();

            // 6. Apply filter and sort
            var filtered = apartmentsWithBuildings.Filter(parameters).Sort(parameters).ToList();
            var totalCount = filtered.Count;
            parameters.checkOverflow(totalCount);

            var apartmentsPage = filtered
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);

            return (apartmentsPage, totalCount, totalPages);
        }
    }
}
