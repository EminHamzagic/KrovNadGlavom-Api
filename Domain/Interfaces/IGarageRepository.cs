using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IGarageRepository : IRepository<Garage>
    {
        Task<Garage> GetGarageById(string id);
        Task<List<Garage>> GetGaragesByBuildingId(string buildingId);
        Task<int> GetBuildingGarageCount(string buildingId);
        Task<bool> IsSpotNumberFree(string spotNumber, Garage garage);
        Task<List<Garage>> GetGaragesByApartmentId(string apartmentId);
    }
}