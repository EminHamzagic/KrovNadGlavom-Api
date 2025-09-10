using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IAgencyRepository : IRepository<Agency>
    {
        Task<Agency> GetAgencyByName(string name);
        Task<List<Agency>> GetAgenciesByIds(List<string> ids);
    }
}