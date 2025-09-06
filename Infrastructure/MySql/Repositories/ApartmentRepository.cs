using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Infrastructure.MySql.Repositories
{
	public class ApartmentRepository : Repository<Apartment>, IApartmentRepository
	{
		private readonly krovNadGlavomDbContext _context;

		public ApartmentRepository(krovNadGlavomDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<Apartment>> GetApartmentsByBuildingId(string buildingId)
		{
			return await _context.Apartments.Where(a => a.BuildingId == buildingId).ToListAsync();
		}

		public async Task<Apartment> GetApartmentById(string id)
		{
			return await _context.Apartments.Where(a => a.Id == id && !a.IsDeleted).FirstOrDefaultAsync();
		}
    }
}