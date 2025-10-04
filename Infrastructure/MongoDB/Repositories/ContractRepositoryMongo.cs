using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Domain.Entities;
using MongoDB.Driver;

namespace krov_nad_glavom_api.Infrastructure.MongoDB.Repositories
{
    public class ContractRepositoryMongo : RepositoryMongo<Contract>, IContractRepository
    {
        private readonly IMongoCollection<Contract> _contracts;
        private readonly IMongoCollection<Installment> _installments;

        public ContractRepositoryMongo(krovNadGlavomMongoDbContext context) : base(context)
        {
            _contracts = context.Contracts;
            _installments = context.Installments;
        }

        public Task<(List<Contract> contractPage, int totalCount, int totalPages)> GetContractsByUserId(string userId, QueryStringParameters parameters)
        {
            var contractsQuery = _contracts
                .AsQueryable()
                .Where(c => c.UserId == userId && c.Status == parameters.Status);

            var totalCount = contractsQuery.Count();
            parameters.checkOverflow(totalCount);

            var contractsPage = contractsQuery
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);

            return Task.FromResult((contractsPage, totalCount, totalPages));
        }

        public Task<(List<Contract> contractPage, int totalCount, int totalPages)> GetContractsByAgencyId(string agencyId, QueryStringParameters parameters)
        {
            var contractsQuery = _contracts
                .AsQueryable()
                .Where(c => c.AgencyId == agencyId && c.Status == parameters.Status);

            var totalCount = contractsQuery.Count();
            parameters.checkOverflow(totalCount);

            var contractsPage = contractsQuery
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);

            return Task.FromResult((contractsPage, totalCount, totalPages));
        }

        public async Task<Contract> GetContractByApartmentId(string apartmentId)
        {
            return await _contracts
                .Find(c => c.ApartmentId == apartmentId)
                .FirstOrDefaultAsync();
        }

        public async Task<Contract> GetContractByApartmentIdAndUserId(string apartmentId, string userId)
        {
            return await _contracts
                .Find(c => c.ApartmentId == apartmentId && c.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Contract>> GetContractsByApartmentIds(List<string> ids)
        {
            return await _contracts
                .Find(u => ids.Contains(u.Id) && u.Status != "Invalid")
                .ToListAsync();
        }

        public async Task<List<Contract>> GetLatePaymentContracts(User user)
        {
            var now = DateTime.UtcNow;

            var lateInstallments = await _installments
                .Find(i => i.DueDate < now && !i.IsConfirmed && !i.IsLate)
                .ToListAsync();

            if (!lateInstallments.Any())
                return new List<Contract>();

            var contractIds = lateInstallments.Select(i => i.ContractId).Distinct().ToList();

            var lateInstallmentIds = lateInstallments.Select(i => i.Id).ToList();
            var filter = Builders<Installment>.Filter.In(i => i.Id, lateInstallmentIds);
            var update = Builders<Installment>.Update.Set(i => i.IsLate, true);
            await _installments.UpdateManyAsync(filter, update);

            if (user.AgencyId == null)
            {
                return await _contracts
                    .Find(c => contractIds.Contains(c.Id) && c.UserId == user.Id)
                    .ToListAsync();
            }
            else
            {
                return await _contracts
                    .Find(c => contractIds.Contains(c.Id) && c.AgencyId == user.AgencyId)
                    .ToListAsync();
            }
        }

        public async Task<List<Contract>> GetByUserId(string userId)
		{
			return await _contracts
                .Find(d => d.UserId == userId)
                .ToListAsync();
		}
    }
}