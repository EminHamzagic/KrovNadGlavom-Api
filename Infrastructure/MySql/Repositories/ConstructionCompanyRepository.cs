using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Infrastructure.MySql.Repositories
{
	public class ConstructionCompanyRepository : Repository<ConstructionCompany>, IConstructionCompanyRepository
	{
		private readonly krovNadGlavomDbContext _context;

		public ConstructionCompanyRepository(krovNadGlavomDbContext context) : base(context)
		{
			_context = context;
		}
		
		public async Task<ConstructionCompany> GetCompanyByName(string name)
        {
            return await _context.ConstructionCompanies.Where(u => u.Name == name).FirstOrDefaultAsync();
        }
		
		public async Task<List<ConstructionCompany>> GetCompaniesByIds(List<string> ids)
        {
            return await _context.ConstructionCompanies.Where(u => ids.Contains(u.Id)).ToListAsync();
        }
        public async Task<ConstructionCompany> GetCompanyById(string id)
        {
            return await _context.ConstructionCompanies
                                 .Where(c => c.Id == id)
                                 .FirstOrDefaultAsync();
        }

    }
}