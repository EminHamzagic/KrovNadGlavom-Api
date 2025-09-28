using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using krov_nad_glavom_api.Application.Utils.Extensions;
using Neo4j.Driver;

namespace krov_nad_glavom_api.Infrastructure.Neo4j.Repositories
{
    public class InstallmentRepositoryNeo4j : RepositoryNeo4j<Installment>, IInstallmentRepository
    {
        private readonly krovNadGlavomNeo4jDbContext _context;
        private readonly string _label = nameof(Installment);

        public InstallmentRepositoryNeo4j(krovNadGlavomNeo4jDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Installment>> GetInstallmentsByContractId(string contractId)
        {
            var query = $"MATCH (i:{_label} {{ ContractId: $contractId }}) RETURN i ORDER BY i.SequenceNumber DESC";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { contractId });

            var list = new List<Installment>();
            await foreach (var record in cursor)
                list.Add(record["i"].As<INode>().ToEntity<Installment>());

            return list;
        }

        public async Task<int> GetConfirmedInstallmentsCount(string contractId)
        {
            var query = $"MATCH (i:{_label} {{ ContractId: $contractId, IsConfirmed: true }}) RETURN count(i) AS cnt";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { contractId });

            var record = await cursor.SingleAsync();
            return record["cnt"].As<int>();
        }

        public async Task<int> GetNextSequenceNumber(string contractId)
        {
            var query = $"MATCH (i:{_label} {{ ContractId: $contractId }}) RETURN i.SequenceNumber AS seq ORDER BY i.CreatedAt DESC LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { contractId });

            if (await cursor.FetchAsync())
                return cursor.Current["seq"].As<int>() + 1;

            return 1;
        }

        public async Task<decimal> GetTotalPaidAmountAsync(string contractId)
        {
            var query = $"MATCH (i:{_label} {{ ContractId: $contractId, IsConfirmed: true }}) RETURN sum(i.Amount) AS total";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { contractId });

            var record = await cursor.SingleAsync();
            return record["total"].As<decimal?>() ?? 0m;
        }
    }
}
