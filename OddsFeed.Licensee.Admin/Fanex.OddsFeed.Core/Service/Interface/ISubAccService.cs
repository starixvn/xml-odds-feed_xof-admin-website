// --------------------------------------------------------------------------
// <copyright file="ISubAccService.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Business;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Criteria.SubAccount;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Model.ValueObject;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed")]
    public interface ISubAccService : IBaseService
    {
        IList<User> GetByNameAndSystemId(byte systemId, string name);

        IList<User> GetBySystemId(byte systemId);

        DataCollection GetSubAccountListReturnDataCollection(GetSubAccByNameCriteria criteria);

        DataSet GetTop10SubAccountReturnDataSet(GetTop10SubAccCriteria criteria);

        PermissionFlag GetUserPermission(int userId, byte systemId);

        ICommandResult Insert(InsertSubAccCriteria criteria);

        ICommandResult Update(CriteriaBase criteria);

        ICommandResult Delete(CriteriaBase criteria);
        
        ICommandResult AssignLicensee(AssignLicensee_SubAccCriteria criteria);

        DataCollection GetSubAccountListReturnDataCollection(CheckExistSubAccountCriteria criteria);
    }
}