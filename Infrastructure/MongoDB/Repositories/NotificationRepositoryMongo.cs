using krov_nad_glavom_api.Domain.Entities;
using krov_nad_glavom_api.Domain.Interfaces;
using MongoDB.Driver;

namespace krov_nad_glavom_api.Infrastructure.MongoDB.Repositories
{
    public class NotificationRepositoryMongo : RepositoryMongo<Notification>, INotificationRepository
    {
        private readonly IMongoCollection<Notification> _notifications;
        public NotificationRepositoryMongo(krovNadGlavomMongoDbContext context) : base(context)
        {
            _notifications = context.Notifications;
        }

        public async Task<List<Notification>> GetNotificationsByUserId(string userId)
        {
            return await _notifications
                .Find(d => d.UserId == userId)
                .SortByDescending(d => d.CreatedAt)
                .ToListAsync();
        }
        
        public Task<int> GetUserNotificationsCount(string userId)
		{
			return Task.FromResult((int)_notifications.CountDocuments(a => a.UserId == userId));
		}
    }
}