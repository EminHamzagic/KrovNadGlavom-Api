using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MongoDB.Driver;

namespace krov_nad_glavom_api.Infrastructure.MongoDB.Repositories
{
    public class GarageRepositoryMongo : RepositoryMongo<Garage>, IGarageRepository
    {
        private readonly IMongoCollection<Garage> _garages;

        public GarageRepositoryMongo(krovNadGlavomMongoDbContext context) : base(context)
        {
            _garages = context.Garages;
        }

        public async Task<Garage> GetGarageById(string id)
        {
            return await _garages
                .Find(a => a.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Garage>> GetGaragesByBuildingId(string buildingId)
        {
            return await _garages
                .Find(a => a.BuildingId == buildingId)
                .ToListAsync();
        }

        public Task<int> GetBuildingGarageCount(string buildingId)
        {
            return Task.FromResult((int)_garages.CountDocuments(a => a.BuildingId == buildingId));
        }

        public async Task<bool> IsSpotNumberFree(string spotNumber, Garage garage)
        {
            var exists = await _garages
                .Find(g => g.BuildingId == garage.BuildingId && g.SpotNumber == spotNumber && g.Id != garage.Id)
                .AnyAsync();

            return !exists;
        }

        public async Task<List<Garage>> GetGaragesByApartmentId(string apartmentId)
        {
            return await _garages
                .Find(a => a.ApartmentId == apartmentId)
                .ToListAsync();
        }
    }
}