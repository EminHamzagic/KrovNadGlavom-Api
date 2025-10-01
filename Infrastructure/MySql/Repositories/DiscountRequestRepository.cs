using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.DiscountRequest;
using krov_nad_glavom_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Infrastructure.MySql.Repositories
{
	public class DiscountRequestRepository : Repository<DiscountRequest>, IDiscountRequestRepository
	{
		private readonly krovNadGlavomDbContext _context;

		public DiscountRequestRepository(krovNadGlavomDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<bool> CheckForExistingRequest(DiscountRequestToAddDto dto)
		{
			return !await _context.DiscountRequests.AnyAsync(d => d.UserId == dto.UserId && d.ApartmentId == dto.ApartmentId && d.Status == "Approved");
		}

		public async Task<List<DiscountRequest>> GetDiscountRequestsByUserId(string userId, string status)
		{
			return await _context.DiscountRequests.Where(d => d.UserId == userId && d.Status == status).OrderByDescending(d => d.CreatedAt).ToListAsync();
		}

		public async Task<List<DiscountRequest>> GetDiscountRequestsByAgencyId(string agencyId, string status)
		{
			return await _context.DiscountRequests.Where(d => d.AgencyId == agencyId && d.Status == status).OrderByDescending(d => d.CreatedAt).ToListAsync();
		}

		public async Task<List<DiscountRequest>> GetDiscountRequestsByCompanyId(string companyId, string status)
		{
			return await _context.DiscountRequests.Where(d => d.ConstructionCompanyId == companyId && d.Status == status).OrderByDescending(d => d.CreatedAt).ToListAsync();
		}

		public async Task<DiscountRequest> GetByApartmentId(string apartmentId)
        {
            return await _context.DiscountRequests.Where(c => c.ApartmentId == apartmentId).FirstOrDefaultAsync();
        }

		public async Task<DiscountRequest> GetByApartmentAndUserId(string apartmentId, string userId)
        {
            return await _context.DiscountRequests.Where(c => c.ApartmentId == apartmentId && c.UserId == userId).FirstOrDefaultAsync();
        }
	}
}