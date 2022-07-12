namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Helpers;
    using Data.Repository;
    using Fanex.OddsFeed.Core.Model;

    public interface IBaseService
    {
        IEnumerable<SelectListItem> GetSelectList(ICriteria criteria, bool order = true, string selectedValue = "");

        IEnumerable<SelectListItem> GetSelectListWithGroup(ICriteria criteria, bool order = true, string selectedValue = "");

        DataCollection GetData(ICriteria criteria);

        T GetDataEntity<T>(ICriteria criteria);

        IEnumerable<T> GetData<T>(ICriteria criteria);

        CommandResult ExecuteNonQuery(ICriteria criteria);
    }
}