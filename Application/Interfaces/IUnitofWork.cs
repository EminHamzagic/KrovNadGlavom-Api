namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IUnitofWork : IDisposable
    {
        IUserRepository Users { get; }
        IUserSessionRepository UserSessions { get; }

        Task Save();
    }
}