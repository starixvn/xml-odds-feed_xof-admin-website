namespace Fanex.OddsFeed.Services.Interface
{
    using System.Collections.Generic;
    using Fanex.OddsFeed.Criteria.Affiliate;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Models.ValueObject;

    public interface IAffiliateService
    {
        Task<IEnumerable<PartnerIP>> GetPartnerIpByAffId(int partnerId);
        Task<CommandResult> Insert(InsertAffiliateCriteria criteria);
    }
}