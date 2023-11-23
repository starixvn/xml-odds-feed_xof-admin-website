namespace Fanex.OddsFeed.Repositories.Interface
{
    using Fanex.Data.Repository;
    using Fanex.OddsFeed.Models;

    public interface IUserRepository
    {
        Task<User> UserLogin(ICriteria criteria);
        Task<PermissionFlag> GetProfile(ICriteria criteria);
        Task UserReCheckIn(ICriteria criteria);
    }
}