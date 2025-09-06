using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.AgencyRequest;
using krov_nad_glavom_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Infrastructure.MySql.Repositories
{
	public class AgencyRequestRepository : Repository<AgencyRequest>, IAgencyRequestRepository
	{
		private readonly krovNadGlavomDbContext _context;

		public AgencyRequestRepository(krovNadGlavomDbContext context) : base(context)
		{
			_context = context;
		}

		public int GetAgencyBuildingCount(string agencyId)
		{
			return _context.AgencyRequests.Where(a => a.AgencyId == agencyId).Count();
		}

		public async Task<int> GetAgencyApartmentCount(string agencyId)
		{
			var buildingIds = await _context.AgencyRequests.Where(a => a.AgencyId == agencyId).Select(a => a.BuildingId).ToListAsync();

			return await _context.Apartments
				.Where(ap => buildingIds.Contains(ap.BuildingId))
				.CountAsync();
		}

		public async Task<bool> CheckForExistingRequest(AgencyRequestToAddDto dto)
		{
			return await _context.AgencyRequests.AnyAsync(ar => ar.AgencyId == dto.AgencyId && ar.BuildingId == dto.BuildingId);
		}
	}
}