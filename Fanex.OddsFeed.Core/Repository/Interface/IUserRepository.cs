namespace Fanex.OddsFeed.Core.Repository.Interface
{
    using Fanex.Data.Repository;
    using Fanex.OddsFeed.Core.Model;

    public interface IUserRepository
    {
        Task<User> UserLogin(ICriteria criteria);
        Task<PermissionFlag> GetProfile(ICriteria criteria);
        Task UserReCheckIn(ICriteria criteria);
    }
}