using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Infrastructure.MongoDB.Repositories;

namespace krov_nad_glavom_api.Infrastructure.MongoDB
{
    public class UnitOfWorkMongo : IUnitofWork
    {
        private readonly krovNadGlavomMongoDbContext _context;

        public IUserRepository Users { get; set; }
        public IUserSessionRepository UserSessions { get; set; }
        public IAgencyRepository Agencies { get; set; }
        public IAgencyRequestRepository AgencyRequests { get; set; }
        public IApartmentRepository Apartments { get; set; }
        public IBuildingRepository Buildings { get; set; }
        public IConstructionCompanyRepository ConstructionCompanies { get; set; }
        public IContractRepository Contracts { get; set; }
        public IDiscountRequestRepository DiscountRequests { get; set; }
        public IGarageRepository Garages { get; set; }
        public IInstallmentRepository Installments { get; set; }
        public IPriceListRepository PriceLists { get; set; }
        public IReservationRepository Reservations { get; set; }
        public IUserAgencyFollowRepository UserAgencyFollows { get; set; }

        public UnitOfWorkMongo(krovNadGlavomMongoDbContext context)
        {
            _context = context;

            Users = new UserRepositoryMongo(_context);
            UserSessions = new UserSessionRepositoryMongo(_context);
            Agencies = new AgencyRepositoryMongo(_context);
            AgencyRequests = new AgencyRequestRepositoryMongo(_context);
            Apartments = new ApartmentRepositoryMongo(_context);
            Buildings = new BuildingRepositoryMongo(_context);
            ConstructionCompanies = new ConstructionCompanyRepositoryMongo(_context);
            Contracts = new ContractRepositoryMongo(_context);
            DiscountRequests = new DiscountRequestRepositoryMongo(_context);
            Garages = new GarageRepositoryMongo(_context);
            Installments = new InstallmentRepositoryMongo(_context);
            PriceLists = new PriceListRepositoryMongo(_context);
            Reservations = new ReservationRepositoryMongo(_context);
            UserAgencyFollows = new UserAgencyFollowRepositoryMongo(_context);
        }

        public async Task Save()
        {
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            
        }
    }
}