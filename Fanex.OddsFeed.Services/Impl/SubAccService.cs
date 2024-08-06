namespace Fanex.OddsFeed.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.OddsFeed.Criteria.SubAccount;
    using Fanex.OddsFeed.Criteria.User;
    using Fanex.OddsFeed.Models;

    public class SubAccService(IBaseService baseService) : ISubAccService
    {
        public async Task<IEnumerable<User>> GetBySystemIdAsync(short systemId)
        {
            var criteria = new GetSubAccBySystemIDCriteria
            {
                SystemId = systemId,
                RoleId = (byte)Core.Enums.UserRole.LicenseeSubAccount
            };
            var users = await baseService.GetDataAsync<User>(criteria);
            return users.OrderBy(s => s.Name).ToList();
        }

        public async Task<PermissionFlag> GetUserPermissionAsync(int userId, short systemId)
        {
            var criteria = new UserGetPermissionCriteria
            {
                UserID = userId,
                SystemID = systemId
            };
            return await baseService.GetDataEntityAsync<PermissionFlag>(criteria);
        }

        public async Task<CommandResult> InsertAsync(InsertSubAccCriteria criteria)
        {
            return await baseService.ExecuteNonQueryAsync(criteria);
        }
    }
}