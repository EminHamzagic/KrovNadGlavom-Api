using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MongoDB.Driver;

namespace krov_nad_glavom_api.Infrastructure.MongoDB.Repositories
{
    public class ConstructionCompanyRepositoryMongo : RepositoryMongo<ConstructionCompany>, IConstructionCompanyRepository
    {
        private readonly IMongoCollection<ConstructionCompany> _companies;

        public ConstructionCompanyRepositoryMongo(krovNadGlavomMongoDbContext context) : base(context)
        {
            _companies = context.ConstructionCompanies;
        }

        public async Task<ConstructionCompany> GetCompanyByName(string name)
        {
            return await _companies
                .Find(u => u.Name == name)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ConstructionCompany>> GetCompaniesByIds(List<string> ids)
        {
            return await _companies
                .Find(u => ids.Contains(u.Id))
                .ToListAsync();
        }

        public async Task<ConstructionCompany> GetCompanyById(string id)
        {
            return await _companies
                .Find(c => c.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}