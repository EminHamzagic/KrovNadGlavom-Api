using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils.Extensions;
using krov_nad_glavom_api.Data.DTO.AgencyRequest;
using krov_nad_glavom_api.Domain.Entities;
using Neo4j.Driver;

namespace krov_nad_glavom_api.Infrastructure.Neo4j.Repositories
{
    public class AgencyRequestRepositoryNeo4j : RepositoryNeo4j<AgencyRequest>, IAgencyRequestRepository
    {
        private readonly krovNadGlavomNeo4jDbContext _context;
        private readonly string _label = nameof(AgencyRequest);

        public AgencyRequestRepositoryNeo4j(krovNadGlavomNeo4jDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetAgencyBuildingCount(string agencyId)
        {
            await using var session = _context.Driver.AsyncSession();

            var cursor = await session.RunAsync(
                $"MATCH (ar:{_label} {{ AgencyId: $agencyId, IsDeleted: false }}) RETURN count(ar) AS cnt",
                new { agencyId }
            );

            var record = await cursor.SingleAsync();
            return record["cnt"].As<int>();
        }

        public async Task<int> GetAgencyApartmentCount(string agencyId)
        {
            var query = @"
                MATCH (ar:AgencyRequest { AgencyId: $agencyId, IsDeleted: false })
                MATCH (ap:Apartment { BuildingId: ar.BuildingId })
                RETURN count(ap) AS cnt";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { agencyId });
            var record = await cursor.SingleAsync();
            return record["cnt"].As<int>();
        }

        public async Task<bool> CheckForExistingRequest(AgencyRequestToAddDto dto)
        {
            var query = $"MATCH (ar:{_label} {{ AgencyId: $agencyId, BuildingId: $buildingId, IsDeleted: false }}) RETURN count(ar) > 0 AS exists";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { agencyId = dto.AgencyId, buildingId = dto.BuildingId });
            var record = await cursor.SingleAsync();
            return record["exists"].As<bool>();
        }

        public async Task<List<AgencyRequest>> GetAgencyRequestsByAgencyId(string agencyId, string status)
        {
            var query = $"MATCH (ar:{_label} {{ AgencyId: $agencyId, Status: $status, IsDeleted: false }}) RETURN ar";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { agencyId, status });

            var list = new List<AgencyRequest>();
            await foreach (var record in cursor)
                list.Add(record["ar"].As<INode>().ToEntity<AgencyRequest>());

            return list;
        }

        public async Task<List<AgencyRequest>> GetAgencyRequestsByCompanyId(string companyId, string status)
        {
            var query = @"
                MATCH (b:Building { CompanyId: $companyId })
                MATCH (ar:AgencyRequest { Status: $status, IsDeleted: false })
                WHERE ar.BuildingId = b.Id
                RETURN ar";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { companyId, status });

            var list = new List<AgencyRequest>();
            await foreach (var record in cursor)
                list.Add(record["ar"].As<INode>().ToEntity<AgencyRequest>());

            return list;
        }

        public async Task<Agency> GetAgencyByBuildingId(string buildingId)
        {
            const string query1 = @"
                MATCH (ar:AgencyRequest { BuildingId: $buildingId, Status: 'Approved', IsDeleted: false })
                RETURN ar.AgencyId AS agencyId
                LIMIT 1";

            await using var session = _context.Driver.AsyncSession();

            var cursor1 = await session.RunAsync(query1, new { buildingId });

            if (await cursor1.FetchAsync())
            {
                var agencyId = cursor1.Current["agencyId"].As<string>();

                const string query2 = "MATCH (a:Agency { Id: $agencyId }) RETURN a LIMIT 1";
                var cursor2 = await session.RunAsync(query2, new { agencyId });

                if (await cursor2.FetchAsync())
                {
                    return cursor2.Current["a"].As<INode>().ToEntity<Agency>();
                }
            }

            return null;
        }

        public async Task<List<Agency>> GetAgenciesByBuildingIds(List<string> ids)
        {
            var query = @"
                MATCH (ar:AgencyRequest)
                WHERE ar.BuildingId IN $ids AND ar.Status = 'Approved' AND ar.IsDeleted = false
                WITH ar.AgencyId AS agencyId
                MATCH (a:Agency)
                WHERE a.Id = agencyId
                RETURN a";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { ids });

            var list = new List<Agency>();
            await foreach (var record in cursor)
                list.Add(record["a"].As<INode>().ToEntity<Agency>());

            return list;
        }

        public async Task<string> GetBuildingRequestStatus(string buildingId)
        {
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync($"MATCH (ar:{_label} {{ BuildingId: $buildingId, IsDeleted: false }}) RETURN ar.Status AS status LIMIT 1",
                new { buildingId });

            if (await cursor.FetchAsync())
            {
                return cursor.Current["status"].As<string>();
            }

            return null;
        }

        public async Task<decimal> GetAgencyCommissionForBuilding(string buildingId)
        {
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync($"MATCH (ar:{_label} {{ BuildingId: $buildingId, IsDeleted: false }}) RETURN ar.CommissionPercentage AS commission LIMIT 1",
                new { buildingId });

            if (await cursor.FetchAsync())
            {
                return cursor.Current["commission"].As<decimal>();
            }

            return 0;
        }
    }
}
