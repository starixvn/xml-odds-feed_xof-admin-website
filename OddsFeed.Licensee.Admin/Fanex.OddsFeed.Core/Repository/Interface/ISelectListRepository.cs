namespace Fanex.OddsFeed.Core.Repository
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Data.Repository;

    public interface ISelectListRepository
    {
        IEnumerable<SelectListItem> Get(string settingKey, bool order = true, string selectedValue = "");

        IEnumerable<SelectListItem> Get(ICriteria criteria, bool order = true, string selectedValue = "");

        IEnumerable<SelectListItem> GetWithGroup(ICriteria criteria, bool order = true, string selectedValue = "");

        IEnumerable<SelectListItem> GetAndGenerateCheck(ICriteria criteria, bool order = true);
    }
}