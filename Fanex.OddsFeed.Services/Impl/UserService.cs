namespace Fanex.OddsFeed.Services.Impl
{
    using Fanex.OddsFeed.Criteria.User;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Services.Interface;

    public class UserService : IUserService
    {
        private readonly IBaseService baseService;

        public UserService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

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