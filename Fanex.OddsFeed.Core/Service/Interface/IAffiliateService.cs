namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System.Collections.Generic;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Model;

    public interface IAffiliateService
    {
        Task<IEnumerable<PartnerIP>> GetPartnerIpByAffId(int partnerId);
        Task<CommandResult> Insert(InsertAffiliateCriteria criteria);
    }
}