using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Infrastructure.MySql.Repositories
{
	public class GarageRepository : Repository<Garage>, IGarageRepository
	{
		private readonly krovNadGlavomDbContext _context;

		public GarageRepository(krovNadGlavomDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<Garage> GetGarageById(string id)
		{
			return await _context.Garages.Where(a => a.Id == id).FirstOrDefaultAsync();
		}

		public async Task<List<Garage>> GetGaragesByBuildingId(string buildingId)
		{
			return await _context.Garages.Where(a => a.BuildingId == buildingId).ToListAsync();
		}

		public Task<int> GetBuildingGarageCount(string buildingId)
		{
			return Task.FromResult(_context.Garages.Where(a => a.BuildingId == buildingId).Count());
		}

		public async Task<bool> IsSpotNumberFree(string spotNumber, Garage garage)
		{
			return !await _context.Garages.AnyAsync(g => g.BuildingId == garage.BuildingId && g.SpotNumber == spotNumber && g.Id != garage.Id);
		}
		
		public async Task<List<Garage>> GetGaragesByApartmentId(string apartmentId)
		{
			return await _context.Garages.Where(a => a.ApartmentId == apartmentId).ToListAsync();
		}
    }
}