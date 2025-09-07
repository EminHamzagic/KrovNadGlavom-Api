using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IConstructionCompanyRepository : IRepository<ConstructionCompany>
    {
        Task<ConstructionCompany> GetCompanyByName(string name);
    }
}