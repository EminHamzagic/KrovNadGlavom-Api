using krov_nad_glavom_api.Application.Utils.Extensions;
using krov_nad_glavom_api.Domain.Entities;
using krov_nad_glavom_api.Domain.Interfaces;
using Neo4j.Driver;

namespace krov_nad_glavom_api.Infrastructure.Neo4j.Repositories
{
    public class NotificationRepositoryNeo4j : RepositoryNeo4j<Notification>, INotificationRepository
    {
        private readonly krovNadGlavomNeo4jDbContext _context;
        private readonly string _label = nameof(Notification);
        public NotificationRepositoryNeo4j(krovNadGlavomNeo4jDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetNotificationsByUserId(string userId)
        {
            var query = $"MATCH (n:{_label} {{ UserId: $userId }}) RETURN n ORDER BY n.CreatedAt DESC";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { userId });

            var list = new List<Notification>();
            await foreach (var record in cursor)
                list.Add(record["n"].As<INode>().ToEntity<Notification>());

            return list;
        }
        
        public async Task<int> GetUserNotificationsCount(string userId)
		{
			var query = $"MATCH (n:{_label} {{ UserId: $userId }}) RETURN count(n) AS cnt";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { userId });

            var record = await cursor.SingleAsync();
            return record["cnt"].As<int>();
		}
    }
}