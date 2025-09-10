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
				return true;
			}
			else
			{
				if (reservation.ToDate < DateTime.Now)
					return true;
				return false;
			}
		}
    }
}