namespace Fanex.OddsFeed.Services.Impl
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.Common.Helper;
    using Fanex.OddsFeed.Core;
    using Fanex.OddsFeed.Criteria.SubAccount;
    using Fanex.OddsFeed.Criteria.User;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Repositories.Interface;
    using Fanex.OddsFeed.Services.Interface;

    public class SubAccService : ISubAccService
    {
        private readonly IBaseService baseService;
        private readonly ISubAccRepository subAccRepository;

        public SubAccService(
            ISubAccRepository subAccRepository,
            IBaseService baseService)
        {
            this.baseService = baseService;
            this.subAccRepository = subAccRepository;
        }

        public async Task<IEnumerable<User>> GetBySystemId(short systemId)
        {
            var criteria = new GetSubAccBySystemIDCriteria
            {
                SystemId = systemId,
                RoleId = (byte)Constant.UserRole.LicenseeSubAccount
            };
            return (await baseService.GetDataAsync<User>(criteria)).OrderBy(s => s.Name).ToList();
        }

        public async Task<PermissionFlag> GetUserPermission(int userId, short systemId)
        {
            var criteria = new UserGetPermissionCriteria
            {
                UserID = userId,
                SystemID = systemId
            };
            return await baseService.GetDataEntityAsync<PermissionFlag>(criteria);
        }

        public async Task<CommandResult> Insert(InsertSubAccCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                return await subAccRepository.Insert(criteria);
            }

            return null;
        }

        
    }
}