using krov_nad_glavom_api.Application.Interfaces;
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

        public async Task<List<Contract>> GetContractsByUserId(string userId)
        {
            return await _context.Contracts.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task<List<Contract>> GetContractsByAgencyId(string agencyId)
        {
            return await _context.Contracts.Where(c => c.AgencyId == agencyId).ToListAsync();
        }
    }
}