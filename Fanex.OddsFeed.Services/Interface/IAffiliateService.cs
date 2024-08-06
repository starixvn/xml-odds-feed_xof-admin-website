namespace Fanex.OddsFeed.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.OddsFeed.Criteria.Affiliate;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Models.ValueObject;

    public interface IAffiliateService
    {
        Task<IEnumerable<PartnerIP>> GetPartnerIpByAffIdAsync(int partnerId);

        Task<CommandResult> InsertAsync(InsertAffiliateCriteria criteria);
    }
}