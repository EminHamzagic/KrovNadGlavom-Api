using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Interfaces
{
    public interface IUserAgencyFollowRepository : IRepository<UserAgencyFollow>
    {
        Task<List<User>> GetAgencyFollowers(string agencyId);
        Task<List<Agency>> GetUserFollowing(string userId);
        Task<UserAgencyFollow> IsUserFollowing(string userId, string agencyId);
        Task<List<UserAgencyFollow>> GetByUserId(string userId);
    }
}