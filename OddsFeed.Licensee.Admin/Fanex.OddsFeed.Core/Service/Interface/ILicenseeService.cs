namespace Fanex.OddsFeed.Core.Service.Interface
{
    using Fanex.OddsFeed.Core.Model;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface ILicenseeService
    {
        bool GetActiveBySystemId(int systemId);

        PartnersProfile GetProfileAllPartnersBySystemId(int systemId);

        IList<SelectListItem> GetResultDelayTimeList();
    }
}