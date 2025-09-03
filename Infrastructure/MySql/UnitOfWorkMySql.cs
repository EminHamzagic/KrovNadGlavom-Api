using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Infrastructure.MySql;
using krov_nad_glavom_api.Infrastructure.MySql.Repositories;

namespace krov_nad_glavom_api.Application
{
    public class UnitOfWorkMySql : IUnitofWork
    {
        private readonly krovNadGlavomDbContext _context;

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

        public UnitOfWorkMySql(krovNadGlavomDbContext context)
        {
            _context = context;

            Users = new UserRepository(_context);
            UserSessions = new UserSessionRepository(_context);
            Agencies = new AgencyRepository(_context);
            AgencyRequests = new AgencyRequestRepository(_context);
            Apartments = new ApartmentRepository(_context);
            Buildings = new BuildingRepository(_context);
            ConstructionCompanies = new ConstructionCompanyRepository(_context);
            Contracts = new ContractRepository(_context);
            DiscountRequests = new DiscountRequestRepository(_context);
            Garages = new GarageRepository(_context);
            Installments = new InstallmentRepository(_context);
            PriceLists = new PriceListRepository(_context);
            Reservations = new ReservationRepository(_context);
            UserAgencyFollows = new UserAgencyFollowRepository(_context);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        
        public async void Dispose()
        {
            await _context.DisposeAsync();
        }
    }
}