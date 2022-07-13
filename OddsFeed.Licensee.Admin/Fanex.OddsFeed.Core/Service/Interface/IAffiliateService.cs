namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Model;

    public interface IAffiliateService
    {
        IList<PartnerIP> GetPartnerIpByAffId(int partnerId);

        DataCollection GetPartnerRequestStatisticReturnDataCollection(RequestStatisticCriteria criteria);

        CommandResult Insert(InsertAffiliateCriteria criteria);

        DataCollection GetAffiliateListReturnDataCollection(GetAffiliateByNameCriteria criteria);
    }
}