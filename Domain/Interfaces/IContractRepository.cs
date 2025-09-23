using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IContractRepository : IRepository<Contract>
    {
        Task<List<Contract>> GetContractsByUserId(string userId);
        Task<List<Contract>> GetContractsByAgencyId(string agencyId);
        Task<Contract> GetContractsByApartmentId(string apartmentId);
    }
}