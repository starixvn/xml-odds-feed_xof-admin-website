namespace Fanex.OddsFeed.Core.Service.Impl
{
    using System.Collections.Generic;
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

        public UserService() : this(null)
        {
        }

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository ?? new UserRepository();
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

        public PermissionFlag GetPermissionFlags(ref UserGetPermissionCriteria criteria)
        {
            PermissionFlag flags = null;
            if (criteria.IsValid())
            {
                flags = userRepository.GetPermissionFlags(ref criteria);
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

        public IEnumerable<IUser> GetUserInfo(GetUserInfoCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                var rawData = userRepository.GetUserInfo(criteria);
                var result = new List<User>();
                foreach (var data in rawData)
                {
                    result.Add(new User
                    {
                        RoleID = (byte)data.RoleID,
                        FirstName = (string)data.FirstName,
                        LastName = (string)data.LastName,
                        Email = (string)data.Email
                    });
                }

                return result;
            }

            return new List<User>();
        }
    }
}