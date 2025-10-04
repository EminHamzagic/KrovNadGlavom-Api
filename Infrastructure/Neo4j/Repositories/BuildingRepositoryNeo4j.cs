using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Domain.Entities;
using krov_nad_glavom_api.Application.Utils.Extensions;
using Neo4j.Driver;

namespace krov_nad_glavom_api.Infrastructure.Neo4j.Repositories
{
    public class BuildingRepositoryNeo4j : RepositoryNeo4j<Building>, IBuildingRepository
    {
        private readonly krovNadGlavomNeo4jDbContext _context;
        private readonly string _label = nameof(Building);

        public BuildingRepositoryNeo4j(krovNadGlavomNeo4jDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Building> GetBuildingById(string id)
        {
            var query = $"MATCH (b:{_label} {{ Id: $id, IsDeleted: false }}) RETURN b LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { id });

            if (await cursor.FetchAsync())
                return cursor.Current["b"].As<INode>().ToEntity<Building>();

            return null;
        }

        public async Task<Building> GetBuildingByParcel(string parcelNum)
        {
            var query = $"MATCH (b:{_label} {{ ParcelNumber: $parcelNum, IsDeleted: false }}) RETURN b LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { parcelNum });

            if (await cursor.FetchAsync())
                return cursor.Current["b"].As<INode>().ToEntity<Building>();

            return null;
        }

        public async Task<List<Building>> GetBuildingsByIds(List<string> ids)
        {
            var query = $"MATCH (b:{_label}) WHERE b.Id IN $ids AND b.IsDeleted = false RETURN b";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { ids });

            var list = new List<Building>();
            await foreach (var record in cursor)
                list.Add(record["b"].As<INode>().ToEntity<Building>());

            return list;
        }

        public async Task<bool> CanAddApartment(ApartmentToAddDto apartmentToAddDto)
        {
            // Get building area
            await using var session = _context.Driver.AsyncSession();
            var buildingQuery = await session.RunAsync($"MATCH (b:{_label} {{ Id: $id }}) RETURN b.Area AS area LIMIT 1", new { id = apartmentToAddDto.BuildingId });
            var buildingRecord = await buildingQuery.SingleAsync();
            var buildingArea = buildingRecord["area"].As<decimal>();

            // Sum existing apartments on same floor
            var sumQuery = @"
                MATCH (a:Apartment { BuildingId: $buildingId, Floor: $floor })
                RETURN sum(a.Area) AS totalArea";
            var sumCursor = await session.RunAsync(sumQuery, new { buildingId = apartmentToAddDto.BuildingId, floor = apartmentToAddDto.Floor });
            var sumRecord = await sumCursor.SingleAsync();
            var totalArea = sumRecord["totalArea"].As<decimal>();

            return buildingArea - (totalArea + apartmentToAddDto.Area) > 0;
        }

        public async Task<(List<Building> buildingsPage, int totalCount, int totalPages)> GetAllValidBuildings(string agencyId, QueryStringParameters parameters)
        {
            await using var session = _context.Driver.AsyncSession();

            var approvedCursor = await session.RunAsync(
                "MATCH (ar:AgencyRequest { Status: 'Approved' }) RETURN ar.BuildingId AS id"
            );
            var approvedIds = (await approvedCursor.ToListAsync())
                .Select(r => r["id"].As<string>())
                .ToHashSet();

            // 2. Get buildings not in approved list
            var cursor = await session.RunAsync(
                $"MATCH (b:{_label}) WHERE NOT b.Id IN $approvedIds AND b.IsDeleted = false RETURN b",
                new { approvedIds }
            );
            var buildings = (await cursor.ToListAsync())
                .Select(r => r["b"].As<INode>().ToEntity<Building>())
                .ToList();

            // 3. Apply filtering and sorting
            var filtered = buildings.AsQueryable().Filter(parameters).Sort(parameters).ToList();
            var totalCount = filtered.Count;
            parameters.checkOverflow(totalCount);

            var buildingsPage = filtered
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);

            return (buildingsPage, totalCount, totalPages);
        }

        public async Task<(List<Building> buildingsPage, int totalCount, int totalPages)> GetCompanyBuildings(string companyId, QueryStringParameters parameters)
        {
            await using var session = _context.Driver.AsyncSession();

            // 2. Get buildings not in approved list
            var cursor = await session.RunAsync(
                $"MATCH (b:{_label}) WHERE b.CompanyId = $companyId AND b.IsDeleted = false RETURN b",
                new { companyId }
            );
            var buildings = (await cursor.ToListAsync())
                .Select(r => r["b"].As<INode>().ToEntity<Building>())
                .ToList();

            // 3. Apply filtering and sorting
            var filtered = buildings.AsQueryable().Filter(parameters).Sort(parameters).ToList();
            var totalCount = filtered.Count;
            parameters.checkOverflow(totalCount);

            var buildingsPage = filtered
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);

            return (buildingsPage, totalCount, totalPages);
        }

        public async Task<(List<Building> buildingsPage, int totalCount, int totalPages)> GetBuildingsPage(QueryStringParameters parameters)
		{
			await using var session = _context.Driver.AsyncSession();

            // 2. Get buildings not in approved list
            var cursor = await session.RunAsync(
                $"MATCH (b:{_label}) WHERE b.IsDeleted = false RETURN b"
            );
            var buildings = (await cursor.ToListAsync())
                .Select(r => r["b"].As<INode>().ToEntity<Building>())
                .ToList();

            // 3. Apply filtering and sorting
            var filtered = buildings.AsQueryable().Filter(parameters).Sort(parameters).ToList();
            var totalCount = filtered.Count;
            parameters.checkOverflow(totalCount);

            var buildingsPage = filtered
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);

            return (buildingsPage, totalCount, totalPages);
		}
    }
}
