using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.DiscountRequest;
using krov_nad_glavom_api.Domain.Entities;
using krov_nad_glavom_api.Application.Utils.Extensions;
using Neo4j.Driver;

namespace krov_nad_glavom_api.Infrastructure.Neo4j.Repositories
{
    public class DiscountRequestRepositoryNeo4j : RepositoryNeo4j<DiscountRequest>, IDiscountRequestRepository
    {
        private readonly krovNadGlavomNeo4jDbContext _context;
        private readonly string _label = nameof(DiscountRequest);

        public DiscountRequestRepositoryNeo4j(krovNadGlavomNeo4jDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> CheckForExistingRequest(DiscountRequestToAddDto dto)
        {
            var query = $"MATCH (d:{_label} {{ UserId: $userId, ApartmentId: $apartmentId, Status: 'Approved' }}) RETURN d LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { userId = dto.UserId, apartmentId = dto.ApartmentId });

            return !await cursor.FetchAsync();
        }

        public async Task<List<DiscountRequest>> GetDiscountRequestsByUserId(string userId, string status)
        {
            var query = $"MATCH (d:{_label} {{ UserId: $userId, Status: $status }}) RETURN d ORDER BY d.CreatedAt DESC";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { userId, status });

            var list = new List<DiscountRequest>();
            await foreach (var record in cursor)
                list.Add(record["d"].As<INode>().ToEntity<DiscountRequest>());

            return list;
        }

        public async Task<List<DiscountRequest>> GetDiscountRequestsByAgencyId(string agencyId, string status)
        {
            var query = $"MATCH (d:{_label} {{ AgencyId: $agencyId, Status: $status }}) RETURN d ORDER BY d.CreatedAt DESC";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { agencyId, status });

            var list = new List<DiscountRequest>();
            await foreach (var record in cursor)
                list.Add(record["d"].As<INode>().ToEntity<DiscountRequest>());

            return list;
        }

        public async Task<List<DiscountRequest>> GetDiscountRequestsByCompanyId(string companyId, string status)
        {
            var query = $"MATCH (d:{_label} {{ ConstructionCompanyId: $companyId, Status: $status }}) RETURN d ORDER BY d.CreatedAt DESC";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { companyId, status });

            var list = new List<DiscountRequest>();
            await foreach (var record in cursor)
                list.Add(record["d"].As<INode>().ToEntity<DiscountRequest>());

            return list;
        }

        public async Task<DiscountRequest> GetByApartmentId(string apartmentId)
        {
            var query = $"MATCH (d:{_label} {{ ApartmentId: $apartmentId }}) RETURN d LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { apartmentId });

            if (await cursor.FetchAsync())
                return cursor.Current["d"].As<INode>().ToEntity<DiscountRequest>();

            return null;
        }

        public async Task<DiscountRequest> GetByApartmentAndUserId(string apartmentId, string userId)
        {
            var query = $"MATCH (d:{_label} {{ ApartmentId: $apartmentId, UserId: $userId }}) RETURN d LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { apartmentId, userId });

            if (await cursor.FetchAsync())
                return cursor.Current["d"].As<INode>().ToEntity<DiscountRequest>();

            return null;
        }
    }
}
