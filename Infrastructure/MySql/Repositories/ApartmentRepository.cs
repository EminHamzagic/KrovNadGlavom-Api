using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
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
			return await _context.Apartments.Where(a => a.BuildingId == buildingId && a.IsAvailable).ToListAsync();
		}

		public async Task<Apartment> GetApartmentById(string id)
		{
			return await _context.Apartments.Where(a => a.Id == id && !a.IsDeleted).FirstOrDefaultAsync();
		}

		public async Task<List<Apartment>> GetApartmentsByIds(List<string> ids)
		{
			return await _context.Apartments.Where(a => ids.Contains(a.Id) && !a.IsDeleted).ToListAsync();
		}

		public async Task<List<Apartment>> GetAllAvailableApartments()
		{
			var reservedApartmentIds = await _context.Reservations.Where(r => r.ToDate > DateTime.Now).Select(r => r.ApartmentId).ToListAsync();
			return await _context.Apartments.Where(a => !reservedApartmentIds.Contains(a.Id) && !a.IsDeleted && a.IsAvailable).ToListAsync();
		}
    }
}