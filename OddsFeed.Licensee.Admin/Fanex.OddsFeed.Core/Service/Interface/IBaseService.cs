namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Criteria.Misc;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;

    public interface IBaseService
    {
        LogExecSpCriteria GetLogExecSpCriteria();

        IEnumerable<SelectListItem> GetSelectList(string settingKey, bool order = true, string selectedValue = "");

        IEnumerable<SelectListItem> GetSelectList(ICriteria criteria, bool order = true, string selectedValue = "");

        IEnumerable<SelectListItem> GetSelectListWithGroup(ICriteria criteria, bool order = true, string selectedValue = "");

        DataCollection GetData(ICriteria criteria);

        DataSet GetDataSet(ICriteria criteria);

        IDataReader GetDataReader(ICriteria criteria);

        T GetDataEntity<T>(ICriteria criteria);

        IEnumerable<T> GetData<T>(ICriteria criteria);

        ICommandResult ExecuteNonQuery(ICriteria criteria);
    }
}
