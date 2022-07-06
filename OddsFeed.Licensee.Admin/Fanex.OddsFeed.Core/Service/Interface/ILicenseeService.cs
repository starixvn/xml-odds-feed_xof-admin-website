namespace Fanex.OddsFeed.Core.Service.Interface
{
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface ILicenseeService
    {
        string GetCurrentOddsStyleBySystemId(int systemId);

        ICommandResult Update(CriteriaBase criteria);

        bool GetActiveBySystemId(int systemId);

        PartnersProfile GetProfileAllPartnersBySystemId(int systemId);

        IList<SelectListItem> GetResultDelayTimeList();
    }
}