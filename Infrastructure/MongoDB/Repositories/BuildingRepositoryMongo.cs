using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Domain.Entities;
using MongoDB.Driver;

namespace krov_nad_glavom_api.Infrastructure.MongoDB.Repositories
{
    public class BuildingRepositoryMongo : RepositoryMongo<Building>, IBuildingRepository
    {
        private readonly IMongoCollection<Building> _buildings;
        private readonly IMongoCollection<Apartment> _apartments;
        private readonly IMongoCollection<AgencyRequest> _agencyRequests;

        public BuildingRepositoryMongo(krovNadGlavomMongoDbContext context) : base(context)
        {
            _buildings = context.Buildings;
            _apartments = context.Apartments;
            _agencyRequests = context.AgencyRequests;
        }

        public async Task<Building> GetBuildingById(string id)
        {
            return await _buildings
                .Find(u => u.Id == id && !u.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<Building> GetBuildingByParcel(string parcelNum)
        {
            return await _buildings
                .Find(u => u.ParcelNumber == parcelNum && !u.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CanAddApartment(ApartmentToAddDto apartmentToAddDto)
        {
            var building = await _buildings
                .Find(b => b.Id == apartmentToAddDto.BuildingId)
                .FirstOrDefaultAsync();

            if (building == null) return false;

            var totalArea = await _apartments
                .Find(a => a.BuildingId == apartmentToAddDto.BuildingId && a.Floor == apartmentToAddDto.Floor)
                .Project(a => a.Area)
                .ToListAsync();

            var sum = totalArea.Sum();

            return building.Area - (sum + apartmentToAddDto.Area) > 0;
        }

        public async Task<List<Building>> GetBuildingsByIds(List<string> ids)
        {
            return await _buildings
                .Find(u => ids.Contains(u.Id) && !u.IsDeleted)
                .ToListAsync();
        }

        public async Task<(List<Building> buildingsPage, int totalCount, int totalPages)> GetAllValidBuildings(string agencyId, QueryStringParameters parameters)
        {
            var validBuildings = await _agencyRequests
                .Find(a => a.Status == "Approved" && !a.IsDeleted)
                .Project(a => a.BuildingId)
                .ToListAsync();

            var buildingsQuery = _buildings
                .AsQueryable()
                .Where(u => !validBuildings.Contains(u.Id) && !u.IsDeleted);

            buildingsQuery = buildingsQuery.Filter(parameters).Sort(parameters);

            var totalCount = buildingsQuery.Count();

            parameters.checkOverflow(totalCount);

            var buildingsPage = buildingsQuery
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);

            return (buildingsPage, totalCount, totalPages);
        }

        public Task<(List<Building> buildingsPage, int totalCount, int totalPages)> GetCompanyBuildings(string companyId, QueryStringParameters parameters)
        {
            var buildingsQuery = _buildings
                .AsQueryable()
                .Where(u => u.CompanyId == companyId && !u.IsDeleted);

            buildingsQuery = buildingsQuery.Filter(parameters).Sort(parameters);

            var totalCount = buildingsQuery.Count();

            parameters.checkOverflow(totalCount);

            var buildingsPage = buildingsQuery
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);

            return Task.FromResult((buildingsPage, totalCount, totalPages));
        }

        public Task<(List<Building> buildingsPage, int totalCount, int totalPages)> GetBuildingsPage(QueryStringParameters parameters)
		{
			var buildingsQuery = _buildings
                .AsQueryable()
                .Where(u => !u.IsDeleted);

            buildingsQuery = buildingsQuery.Filter(parameters).Sort(parameters);

            var totalCount = buildingsQuery.Count();

            parameters.checkOverflow(totalCount);

            var buildingsPage = buildingsQuery
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);

            return Task.FromResult((buildingsPage, totalCount, totalPages));
		}
    }
}