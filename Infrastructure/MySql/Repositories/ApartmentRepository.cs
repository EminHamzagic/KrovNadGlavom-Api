using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.Apartment;
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

		public async Task<IQueryable<ApartmentWithBuildingDto>> GetAllAvailableApartmentsWithBuildings()
		{
			var reservedApartmentIds = await _context.Reservations
				.Where(r => r.ToDate > DateTime.Now)
				.Select(r => r.ApartmentId)
				.ToListAsync();

			var availableBuildingIds = await _context.AgencyRequests
				.Where(a => a.Status == "Approved")
				.Select(a => a.BuildingId)
				.ToListAsync();

			var query =
				from a in _context.Apartments
				join b in _context.Buildings on a.BuildingId equals b.Id
				where !reservedApartmentIds.Contains(a.Id)
					&& availableBuildingIds.Contains(a.BuildingId)
					&& !a.IsDeleted
					&& a.IsAvailable
				select new ApartmentWithBuildingDto
				{
					Apartment = a,
					Building = b
				};

			return query;
		}
    }
}