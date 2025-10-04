using krov_nad_glavom_api.Data.DTO.DiscountRequest;
using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IDiscountRequestRepository : IRepository<DiscountRequest>
    {
        Task<bool> CheckForExistingRequest(DiscountRequestToAddDto dto);
        Task<List<DiscountRequest>> GetDiscountRequestsByUserId(string userId, string status);
        Task<List<DiscountRequest>> GetDiscountRequestsByAgencyId(string agencyId, string status);
        Task<List<DiscountRequest>> GetDiscountRequestsByCompanyId(string companyId, string status);
        Task<DiscountRequest> GetByApartmentId(string apartmentId);
        Task<DiscountRequest> GetByApartmentAndUserId(string apartmentId, string userId);
        Task<List<DiscountRequest>> GetByUserId(string userId);
    }
}