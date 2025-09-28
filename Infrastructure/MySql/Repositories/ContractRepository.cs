using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Infrastructure.MySql.Repositories
{
    public class ContractRepository : Repository<Contract>, IContractRepository
    {
        private readonly krovNadGlavomDbContext _context;

        public ContractRepository(krovNadGlavomDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<(List<Contract> contractPage, int totalCount, int totalPages)> GetContractsByUserId(string userId, QueryStringParameters parameters)
        {
            var contractsQuery = _context.Contracts.Where(c => c.UserId == userId && c.Status == parameters.Status).AsQueryable();

            var totalCount = contractsQuery.Count();
            parameters.checkOverflow(totalCount);

            var contractsPage = await contractsQuery
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);
            
            return (contractsPage, totalCount, totalPages);
        }

        public async Task<(List<Contract> contractPage, int totalCount, int totalPages)> GetContractsByAgencyId(string agencyId, QueryStringParameters parameters)
        {
            var contractsQuery = _context.Contracts.Where(c => c.AgencyId == agencyId && c.Status == parameters.Status).AsQueryable();

            var totalCount = contractsQuery.Count();
            parameters.checkOverflow(totalCount);

            var contractsPage = await contractsQuery
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);
            
            return (contractsPage, totalCount, totalPages);
        }
        public async Task<Contract> GetContractsByApartmentId(string apartmentId)
        {
            return await _context.Contracts.Where(c => c.ApartmentId == apartmentId).FirstOrDefaultAsync();
        }
    }
}