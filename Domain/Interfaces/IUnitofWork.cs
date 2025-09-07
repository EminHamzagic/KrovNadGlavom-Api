namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IUnitofWork : IDisposable
    {
        IUserRepository Users { get; }
        IUserSessionRepository UserSessions { get; }
        IAgencyRepository Agencies { get; }
        IAgencyRequestRepository AgencyRequests { get; }
        IApartmentRepository Apartments { get; }
        IBuildingRepository Buildings { get; }
        IConstructionCompanyRepository ConstructionCompanies { get; }
        IContractRepository Contracts { get; }
        IDiscountRequestRepository DiscountRequests { get; }
        IGarageRepository Garages { get; }
        IInstallmentRepository Installments { get; }
        IPriceListRepository PriceLists { get; }
        IReservationRepository Reservations { get; }
        IUserAgencyFollowRepository UserAgencyFollows { get; }

        Task Save();
    }
}