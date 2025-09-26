using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IApartmentRepository : IRepository<Apartment>
    {
        Task<List<Apartment>> GetApartmentsByBuildingId(string buildingId);
        Task<Apartment> GetApartmentById(string id);
        Task<List<Apartment>> GetApartmentsByIds(List<string> ids);
        Task<(List<ApartmentWithBuildingDto> apartmentsPage, int totalCount, int totalPages)> GetAllAvailableApartmentsWithBuildings(QueryStringParameters parameters);
    }
}