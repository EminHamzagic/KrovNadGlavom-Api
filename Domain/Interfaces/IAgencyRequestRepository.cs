using krov_nad_glavom_api.Data.DTO.AgencyRequest;
using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IAgencyRequestRepository : IRepository<AgencyRequest>
    {
        int GetAgencyBuildingCount(string agencyId);
        Task<int> GetAgencyApartmentCount(string agencyId);
        Task<bool> CheckForExistingRequest(AgencyRequestToAddDto dto);
        Task<List<AgencyRequest>> GetAgencyRequestsByAgencyId(string agencyId, string status);
        Task<List<AgencyRequest>> GetAgencyRequestsByCompanyId(string comapnyId, string status);
        Task<Agency> GetAgencyByBuildingId(string buildingId);
        Task<List<Agency>> GetAgenciesByBuildingIds(List<string> ids);
        Task<string> GetBuildingRequestStatus(string buildingId);
    }
}