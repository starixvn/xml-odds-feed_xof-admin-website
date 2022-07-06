namespace Fanex.OddsFeed.Core.Service.Impl
{
    using Fanex.Common.Helper;
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Repository.Impl;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Core.Service.Interface;

    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IBaseService baseService;

        public UserService() : this(null, null)
        {
        }

        public UserService(
            IUserRepository userRepository,
            IBaseService baseService)
        {
            this.userRepository = userRepository ?? new UserRepository();
            this.baseService = baseService ?? new BaseService();
        }

        public IUser UserLogin(ref UserLoginUserCriteria criteria)
        {
            Check.Argument.IsNotEmpty(criteria.UserName, "username");
            Check.Argument.IsNotEmpty(criteria.UserPwd, "password");
            IUser user = null;

            if (criteria.IsValid())
            {
                user = userRepository.UserLogin(ref criteria);
            }

            return user;
        }

        public PermissionFlag UserGetProfile(ref UserGetProfileCriteria criteria)
        {
            PermissionFlag flags = null;
            if (criteria.IsValid())
            {
                flags = userRepository.GetProfile(ref criteria);
            }

            return flags;
        }

        public void ReUserCheckIn(ref UserReCheckInUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                userRepository.UserReCheckIn(ref criteria);
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