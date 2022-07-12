namespace Fanex.OddsFeed.Core.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.OddsFeed.Core.Business;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Criteria.SubAccount;
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Repository.Impl;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Core.Service.Interface;

    public class SubAccService : ISubAccService
    {
        private readonly IBaseService baseService;
        private readonly ISubAccRepository subAccRepository;

        public SubAccService() : this(null, null)
        {
        }

        public SubAccService(
            ISubAccRepository subAccRepository,
            IBaseService baseService)
        {
            this.baseService = baseService ?? new BaseService();
            this.subAccRepository = subAccRepository ?? new SubAccRepository();
        }

        public IList<User> GetByNameAndSystemId(byte systemId, string name)
        {
            var criteria = new GetSubAccByNameCriteria
            {
                SystemId = systemId,
                RoleId = (byte)Constant.UserRole.LicenseeSubAccount,
                Username = name
            };

            if (criteria.IsValid())
            {
                var users = baseService.GetData<User>(criteria).OrderBy(s => s.Name);

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

        public IList<User> GetBySystemId(byte systemId)
        {
            var criteria = new GetSubAccBySystemIDCriteria
            {
                SystemId = systemId,
                RoleId = (byte)Constant.UserRole.LicenseeSubAccount
            };
            return baseService.GetData<User>(criteria).OrderBy(s => s.Name).ToList();
        }

        public PermissionFlag GetUserPermission(int userId, byte systemId)
        {
            var criteria = new UserGetPermissionCriteria
            {
                UserID = userId,
                SystemID = systemId
            };
            return baseService.GetDataEntity<PermissionFlag>(criteria);
        }

        public CommandResult Insert(InsertSubAccCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                return subAccRepository.Insert(ref criteria);
            }

            return null;
        }
    }
}