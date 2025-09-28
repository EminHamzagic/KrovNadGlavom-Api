using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Domain.Entities;
using krov_nad_glavom_api.Application.Utils.Extensions;
using Neo4j.Driver;

namespace krov_nad_glavom_api.Infrastructure.Neo4j.Repositories
{
    public class ContractRepositoryNeo4j : RepositoryNeo4j<Contract>, IContractRepository
    {
        private readonly krovNadGlavomNeo4jDbContext _context;
        private readonly string _label = nameof(Contract);

        public ContractRepositoryNeo4j(krovNadGlavomNeo4jDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<(List<Contract> contractPage, int totalCount, int totalPages)> GetContractsByUserId(string userId, QueryStringParameters parameters)
        {
            var query = $"MATCH (c:{_label} {{ UserId: $userId, Status: $status }}) RETURN c";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { userId, status = parameters.Status });

            var contracts = new List<Contract>();
            await foreach (var record in cursor)
                contracts.Add(record["c"].As<INode>().ToEntity<Contract>());

            var filtered = contracts.ToList(); // No additional filter needed
            var totalCount = filtered.Count;
            parameters.checkOverflow(totalCount);

            var contractPage = filtered
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);

            return (contractPage, totalCount, totalPages);
        }

        public async Task<(List<Contract> contractPage, int totalCount, int totalPages)> GetContractsByAgencyId(string agencyId, QueryStringParameters parameters)
        {
            var query = $"MATCH (c:{_label} {{ AgencyId: $agencyId, Status: $status }}) RETURN c";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { agencyId, status = parameters.Status });

            var contracts = new List<Contract>();
            await foreach (var record in cursor)
                contracts.Add(record["c"].As<INode>().ToEntity<Contract>());

            var filtered = contracts.ToList(); 
            var totalCount = filtered.Count;
            parameters.checkOverflow(totalCount);

            var contractPage = filtered
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);

            return (contractPage, totalCount, totalPages);
        }

        public async Task<Contract> GetContractsByApartmentId(string apartmentId)
        {
            var query = $"MATCH (c:{_label} {{ ApartmentId: $apartmentId }}) RETURN c LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { apartmentId });

            if (await cursor.FetchAsync())
                return cursor.Current["c"].As<INode>().ToEntity<Contract>();

            return null;
        }
    }
}
