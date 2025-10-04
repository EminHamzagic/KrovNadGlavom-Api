using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace krov_nad_glavom_api.Infrastructure.MongoDB.Repositories
{
    public class UserRepositoryMongo : RepositoryMongo<User>, IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepositoryMongo(krovNadGlavomMongoDbContext context) : base(context)
        {
            _users = context.Users;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            var user = await _users.Find(filter).FirstOrDefaultAsync();

            var doc = await _users.Find(Builders<User>.Filter.Empty).FirstOrDefaultAsync();
            Console.WriteLine(doc.ToJson());

            return await _users
                .Find(u => u.Email == email && !u.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetUsersByIds(List<string> ids)
        {
            return await _users
                .Find(u => ids.Contains(u.Id) && !u.IsDeleted)
                .ToListAsync();
        }

        public async Task<User> GetUserByCompanyId(string companyId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.ConstructionCompanyId, companyId);
            var user = await _users.Find(filter).FirstOrDefaultAsync();

            var doc = await _users.Find(Builders<User>.Filter.Empty).FirstOrDefaultAsync();
            Console.WriteLine(doc.ToJson());

            return await _users
                .Find(u => u.ConstructionCompanyId == companyId && !u.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByAgencyId(string agencyId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.AgencyId, agencyId);
            var user = await _users.Find(filter).FirstOrDefaultAsync();

            var doc = await _users.Find(Builders<User>.Filter.Empty).FirstOrDefaultAsync();
            Console.WriteLine(doc.ToJson());

            return await _users
                .Find(u => u.AgencyId == agencyId && !u.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public Task<(List<User> userPage, int totalCount, int totalPages)> GetUsersPage(QueryStringParameters parameters)
        {
            var usersQuery = _users
                .AsQueryable()
                .Where(c => c.IsDeleted == false && c.IsAllowed == parameters.IsAllowed && c.Role != "Admin");

            usersQuery = usersQuery.Filter(parameters).Sort(parameters);

            var totalCount = usersQuery.Count();
            parameters.checkOverflow(totalCount);

            var usersPage = usersQuery
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);

            return Task.FromResult((usersPage, totalCount, totalPages));
        }
    }
}