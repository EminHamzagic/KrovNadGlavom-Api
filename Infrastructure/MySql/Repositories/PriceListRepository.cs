using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Infrastructure.MySql.Repositories
{
	public class PriceListRepository : Repository<PriceList>, IPriceListRepository
	{
		private readonly krovNadGlavomDbContext _context;

		public PriceListRepository(krovNadGlavomDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<PriceList> GetPriceListByBuildingId(string buildingId)
		{
			return await _context.PriceLists.Where(p => p.BuildingId == buildingId).FirstOrDefaultAsync();
		}

		public async Task<List<PriceList>> GetPriceListsByBuildingIds(List<string> ids)
        {
            return await _context.PriceLists.Where(u => ids.Contains(u.BuildingId)).ToListAsync();
        }
    }
}