namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System.Collections.Generic;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Model;

    public interface IAffiliateService
    {
        IList<PartnerIP> GetPartnerIpByAffId(int partnerId);

        CommandResult Insert(InsertAffiliateCriteria criteria);
    }
}