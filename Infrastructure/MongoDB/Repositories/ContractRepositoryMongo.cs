using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Domain.Entities;
using MongoDB.Driver;

namespace krov_nad_glavom_api.Infrastructure.MongoDB.Repositories
{
    public class ContractRepositoryMongo : RepositoryMongo<Contract>, IContractRepository
    {
        private readonly IMongoCollection<Contract> _contracts;

        public ContractRepositoryMongo(krovNadGlavomMongoDbContext context) : base(context)
        {
            _contracts = context.Contracts;
        }

        public Task<(List<Contract> constractPage, int totalCount, int totalPages)> GetContractsByUserId(string userId, QueryStringParameters parameters)
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

        public Task<(List<Contract> constractPage, int totalCount, int totalPages)> GetContractsByAgencyId(string agencyId, QueryStringParameters parameters)
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

        public async Task<Contract> GetContractsByApartmentId(string apartmentId)
        {
            return await _contracts
                .Find(c => c.ApartmentId == apartmentId)
                .FirstOrDefaultAsync();
        }
    }
}