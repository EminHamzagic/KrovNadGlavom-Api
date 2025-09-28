using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IContractRepository : IRepository<Contract>
    {
        Task<(List<Contract> contractPage, int totalCount, int totalPages)> GetContractsByUserId(string userId, QueryStringParameters parameters);
        Task<(List<Contract> contractPage, int totalCount, int totalPages)> GetContractsByAgencyId(string agencyId, QueryStringParameters parameters);
        Task<Contract> GetContractsByApartmentId(string apartmentId);
    }
}