using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Domain.Entities;
using MongoDB.Driver;

namespace krov_nad_glavom_api.Infrastructure.MongoDB.Repositories
{
    public class ApartmentRepositoryMongo : RepositoryMongo<Apartment>, IApartmentRepository
    {
        private readonly IMongoCollection<Apartment> _apartments;
        private readonly IMongoCollection<Building> _buildings;
        private readonly IMongoCollection<Reservation> _reservations;
        private readonly IMongoCollection<AgencyRequest> _agencyRequests;

        public ApartmentRepositoryMongo(krovNadGlavomMongoDbContext context) : base(context)
        {
            _apartments = context.Apartments;
            _buildings = context.Buildings;
            _reservations = context.Reservations;
            _agencyRequests = context.AgencyRequests;
        }

        public async Task<List<Apartment>> GetApartmentsByBuildingId(string buildingId)
        {
            return await _apartments
                .Find(a => a.BuildingId == buildingId && a.IsAvailable)
                .ToListAsync();
        }

        public async Task<Apartment> GetApartmentById(string id)
        {
            return await _apartments
                .Find(a => a.Id == id && !a.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Apartment>> GetApartmentsByIds(List<string> ids)
        {
            return await _apartments
                .Find(a => ids.Contains(a.Id) && !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<(List<ApartmentWithBuildingDto> apartmentsPage, int totalCount, int totalPages)> GetAllAvailableApartmentsWithBuildings(QueryStringParameters parameters)
        {
            // 1. Get reserved apartment IDs
            var reservedApartmentIds = await _reservations
                .Find(r => r.ToDate > DateTime.Now)
                .Project(r => r.ApartmentId)
                .ToListAsync();

            // 2. Get approved building IDs
            var availableBuildingIds = await _agencyRequests
                .Find(a => a.Status == "Approved" && !a.IsDeleted)
                .Project(a => a.BuildingId)
                .ToListAsync();

            // 3. Get available apartments
            var apartments = await _apartments
                .Find(a =>
                    !reservedApartmentIds.Contains(a.Id) &&
                    availableBuildingIds.Contains(a.BuildingId) &&
                    !a.IsDeleted &&
                    a.IsAvailable
                )
                .ToListAsync();

            // 4. Load related buildings
            var buildingIds = apartments.Select(a => a.BuildingId).Distinct().ToList();
            var buildings = await _buildings
                .Find(b => buildingIds.Contains(b.Id))
                .ToListAsync();

            // 5. Join in-memory
            var apartmentsQuery = apartments
                .Join(buildings,
                      a => a.BuildingId,
                      b => b.Id,
                      (a, b) => new ApartmentWithBuildingDto
                      {
                          Apartment = a,
                          Building = b
                      })
                .AsQueryable();

            apartmentsQuery = apartmentsQuery.Filter(parameters).Sort(parameters);

            var totalCount = apartmentsQuery.Count();
            parameters.checkOverflow(totalCount);

            var apartmentsPage = apartmentsQuery
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

			var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);

			return (apartmentsPage, totalCount, totalPages);
        }
    }
}