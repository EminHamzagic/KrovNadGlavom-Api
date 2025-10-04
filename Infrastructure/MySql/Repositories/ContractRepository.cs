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

        public async Task<Contract> GetContractByApartmentId(string apartmentId)
        {
            return await _context.Contracts.Where(c => c.ApartmentId == apartmentId).FirstOrDefaultAsync();
        }

        public async Task<Contract> GetContractByApartmentIdAndUserId(string apartmentId, string userId)
        {
            return await _context.Contracts.Where(c => c.ApartmentId == apartmentId && c.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<List<Contract>> GetContractsByApartmentIds(List<string> ids)
        {
            return await _context.Contracts.Where(c => ids.Contains(c.ApartmentId) && c.Status != "Invalid").ToListAsync();
        }

        public async Task<List<Contract>> GetLatePaymentContracts(User user)
        {
            var lateInstallments = await _context.Installments.Where(i => i.DueDate < DateTime.Now && i.IsConfirmed == false && i.IsLate == false).ToListAsync();
            if (lateInstallments.Any())
            {
                foreach (var item in lateInstallments)
                {
                    item.IsLate = true;
                }
                await _context.SaveChangesAsync();
                var contractIds = lateInstallments.Select(i => i.ContractId).Distinct().ToList();
                if (user.AgencyId == null && contractIds.Count > 0)
                {
                    return await _context.Contracts.Where(c => contractIds.Contains(c.Id) && c.UserId == user.Id).ToListAsync();
                }
                else
                {
                    return await _context.Contracts.Where(c => contractIds.Contains(c.Id) && c.AgencyId == user.AgencyId).ToListAsync();
                }
            }

            return new List<Contract>();
        }
        
        public async Task<List<Contract>> GetByUserId(string userId)
		{
			return await _context.Contracts.Where(u => u.UserId == userId).ToListAsync();
		}
    }
}