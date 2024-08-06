namespace Fanex.OddsFeed.Services
{
    using System.Threading.Tasks;
    using Fanex.OddsFeed.Criteria.User;
    using Fanex.OddsFeed.Models;

    public class UserService(IBaseService baseService) : IUserService
    {
        public async Task<User> UserLoginAsync(UserLoginUserCriteria criteria)
        {
            return await baseService.GetDataEntityAsync<User>(criteria);
        }

        public async Task<PermissionFlag> UserGetProfileAsync(UserGetProfileCriteria criteria)
        {
            return await baseService.GetDataEntityAsync<PermissionFlag>(criteria);
        }

        public async Task ReUserCheckInAsync(UserReCheckInUserCriteria criteria)
        {
            await baseService.GetDataEntityAsync<bool>(criteria);
        }

        public async Task<User> GetUserInfoAsync(int userId)
        {
            var criteria = new GetUserInfoCriteria
            {
                UserId = userId
            };
            return await baseService.GetDataEntityAsync<User>(criteria);
        }
    }
}