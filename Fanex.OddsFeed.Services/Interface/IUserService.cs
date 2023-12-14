namespace Fanex.OddsFeed.Services.Interface
{
    using System.Threading.Tasks;
    using Fanex.OddsFeed.Criteria.User;
    using Fanex.OddsFeed.Models;

    public interface IUserService
    {
        Task<User> UserLoginAsync(UserLoginUserCriteria criteria);

        Task<PermissionFlag> UserGetProfileAsync(UserGetProfileCriteria criteria);

        Task ReUserCheckInAsync(UserReCheckInUserCriteria criteria);

        Task<User> GetUserInfoAsync(int userId);
    }
}