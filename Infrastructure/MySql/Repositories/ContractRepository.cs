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


        public IQueryable<Contract> GetContractsByUserId(string userId, string status)
        {
            return _context.Contracts.Where(c => c.UserId == userId && c.Status == status).AsQueryable();
        }

        public IQueryable<Contract> GetContractsByAgencyId(string agencyId, string status)
        {
            return _context.Contracts.Where(c => c.AgencyId == agencyId && c.Status == status).AsQueryable();
        }
        public async Task<Contract> GetContractsByApartmentId(string apartmentId)
        {
            return await _context.Contracts.Where(c => c.ApartmentId == apartmentId).FirstOrDefaultAsync();
        }
    }
}