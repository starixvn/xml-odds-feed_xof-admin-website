namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Criteria.SubAccount;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;

    public interface ISubAccService
    {
        IList<User> GetByNameAndSystemId(byte systemId, string name);

        IList<User> GetBySystemId(byte systemId);

        DataCollection GetSubAccountListReturnDataCollection(GetSubAccByNameCriteria criteria);

        DataSet GetTop10SubAccountReturnDataSet(GetTop10SubAccCriteria criteria);

        PermissionFlag GetUserPermission(int userId, byte systemId);

        ICommandResult Insert(InsertSubAccCriteria criteria);
        
        ICommandResult AssignLicensee(AssignLicensee_SubAccCriteria criteria);

        DataCollection GetSubAccountListReturnDataCollection(CheckExistSubAccountCriteria criteria);
    }
}