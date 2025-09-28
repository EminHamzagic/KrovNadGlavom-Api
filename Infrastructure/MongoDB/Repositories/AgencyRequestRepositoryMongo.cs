using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.AgencyRequest;
using krov_nad_glavom_api.Domain.Entities;
using MongoDB.Driver;

namespace krov_nad_glavom_api.Infrastructure.MongoDB.Repositories
{
    public class AgencyRequestRepositoryMongo : RepositoryMongo<AgencyRequest>, IAgencyRequestRepository
    {
        private readonly IMongoCollection<AgencyRequest> _agencyRequests;
        private readonly IMongoCollection<Apartment> _apartments;
        private readonly IMongoCollection<Building> _buildings;
        private readonly IMongoCollection<Agency> _agencies;

        public AgencyRequestRepositoryMongo(krovNadGlavomMongoDbContext context) : base(context)
        {
            _agencyRequests = context.AgencyRequests;
            _apartments = context.Apartments;
            _buildings = context.Buildings;
            _agencies = context.Agencies;
        }

        public Task<int> GetAgencyBuildingCount(string agencyId)
        {
            return Task.FromResult((int)_agencyRequests
                .Find(a => a.AgencyId == agencyId && !a.IsDeleted)
                .CountDocuments());
        }

        public async Task<int> GetAgencyApartmentCount(string agencyId)
        {
            var buildingIds = await _agencyRequests
                .Find(a => a.AgencyId == agencyId && !a.IsDeleted)
                .Project(a => a.BuildingId)
                .ToListAsync();

            return (int)await _apartments
                .Find(ap => buildingIds.Contains(ap.BuildingId))
                .CountDocumentsAsync();
        }

        public async Task<bool> CheckForExistingRequest(AgencyRequestToAddDto dto)
        {
            return await _agencyRequests
                .Find(ar => ar.AgencyId == dto.AgencyId && ar.BuildingId == dto.BuildingId && !ar.IsDeleted)
                .AnyAsync();
        }

        public async Task<List<AgencyRequest>> GetAgencyRequestsByAgencyId(string agencyId, string status)
        {
            return await _agencyRequests
                .Find(ar => ar.AgencyId == agencyId && !ar.IsDeleted && ar.Status == status)
                .ToListAsync();
        }

        public async Task<List<AgencyRequest>> GetAgencyRequestsByCompanyId(string companyId, string status)
        {
            var buildingIds = await _buildings
                .Find(b => b.CompanyId == companyId)
                .Project(b => b.Id)
                .ToListAsync();

            return await _agencyRequests
                .Find(ar => buildingIds.Contains(ar.BuildingId) && !ar.IsDeleted && ar.Status == status)
                .ToListAsync();
        }

        public async Task<Agency> GetAgencyByBuildingId(string buildingId)
        {
            var agencyRequest = await _agencyRequests
                .Find(a => a.BuildingId == buildingId && a.Status == "Approved" && !a.IsDeleted)
                .FirstOrDefaultAsync();

            if (agencyRequest == null)
                return null;

            return await _agencies
                .Find(a => a.Id == agencyRequest.AgencyId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Agency>> GetAgenciesByBuildingIds(List<string> ids)
        {
            var agencyIds = await _agencyRequests
                .Find(a => ids.Contains(a.BuildingId) && a.Status == "Approved" && !a.IsDeleted)
                .Project(a => a.AgencyId)
                .ToListAsync();

            return await _agencies
                .Find(a => agencyIds.Contains(a.Id))
                .ToListAsync();
        }

        public async Task<string> GetBuildingRequestStatus(string buildingId)
        {
            return await _agencyRequests
                .Find(a => a.BuildingId == buildingId && !a.IsDeleted)
                .Project(a => a.Status)
                .FirstOrDefaultAsync();
        }

        public async Task<decimal> GetAgencyCommissionForBuilding(string buildingId)
        {
            return await _agencyRequests
                .Find(a => a.BuildingId == buildingId && !a.IsDeleted)
                .Project(a => a.CommissionPercentage)
                .FirstOrDefaultAsync();
        }
    }
}