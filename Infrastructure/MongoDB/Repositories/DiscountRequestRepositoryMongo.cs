using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.DiscountRequest;
using krov_nad_glavom_api.Domain.Entities;
using MongoDB.Driver;

namespace krov_nad_glavom_api.Infrastructure.MongoDB.Repositories
{
    public class DiscountRequestRepositoryMongo : RepositoryMongo<DiscountRequest>, IDiscountRequestRepository
    {
        private readonly IMongoCollection<DiscountRequest> _discountRequests;

        public DiscountRequestRepositoryMongo(krovNadGlavomMongoDbContext context) : base(context)
        {
            _discountRequests = context.DiscountRequests;
        }

        public async Task<bool> CheckForExistingRequest(DiscountRequestToAddDto dto)
        {
            var exists = await _discountRequests
                .Find(d => d.UserId == dto.UserId && d.ApartmentId == dto.ApartmentId && d.Status == "Approved")
                .AnyAsync();

            return !exists;
        }

        public async Task<List<DiscountRequest>> GetDiscountRequestsByUserId(string userId, string status)
        {
            return await _discountRequests
                .Find(d => d.UserId == userId && d.Status == status)
                .ToListAsync();
        }

        public async Task<List<DiscountRequest>> GetDiscountRequestsByAgencyId(string agencyId, string status)
        {
            return await _discountRequests
                .Find(d => d.AgencyId == agencyId && d.Status == status)
                .ToListAsync();
        }

        public async Task<List<DiscountRequest>> GetDiscountRequestsByCompanyId(string companyId, string status)
        {
            return await _discountRequests
                .Find(d => d.ConstructionCompanyId == companyId && d.Status == status)
                .ToListAsync();
        }

        public async Task<DiscountRequest> GetByApartmentId(string apartmentId)
        {
            return await _discountRequests
                .Find(c => c.ApartmentId == apartmentId)
                .FirstOrDefaultAsync();
        }

        public async Task<DiscountRequest> GetByApartmentAndUserId(string apartmentId, string userId)
        {
            return await _discountRequests
                .Find(c => c.ApartmentId == apartmentId && c.UserId == userId)
                .FirstOrDefaultAsync();
        }
    }
}