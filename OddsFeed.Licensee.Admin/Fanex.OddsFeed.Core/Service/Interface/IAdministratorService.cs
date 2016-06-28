// --------------------------------------------------------------------------
// <copyright file="IAdministratorService.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013.03.05">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Criteria.Administrator;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed")]
    public interface IAdministratorService : IBaseService
    {
        ICommandResult InsertAdmin(InsertAdminCriteria criteria);

        DataCollection GetAdminListReturnDataCollection(GetAdminCriteria criteria);

        DataSet GetExistedAdminDataSet(GetExistedAdminCriteria criteria);

        DataSet GetTop10AdminReturnDataSet(GetTop10AdminCriteria criteria);
       
        ICommandResult UpdateAdmin(CriteriaBase criteria);

        ICommandResult UpdateAdminInfo(UpdateAdminInfoCriteria criteria);

        ICommandResult UpdateAdminSystem(UpdateAdminSystemCriteria criteria);

        ICommandResult UpdateAdminPassword(UpdateAdminPasswordCriteria criteria);

        DataSet AdminList_GetTop10(AdminList_GetTop10Criteria criteria);

        DataCollection GetAdminListReturnDataCollection(CheckExistAdministratorCriteria criteria);
    }
}
