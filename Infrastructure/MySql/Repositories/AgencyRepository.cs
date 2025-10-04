using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
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

		public async Task<(List<Agency> agenciesPage, int totalCount, int totalPages)> GetAgenciesQuery(QueryStringParameters parameters)
		{
			var agenciesQuery = _context.Agencies.Where(a => a.IsAllowed).AsQueryable();
			agenciesQuery = agenciesQuery.Filter(parameters).Sort(parameters);

			var totalCount = agenciesQuery.Count();
			parameters.checkOverflow(totalCount);

			var agenciesPage = await agenciesQuery
				.Skip((parameters.PageNumber - 1) * parameters.PageSize)
				.Take(parameters.PageSize)
				.ToListAsync();

			var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);
				
			return (agenciesPage, totalCount, totalPages);
		}
    }
}