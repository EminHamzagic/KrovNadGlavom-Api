using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IContractRepository : IRepository<Contract>
    {
        Task<(List<Contract> contractPage, int totalCount, int totalPages)> GetContractsByUserId(string userId, QueryStringParameters parameters);
        Task<(List<Contract> contractPage, int totalCount, int totalPages)> GetContractsByAgencyId(string agencyId, QueryStringParameters parameters);
        Task<List<Contract>> GetContractsByApartmentIds(List<string> ids);
        Task<Contract> GetContractByApartmentId(string apartmentId);
        Task<Contract> GetContractByApartmentIdAndUserId(string apartmentId, string userId);
        Task<List<Contract>> GetLatePaymentContracts(User user);
        Task<List<Contract>> GetByUserId(string userId);
    }
}