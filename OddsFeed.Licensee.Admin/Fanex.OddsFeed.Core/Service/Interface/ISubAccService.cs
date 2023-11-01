namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System.Collections.Generic;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Model;

    public interface ISubAccService
    {
        IList<User> GetBySystemId(short systemId);

        PermissionFlag GetUserPermission(int userId, short systemId);

        CommandResult Insert(InsertSubAccCriteria criteria);
    }
}