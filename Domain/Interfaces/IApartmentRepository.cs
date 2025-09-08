using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IApartmentRepository : IRepository<Apartment>
    {
        Task<List<Apartment>> GetApartmentsByBuildingId(string buildingId);
        Task<Apartment> GetApartmentById(string id);
        Task<List<Apartment>> GetApartmentsByIds(List<string> ids);
    }
}