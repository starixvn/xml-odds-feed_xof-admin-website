namespace Fanex.OddsFeed.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.OddsFeed.Criteria.Affiliate;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Models.ValueObject;

    public class AffiliateService(IBaseService baseService) : IAffiliateService
    {
        public async Task<IEnumerable<PartnerIP>> GetPartnerIpByAffIdAsync(int partnerId)
        {
            var criteria = new GetIpByAffiliateIDCriteria
            {
                PartnerID = partnerId
            };
            var partnerIps = await baseService.GetDataAsync<PartnerIP>(criteria);
            return partnerIps.OrderBy(s => s.Name).ToList();
        }

        public async Task<CommandResult> InsertAsync(InsertAffiliateCriteria criteria)
        {
            return await baseService.ExecuteNonQueryAsync(criteria);
        }
    }
}
