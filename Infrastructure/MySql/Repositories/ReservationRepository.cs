using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Infrastructure.MySql.Repositories
{
	public class ReservationRepository : Repository<Reservation>, IReservationRepository
	{
		private readonly krovNadGlavomDbContext _context;

		public ReservationRepository(krovNadGlavomDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<Reservation> GetReservationByUserId(string userId)
		{
			return await _context.Reservations.Where(r => r.UserId == userId).FirstOrDefaultAsync();
		}

		public async Task<bool> IsApartmentReserved(string apartmentId)
		{
			var reservation = await _context.Reservations.Where(r => r.ApartmentId == apartmentId).FirstOrDefaultAsync();
			if (reservation == null)
			{
				return false;
			}
			else
			{
				if (reservation.ToDate < DateTime.Now)
					return true;

				_context.Reservations.Remove(reservation);
				await _context.SaveChangesAsync();
				return false;
			}
		}

		public async Task<Reservation> GetByApartmentId(string apartmentId)
        {
            return await _context.Reservations.Where(c => c.ApartmentId == apartmentId).FirstOrDefaultAsync();
        }
		
		public bool CanUserReserve(string userId, string agencyId)
		{
			return _context.UserAgencyFollows.Any(c => c.AgencyId == agencyId && c.UserId == userId);
		}
    }
}