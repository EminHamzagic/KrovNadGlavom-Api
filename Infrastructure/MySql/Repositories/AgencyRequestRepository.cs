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
			return _context.AgencyRequests.Where(a => a.AgencyId == agencyId && !a.IsDeleted).Count();
		}

		public async Task<int> GetAgencyApartmentCount(string agencyId)
		{
			var buildingIds = await _context.AgencyRequests.Where(a => a.AgencyId == agencyId && !a.IsDeleted).Select(a => a.BuildingId).ToListAsync();

			return await _context.Apartments
				.Where(ap => buildingIds.Contains(ap.BuildingId))
				.CountAsync();
		}

		public async Task<bool> CheckForExistingRequest(AgencyRequestToAddDto dto)
		{
			return await _context.AgencyRequests.AnyAsync(ar => ar.AgencyId == dto.AgencyId && ar.BuildingId == dto.BuildingId && !ar.IsDeleted);
		}

		public async Task<List<AgencyRequest>> GetAgencyRequestsByAgencyId(string agencyId, string status)
		{
			return await _context.AgencyRequests.Where(ar => ar.AgencyId == agencyId && !ar.IsDeleted && ar.Status == status).ToListAsync();
		}

		public async Task<List<AgencyRequest>> GetAgencyRequestsByCompanyId(string comapnyId, string status)
		{
			var buildingIds = await _context.Buildings.Where(b => b.CompanyId == comapnyId).Select(b => b.Id).ToListAsync();
			return await _context.AgencyRequests.Where(ar => buildingIds.Contains(ar.BuildingId) && !ar.IsDeleted &&  ar.Status == status).ToListAsync();
		}

		public async Task<Agency> GetAgencyByBuildingId(string buildingId)
		{
			var agencyRequest = await _context.AgencyRequests.Where(a => a.BuildingId == buildingId && a.Status == "Approved"  && !a.IsDeleted).FirstOrDefaultAsync();
			return await _context.Agencies.Where(a => a.Id == agencyRequest.AgencyId).FirstOrDefaultAsync();
		}

		public async Task<List<Agency>> GetAgenciesByBuildingIds(List<string> ids)
		{
			var agencyIds = await _context.AgencyRequests.Where(a => ids.Contains(a.BuildingId) && a.Status == "Approved" && !a.IsDeleted).Select(a => a.AgencyId).ToListAsync();
			return await _context.Agencies.Where(a => agencyIds.Contains(a.Id)).ToListAsync();
		}

		public async Task<string> GetBuildingRequestStatus(string buildingId)
		{
			return await _context.AgencyRequests.Where(a => a.BuildingId == buildingId && !a.IsDeleted).Select(a => a.Status).FirstOrDefaultAsync();
		}
	}
}