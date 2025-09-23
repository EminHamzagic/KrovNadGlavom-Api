using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IPriceListRepository : IRepository<PriceList>
    {
        Task<PriceList> GetPriceListByBuildingId(string buildingId);
        Task<List<PriceList>> GetPriceListsByBuildingIds(List<string> ids);
    }
}