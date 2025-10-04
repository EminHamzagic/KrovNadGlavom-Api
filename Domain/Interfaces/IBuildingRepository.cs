using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IBuildingRepository : IRepository<Building>
    {
        Task<Building> GetBuildingByParcel(string parcelNum);
        Task<bool> CanAddApartment(ApartmentToAddDto apartmentToAddDto);
        Task<Building> GetBuildingById(string id);
        Task<List<Building>> GetBuildingsByIds(List<string> ids);
        Task<(List<Building> buildingsPage, int totalCount, int totalPages)> GetAllValidBuildings(string agencyId, QueryStringParameters parameters);
        Task<(List<Building> buildingsPage, int totalCount, int totalPages)> GetCompanyBuildings(string companyId, QueryStringParameters parameters);
        Task<(List<Building> buildingsPage, int totalCount, int totalPages)> GetBuildingsPage(QueryStringParameters parameters);
    }
}