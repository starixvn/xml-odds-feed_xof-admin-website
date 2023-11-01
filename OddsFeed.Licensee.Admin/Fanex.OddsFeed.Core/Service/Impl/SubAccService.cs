namespace Fanex.OddsFeed.Core.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.OddsFeed.Core.Business;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Criteria.User;
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

        public IList<User> GetBySystemId(short systemId)
        {
            var criteria = new GetSubAccBySystemIDCriteria
            {
                SystemId = systemId,
                RoleId = (byte)Constant.UserRole.LicenseeSubAccount
            };
            return baseService.GetData<User>(criteria).OrderBy(s => s.Name).ToList();
        }

        public PermissionFlag GetUserPermission(int userId, short systemId)
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