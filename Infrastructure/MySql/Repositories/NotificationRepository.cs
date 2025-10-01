using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Domain.Entities;
using krov_nad_glavom_api.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Infrastructure.MySql.Repositories
{
	public class NotificationRepository : Repository<Notification>, INotificationRepository
	{
		private readonly krovNadGlavomDbContext _context;

		public NotificationRepository(krovNadGlavomDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<Notification>> GetNotificationsByUserId(string userId)
		{
			return await _context.Notifications.Where(d => d.UserId == userId).OrderByDescending(d => d.CreatedAt).ToListAsync();
		}
    }
}