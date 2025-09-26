using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Domain.Entities;
using MongoDB.Driver;

namespace krov_nad_glavom_api.Infrastructure.MongoDB.Repositories
{
    public class AgencyRepositoryMongo : RepositoryMongo<Agency>, IAgencyRepository
    {
        private readonly IMongoCollection<Agency> _collection;

        public AgencyRepositoryMongo(krovNadGlavomMongoDbContext context) : base(context)
        {
            _collection = context.Agencies;
        }

        public async Task<Agency> GetAgencyByName(string name)
        {
            return await _collection
                .Find(a => a.Name == name)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Agency>> GetAgenciesByIds(List<string> ids)
        {
            return await _collection
                .Find(a => ids.Contains(a.Id))
                .ToListAsync();
        }

        public Task<(List<Agency> agenciesPage, int totalCount, int totalPages)> GetAgenciesQuery(QueryStringParameters parameters)
		{
			var agenciesQuery = _collection.AsQueryable().Where(a => a.Id != null);
			agenciesQuery = agenciesQuery.Filter(parameters).Sort(parameters);

			var totalCount = agenciesQuery.Count();
			parameters.checkOverflow(totalCount);

			var agenciesPage = agenciesQuery
				.Skip((parameters.PageNumber - 1) * parameters.PageSize)
				.Take(parameters.PageSize)
				.ToList();

			var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);
				
			return Task.FromResult((agenciesPage, totalCount, totalPages));
		}
    }
}