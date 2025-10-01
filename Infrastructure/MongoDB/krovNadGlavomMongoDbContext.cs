using krov_nad_glavom_api.Domain.Entities;
using MongoDB.Driver;

namespace krov_nad_glavom_api.Infrastructure.MongoDB
{
    public class krovNadGlavomMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public krovNadGlavomMongoDbContext(string connectionString)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("KrovNadGlavom");
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("User");
        public IMongoCollection<UserSession> UserSessions => _database.GetCollection<UserSession>("UserSession");
        public IMongoCollection<ConstructionCompany> ConstructionCompanies => _database.GetCollection<ConstructionCompany>("ConstructionCompany");
        public IMongoCollection<Building> Buildings => _database.GetCollection<Building>("Building");
        public IMongoCollection<PriceList> PriceLists => _database.GetCollection<PriceList>("PriceList");
        public IMongoCollection<Apartment> Apartments => _database.GetCollection<Apartment>("Apartment");
        public IMongoCollection<Garage> Garages => _database.GetCollection<Garage>("Garage");
        public IMongoCollection<Agency> Agencies => _database.GetCollection<Agency>("Agency");
        public IMongoCollection<AgencyRequest> AgencyRequests => _database.GetCollection<AgencyRequest>("AgencyRequest");
        public IMongoCollection<UserAgencyFollow> UserAgencyFollows => _database.GetCollection<UserAgencyFollow>("UserAgencyFollow");
        public IMongoCollection<Reservation> Reservations => _database.GetCollection<Reservation>("Reservation");
        public IMongoCollection<DiscountRequest> DiscountRequests => _database.GetCollection<DiscountRequest>("DiscountRequest");
        public IMongoCollection<Contract> Contracts => _database.GetCollection<Contract>("Contract");
        public IMongoCollection<Installment> Installments => _database.GetCollection<Installment>("Installment");
        public IMongoCollection<Notification> Notifications => _database.GetCollection<Notification>("Notification");
        
        public IMongoCollection<T> GetCollection<T>()
        {
            return _database.GetCollection<T>(typeof(T).Name);
        }
    }
}