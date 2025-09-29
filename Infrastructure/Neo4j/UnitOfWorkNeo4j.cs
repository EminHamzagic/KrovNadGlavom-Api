using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Infrastructure.Neo4j;
using krov_nad_glavom_api.Infrastructure.Neo4j.Repositories;

namespace krov_nad_glavom_api.Application
{
    public class UnitOfWorkNeo4j : IUnitOfWork
    {
        private readonly krovNadGlavomNeo4jDbContext _context;

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

        public UnitOfWorkNeo4j(krovNadGlavomNeo4jDbContext context)
        {
            _context = context;

            Users = new UserRepositoryNeo4j(_context);
            UserSessions = new UserSessionRepositoryNeo4j(_context);
            Agencies = new AgencyRepositoryNeo4j(_context);
            AgencyRequests = new AgencyRequestRepositoryNeo4j(_context);
            Apartments = new ApartmentRepositoryNeo4j(_context);
            Buildings = new BuildingRepositoryNeo4j(_context);
            ConstructionCompanies = new ConstructionCompanyRepositoryNeo4j(_context);
            Contracts = new ContractRepositoryNeo4j(_context);
            DiscountRequests = new DiscountRequestRepositoryNeo4j(_context);
            Garages = new GarageRepositoryNeo4j(_context);
            Installments = new InstallmentRepositoryNeo4j(_context);
            PriceLists = new PriceListRepositoryNeo4j(_context);
            Reservations = new ReservationRepositoryNeo4j(_context);
            UserAgencyFollows = new UserAgencyFollowRepositoryNeo4j(_context);
        }

        public Task Save() => Task.CompletedTask;

        public void Dispose()
        {
        }
    }
}
