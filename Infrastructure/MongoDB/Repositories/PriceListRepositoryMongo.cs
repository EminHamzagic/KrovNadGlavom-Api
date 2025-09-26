using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MongoDB.Driver;

namespace krov_nad_glavom_api.Infrastructure.MongoDB.Repositories
{
    public class PriceListRepositoryMongo : RepositoryMongo<PriceList>, IPriceListRepository
    {
        private readonly IMongoCollection<PriceList> _priceLists;

        public PriceListRepositoryMongo(krovNadGlavomMongoDbContext context) : base(context)
        {
            _priceLists = context.PriceLists;
        }

        public async Task<PriceList> GetPriceListByBuildingId(string buildingId)
        {
            return await _priceLists
                .Find(p => p.BuildingId == buildingId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<PriceList>> GetPriceListsByBuildingIds(List<string> ids)
        {
            return await _priceLists
                .Find(u => ids.Contains(u.BuildingId))
                .ToListAsync();
        }
    }
}