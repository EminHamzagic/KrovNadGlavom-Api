using krov_nad_glavom_api.Application.Interfaces;
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
                .Find(u => u.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetUsersByIds(List<string> ids)
        {
            return await _users
                .Find(u => ids.Contains(u.Id))
                .ToListAsync();
        }

        public async Task<User> GetUserByCompanyId(string companyId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.ConstructionCompanyId, companyId);
            var user = await _users.Find(filter).FirstOrDefaultAsync();

            var doc = await _users.Find(Builders<User>.Filter.Empty).FirstOrDefaultAsync();
            Console.WriteLine(doc.ToJson());

            return await _users
                .Find(u => u.ConstructionCompanyId == companyId)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByAgencyId(string agencyId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.AgencyId, agencyId);
            var user = await _users.Find(filter).FirstOrDefaultAsync();

            var doc = await _users.Find(Builders<User>.Filter.Empty).FirstOrDefaultAsync();
            Console.WriteLine(doc.ToJson());

            return await _users
                .Find(u => u.AgencyId == agencyId)
                .FirstOrDefaultAsync();
        }
    }
}