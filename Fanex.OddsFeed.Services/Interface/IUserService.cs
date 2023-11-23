namespace Fanex.OddsFeed.Services.Interface
{
    using Fanex.OddsFeed.Criteria.User;
    using Fanex.OddsFeed.Models;

    public interface IUserService
    {
        Task<User> UserLogin(UserLoginUserCriteria criteria);

        Task<PermissionFlag> UserGetProfile(UserGetProfileCriteria criteria);

        Task ReUserCheckIn(UserReCheckInUserCriteria criteria);

        Task<User> GetUserInfo(int userId);
    }
}