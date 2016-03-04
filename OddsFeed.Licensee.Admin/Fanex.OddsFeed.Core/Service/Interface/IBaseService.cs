// --------------------------------------------------------------------------
// <copyright file="IBaseService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.6">Created</change>
//   <change who="Edward.Nguyen" date="2012.8.14">Add GetRepositoryExecLog</change>
// </history>

namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Criteria.Misc;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed")]
    public interface IBaseService
    {
        IServiceContext ServiceContext { get; set; }

        void HandleException(Exception ex);

        LogExecSpCriteria GetLogExecSpCriteria();

        IEnumerable<SelectListItem> GetSelectList(string settingKey, bool order = true, string selectedValue = "");

        IEnumerable<SelectListItem> GetSelectList(ICriteria criteria, bool order = true, string selectedValue = "");

        DataCollection GetData(ICriteria criteria);

        DataSet GetDataSet(ICriteria criteria);

        IDataReader GetDataReader(ICriteria criteria);

        ICommandResult UpdateData(ICriteria criteria);

        ICommandResult DeleteData(ICriteria criteria);
    }
}
