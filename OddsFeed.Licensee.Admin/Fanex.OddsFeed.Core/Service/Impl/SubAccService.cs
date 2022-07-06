namespace Fanex.OddsFeed.Core.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Criteria.SubAccount;
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Repository.Impl;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Framework.Data;

    public class SubAccService : ISubAccService
    {
        private readonly IBaseService baseService;
        private readonly IUserRepository userRepository;
        private readonly ISubAccRepository subAccRepository;

        public SubAccService()
            : this(null, null, null)
        {
        }

        public SubAccService(
            IUserRepository userRepository,
            ISubAccRepository subAccRepository,
            IBaseService baseService)
        {
            this.baseService = baseService ?? new BaseService();
            this.userRepository = userRepository ?? new UserRepository();
            this.subAccRepository = subAccRepository ?? new SubAccRepository();
        }

        public IList<User> GetByNameAndSystemId(byte systemId, string name)
        {
            var criteria = new GetSubAccByNameCriteria
            {
                SystemId = systemId,
                RoleId = 3,
                Username = name
            };

            if (criteria.IsValid())
            {
                IEnumerable<IUser> users = baseService.GetData<User>(criteria).OrderBy(s => s.Name);

                if (users != null)
                {
                    return users.Select(s => new User
                    {
                        Id = s.Id,
                        Username = s.Name,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Email = s.Email,
                        Creator = s.Creator,
                        CreatedDate = Convert.ToDateTime(s.CreatedDate),
                        IsActive = s.IsActive
                    }).ToList();
                }
            }

            return new List<User>();
        }

        public DataCollection GetSubAccountListReturnDataCollection(GetSubAccByNameCriteria criteria)
        {
            if (criteria.IsValid())
            {
                return baseService.GetData(criteria);
            }

            return null;
        }

        public DataCollection GetSubAccountListReturnDataCollection(CheckExistSubAccountCriteria criteria)
        {
            if (criteria.IsValid())
            {
                return baseService.GetData(criteria);
            }

            return null;
        }

        public DataSet GetTop10SubAccountReturnDataSet(GetTop10SubAccCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                return baseService.GetDataSet(criteria);
            }

            return null;
        }

        public IList<User> GetBySystemId(byte systemId)
        {
            var criteria = new GetSubAccBySystemIDCriteria
            {
                SystemId = systemId,
                RoleId = 3
            };

            if (criteria.IsValid())
            {
                IEnumerable<IUser> subAccList = baseService.GetData<User>(criteria).OrderBy(s => s.Name);

                if (subAccList != null)
                {
                    return subAccList.Select(s => new User
                    {
                        Id = s.Id,
                        Username = s.Name,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Email = s.Email,
                        Creator = s.Creator,
                        CreatedDate = Convert.ToDateTime(s.CreatedDate),
                        IsActive = s.IsActive
                    }).ToList();
                }
            }

            return null;
        }

        public PermissionFlag GetUserPermission(int userId, byte systemId)
        {
            var criteria = new UserGetPermissionCriteria
            {
                UserID = userId,
                SystemID = systemId
            };

            if (criteria.IsValid())
            {
                return userRepository.GetPermissionFlags(ref criteria);
            }

            return null;
        }

        public ICommandResult Insert(InsertSubAccCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                return subAccRepository.Insert(ref criteria);
            }

            return null;
        }

        public ICommandResult AssignLicensee(AssignLicensee_SubAccCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                return baseService.ExecuteNonQuery(criteria);
            }

            return null;
        }
    }
}