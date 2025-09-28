using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using krov_nad_glavom_api.Application.Utils.Extensions;
using Neo4j.Driver;

namespace krov_nad_glavom_api.Infrastructure.Neo4j.Repositories
{
    public class ReservationRepositoryNeo4j : RepositoryNeo4j<Reservation>, IReservationRepository
    {
        private readonly krovNadGlavomNeo4jDbContext _context;
        private readonly string _label = nameof(Reservation);

        public ReservationRepositoryNeo4j(krovNadGlavomNeo4jDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Reservation> GetReservationByUserId(string userId)
        {
            var query = $"MATCH (r:{_label} {{ UserId: $userId }}) RETURN r LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { userId });

            if (await cursor.FetchAsync())
                return cursor.Current["r"].As<INode>().ToEntity<Reservation>();

            return null;
        }

        public async Task<Reservation> GetByApartmentId(string apartmentId)
        {
            var query = $"MATCH (r:{_label} {{ ApartmentId: $apartmentId }}) RETURN r LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { apartmentId });

            if (await cursor.FetchAsync())
                return cursor.Current["r"].As<INode>().ToEntity<Reservation>();

            return null;
        }

        public async Task<bool> CanUserReserve(string userId, string agencyId)
        {
            var query = "MATCH (f:UserAgencyFollow { UserId: $userId, AgencyId: $agencyId }) RETURN f LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { userId, agencyId });

            return await cursor.FetchAsync();
        }

        public async Task<Reservation> GetUserReservation(string userId)
        {
            var query = $"MATCH (r:{_label} {{ UserId: $userId }}) RETURN r LIMIT 1";
            await using var session = _context.Driver.AsyncSession();
            var cursor = await session.RunAsync(query, new { userId });

            if (await cursor.FetchAsync())
                return cursor.Current["r"].As<INode>().ToEntity<Reservation>();

            return null;
        }
    }
}
