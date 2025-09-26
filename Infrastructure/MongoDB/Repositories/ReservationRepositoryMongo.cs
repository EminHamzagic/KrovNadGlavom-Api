using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MongoDB.Driver;

namespace krov_nad_glavom_api.Infrastructure.MongoDB.Repositories
{
    public class ReservationRepositoryMongo : RepositoryMongo<Reservation>, IReservationRepository
    {
        private readonly IMongoCollection<Reservation> _reservations;
        private readonly IMongoCollection<UserAgencyFollow> _userAgencyFollows;

        public ReservationRepositoryMongo(krovNadGlavomMongoDbContext context) : base(context)
        {
            _reservations = context.Reservations;
            _userAgencyFollows = context.UserAgencyFollows;
        }

        public async Task<Reservation> GetReservationByUserId(string userId)
        {
            return await _reservations
                .Find(r => r.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<Reservation> GetByApartmentId(string apartmentId)
        {
            return await _reservations
                .Find(r => r.ApartmentId == apartmentId)
                .FirstOrDefaultAsync();
        }

        public bool CanUserReserve(string userId, string agencyId)
        {
            return _userAgencyFollows
                .AsQueryable()
                .Any(c => c.AgencyId == agencyId && c.UserId == userId);
        }

        public async Task<Reservation> GetUserReservation(string userId)
        {
            return await _reservations
                .Find(c => c.UserId == userId)
                .FirstOrDefaultAsync();
        }
    }
}