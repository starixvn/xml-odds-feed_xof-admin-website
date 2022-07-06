namespace Fanex.OddsFeed.Core.Repository.Interface
{
    using System.Linq;
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Model;

    public interface IUserRepository
    {
        IUser UserLogin(ref UserLoginUserCriteria criteria);

        PermissionFlag GetProfile(ref UserGetProfileCriteria criteria);

        PermissionFlag GetPermissionFlags(ref UserGetPermissionCriteria criteria);

        void UserReCheckIn(ref UserReCheckInUserCriteria criteria);

        IQueryable<dynamic> GetUserInfo(GetUserInfoCriteria criteria);
    }
}