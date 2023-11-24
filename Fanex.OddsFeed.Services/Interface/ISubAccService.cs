namespace Fanex.OddsFeed.Services.Interface
{
    using System.Collections.Generic;
    using Fanex.OddsFeed.Criteria.SubAccount;
    using Fanex.OddsFeed.Models;

    public interface ISubAccService
    {
        Task<IEnumerable<User>> GetBySystemId(short systemId);

        Task<PermissionFlag> GetUserPermission(int userId, short systemId);

        Task<CommandResult> Insert(InsertSubAccCriteria criteria);
    }
}