namespace Fanex.OddsFeed.Services.Impl
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Common.Helper;
    using Fanex.OddsFeed.Criteria.Affiliate;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Models.ValueObject;
    using Fanex.OddsFeed.Repositories.Interface;
    using Fanex.OddsFeed.Services.Interface;

    public class AffiliateService : IAffiliateService
    {
        private readonly IBaseService baseService;
        private readonly IAffiliateRepository affilliateRepository;

        public AffiliateService(
            IAffiliateRepository affilliateRepository,
            IBaseService baseService)
        {
            this.baseService = baseService;
            this.affilliateRepository = affilliateRepository;
        }

        public async Task<IEnumerable<PartnerIP>> GetPartnerIpByAffId(int partnerId)
        {
            var criteria = new GetIpByAffiliateIDCriteria
            {
                PartnerID = partnerId
            };
            return (await baseService.GetDataAsync<PartnerIP>(criteria)).OrderBy(s => s.Name).ToList();
        }

        public async Task<CommandResult> Insert(InsertAffiliateCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                return await affilliateRepository.Insert(criteria);
            }

            return null;
        }
    }
}
