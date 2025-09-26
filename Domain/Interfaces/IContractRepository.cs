using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IContractRepository : IRepository<Contract>
    {
        IQueryable<Contract> GetContractsByUserId(string userId, string status);
        IQueryable<Contract> GetContractsByAgencyId(string agencyId, string status);
        Task<Contract> GetContractsByApartmentId(string apartmentId);
    }
}