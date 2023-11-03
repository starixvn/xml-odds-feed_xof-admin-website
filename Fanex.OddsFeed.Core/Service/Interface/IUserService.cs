namespace Fanex.OddsFeed.Core.Service.Interface
{
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Model;

    public interface IUserService
    {
        Task<User> UserLogin(UserLoginUserCriteria criteria);

        Task<PermissionFlag> UserGetProfile(UserGetProfileCriteria criteria);

        Task ReUserCheckIn(UserReCheckInUserCriteria criteria);

        User GetUserInfo(int userId);
    }
}