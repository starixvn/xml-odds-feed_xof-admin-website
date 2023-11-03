namespace Fanex.OddsFeed.Core.Service.Impl
{
    using Fanex.Common.Helper;
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Core.Service.Interface;

    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IBaseService baseService;

        public UserService(
            IUserRepository userRepository,
            IBaseService baseService)
        {
            this.userRepository = userRepository;
            this.baseService = baseService;
        }

        public async Task<User> UserLogin(UserLoginUserCriteria criteria)
        {
            Check.Argument.IsNotEmpty(criteria.UserName, "username");
            Check.Argument.IsNotEmpty(criteria.UserPwd, "password");
            User user = null;

            if (criteria.IsValid())
            {
                 user = await userRepository.UserLogin(criteria);
            }

            return user;
        }

        public async Task<PermissionFlag> UserGetProfile(UserGetProfileCriteria criteria)
        {
            PermissionFlag flags = null;
            if (criteria.IsValid())
            {
                flags = await userRepository.GetProfile(criteria);
            }

            return flags;
        }

        public async Task ReUserCheckIn(UserReCheckInUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                await userRepository.UserReCheckIn(criteria);
            }
        }

        public User GetUserInfo(int userId)
        {
            var criteria = new GetUserInfoCriteria
            {
                UserId = userId
            };
            return baseService.GetDataEntity<User>(criteria);
        }
    }
}