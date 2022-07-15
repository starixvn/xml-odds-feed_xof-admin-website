namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System.Collections.Generic;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Model;

    public interface ISubAccService
    {
        IList<User> GetBySystemId(byte systemId);

        PermissionFlag GetUserPermission(int userId, byte systemId);

        CommandResult Insert(InsertSubAccCriteria criteria);
    }
}