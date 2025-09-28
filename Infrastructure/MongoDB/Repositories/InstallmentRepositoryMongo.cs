using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace krov_nad_glavom_api.Infrastructure.MongoDB.Repositories
{
    public class InstallmentRepositoryMongo : RepositoryMongo<Installment>, IInstallmentRepository
    {
        private readonly IMongoCollection<Installment> _installments;

        public InstallmentRepositoryMongo(krovNadGlavomMongoDbContext context) : base(context)
        {
            _installments = context.Installments;
        }

        public async Task<List<Installment>> GetInstallmentsByContractId(string contractId)
        {
            return await _installments
                .Find(i => i.ContractId == contractId)
                .SortByDescending(i => i.SequenceNumber)
                .ToListAsync();
        }

        public Task<int> GetConfirmedInstallmentsCount(string contractId)
        {
            return Task.FromResult((int)_installments
                .CountDocuments(i => i.ContractId == contractId && i.IsConfirmed));
        }

        public async Task<int> GetNextSequenceNumber(string contractId)
        {
            var last = await _installments
                .Find(i => i.ContractId == contractId)
                .SortByDescending(i => i.CreatedAt)
                .Project(i => i.SequenceNumber)
                .FirstOrDefaultAsync();

            return last + 1;
        }

        public async Task<decimal> GetTotalPaidAmountAsync(string contractId)
        {
            var total = await _installments
                .Aggregate()
                .Match(i => i.ContractId == contractId && i.IsConfirmed)
                .Group(new BsonDocument
                {
                    { "_id", BsonNull.Value },
                    { "sum", new BsonDocument("$sum", "$Amount") }
                })
                .FirstOrDefaultAsync();

            var sumValue = total["sum"];

            return sumValue.BsonType switch
            {
                BsonType.Int32 => sumValue.AsInt32,
                BsonType.Int64 => sumValue.AsInt64,
                BsonType.Decimal128 => sumValue.AsDecimal,
                BsonType.Double => (decimal)sumValue.AsDouble,
                _ => 0m
            };
        }
    }
}