using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Infrastructure.MySql;
using krov_nad_glavom_api.Infrastructure.MySql.Repositories;

namespace krov_nad_glavom_api.Application
{
    public class UnitOfWorkMySql : IUnitofWork
    {
        private readonly krovNadGlavomDbContext _context;

        public IUserRepository Users { get; set; }
        public IUserSessionRepository UserSessions { get; set; }

        public UnitOfWorkMySql(krovNadGlavomDbContext context)
        {
            _context = context;

            Users = new UserRepository(_context);
            UserSessions = new UserSessionRepository(_context);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        
        public async void Dispose()
        {
            await _context.DisposeAsync();
        }
    }
}