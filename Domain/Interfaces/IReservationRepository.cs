using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<Reservation> GetReservationByUserId(string userId);
        Task<bool> IsApartmentReserved(string apartmentId);
    }
}