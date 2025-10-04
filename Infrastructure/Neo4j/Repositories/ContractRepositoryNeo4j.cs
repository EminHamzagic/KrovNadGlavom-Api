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

        public async Task<Contract> GetContractByApartmentId(string apartmentId)
        {
            var query = $"MATCH (c:{_label} {{ ApartmentId: $apartmentId }}) RETURN c LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { apartmentId });

            if (await cursor.FetchAsync())
                return cursor.Current["c"].As<INode>().ToEntity<Contract>();

            return null;
        }

        public async Task<Contract> GetContractByApartmentIdAndUserId(string apartmentId, string userId)
        {
            var query = $"MATCH (c:{_label} {{ ApartmentId: $apartmentId, UserId: $userId }}) RETURN c LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { apartmentId, userId });

            if (await cursor.FetchAsync())
                return cursor.Current["c"].As<INode>().ToEntity<Contract>();

            return null;
        }

        public async Task<List<Contract>> GetContractsByApartmentIds(List<string> ids)
        {
            var query = $"MATCH (c:{_label}) WHERE c.Id IN $ids AND NOT c.Status = Invalid RETURN c";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { ids });

            var list = new List<Contract>();
            await foreach (var record in cursor)
                list.Add(record["c"].As<INode>().ToEntity<Contract>());

            return list;
        }

        public async Task<List<Contract>> GetLatePaymentContracts(User user)
        {
            var now = DateTime.UtcNow;

            await using var session = _context.Driver.AsyncSession();

            var installmentsQuery = @"
                MATCH (i:Installment)
                WHERE i.DueDate < $now AND i.IsConfirmed = false AND i.IsLate = false
                RETURN DISTINCT i.ContractId AS contractId, i AS installment";

            var installmentsCursor = await session.RunAsync(installmentsQuery, new { now });

            var contractIds = new List<string>();
            var lateInstallmentIds = new List<string>();

            await foreach (var record in installmentsCursor)
            {
                contractIds.Add(record["contractId"].As<string>());

                lateInstallmentIds.Add(record["installment"].As<INode>().ElementId);
            }

            if (!contractIds.Any())
                return new List<Contract>();

            if (lateInstallmentIds.Any())
            {
                var updateQuery = @"
                    MATCH (i:Installment)
                    WHERE id(i) IN $ids
                    SET i.IsLate = true";

                var nodeIds = lateInstallmentIds.Select(id => Convert.ToInt64(id)).ToList();
                await session.RunAsync(updateQuery, new { ids = nodeIds });
            }

            var contractsQuery = user.AgencyId == null
                ? $@"MATCH (c:{_label})
                    WHERE c.Id IN $ids AND c.UserId = $userId
                    RETURN c"
                : $@"MATCH (c:{_label})
                    WHERE c.Id IN $ids AND c.AgencyId = $agencyId
                    RETURN c";

            var contractsCursor = user.AgencyId == null
                ? await session.RunAsync(contractsQuery, new { ids = contractIds, userId = user.Id })
                : await session.RunAsync(contractsQuery, new { ids = contractIds, agencyId = user.AgencyId });

            var list = new List<Contract>();
            await foreach (var record in contractsCursor)
                list.Add(record["c"].As<INode>().ToEntity<Contract>());

            return list;
        }
        
        public async Task<List<Contract>> GetByUserId(string userId)
		{
			var query = $"MATCH (d:{_label} {{ UserId: $userId }}) RETURN d";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { userId });

            var list = new List<Contract>();
            await foreach (var record in cursor)
                list.Add(record["d"].As<INode>().ToEntity<Contract>());

            return list;
		}
    }
}
