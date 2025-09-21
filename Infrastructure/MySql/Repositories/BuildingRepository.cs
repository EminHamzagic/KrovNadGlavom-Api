using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Infrastructure.MySql.Repositories
{
	public class BuildingRepository : Repository<Building>, IBuildingRepository
	{
		private readonly krovNadGlavomDbContext _context;

		public BuildingRepository(krovNadGlavomDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<Building> GetBuildingById(string id)
		{
			return await _context.Buildings.Where(u => u.Id == id && !u.IsDeleted).FirstOrDefaultAsync();
		}

		public async Task<Building> GetBuildingByParcel(string parcelNum)
		{
			return await _context.Buildings.Where(u => u.ParcelNumber == parcelNum && !u.IsDeleted).FirstOrDefaultAsync();
		}

		public async Task<List<Building>> GetBuildingsByCompanyId(string comapnyId)
		{
			return await _context.Buildings.Where(u => u.CompanyId == comapnyId && !u.IsDeleted).OrderBy(b => b.CreatedAt).ToListAsync();
		}

		public async Task<bool> CanAddApartment(ApartmentToAddDto apartmentToAddDto)
		{
			var building = await _context.Buildings.Where(b => b.Id == apartmentToAddDto.BuildingId).FirstOrDefaultAsync();
			var totalArea = await _context.Apartments
				.Where(a => a.BuildingId == apartmentToAddDto.BuildingId
						&& a.Floor == apartmentToAddDto.Floor)
				.SumAsync(a => a.Area);

			if (building.Area - (totalArea + apartmentToAddDto.Area) > 0)
				return true;

			return false;
		}

		public async Task<List<Building>> GetBuildingsByIds(List<string> ids)
		{
			return await _context.Buildings.Where(u => ids.Contains(u.Id) && !u.IsDeleted).ToListAsync();
		}
    }
}