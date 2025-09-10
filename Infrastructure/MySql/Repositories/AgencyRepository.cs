using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Infrastructure.MySql.Repositories
{
	public class AgencyRepository : Repository<Agency>, IAgencyRepository
	{
		private readonly krovNadGlavomDbContext _context;

		public AgencyRepository(krovNadGlavomDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<Agency> GetAgencyByName(string name)
		{
			return await _context.Agencies.Where(a => a.Name == name).FirstOrDefaultAsync();
		}

		public async Task<List<Agency>> GetAgenciesByIds(List<string> ids)
		{
			return await _context.Agencies.Where(a => ids.Contains(a.Id)).ToListAsync();
		}
    }
}