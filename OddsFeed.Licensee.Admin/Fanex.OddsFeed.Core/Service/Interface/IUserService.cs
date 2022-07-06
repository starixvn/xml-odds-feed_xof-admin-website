namespace Fanex.OddsFeed.Core.Service.Interface
{
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Model;

    public interface IUserService
    {
        IUser UserLogin(ref UserLoginUserCriteria criteria);

        PermissionFlag UserGetProfile(ref UserGetProfileCriteria criteria);

        void ReUserCheckIn(ref UserReCheckInUserCriteria criteria);

        User GetUserInfo(int userId);
    }
}