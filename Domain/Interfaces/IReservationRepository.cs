using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<Reservation> GetReservationByUserId(string userId);
        Task<Reservation> GetByApartmentId(string apartmentId);
        Task<bool> CanUserReserve(string userId, string agencyId);
        Task<Reservation> GetUserReservation(string userId);
    }
}