namespace Fanex.OddsFeed.Core.Service.Impl
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Repository.Impl;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web.Session;

    public class AffiliateService : IAffiliateService
    {
        protected readonly ICurrentSession currentSession;
        private readonly IBaseService baseService;
        private readonly IAffiliateRepository affilliateRepository;

        public AffiliateService()
            : this(null, null, null)
        {
        }

        public AffiliateService(
            ICurrentSession currentSession,
            IBaseService baseService,
            IAffiliateRepository affilliateRepository)
        {
            this.currentSession = currentSession ?? new CurrentSession();
            this.baseService = baseService ?? new BaseService();
            this.affilliateRepository = affilliateRepository ?? new AffiliateRepository();
        }

        public DataCollection GetAffiliateListReturnDataCollection(GetAffiliateByNameCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                return baseService.GetData(criteria);
            }

            return null;
        }

        public IList<PartnerIP> GetPartnerIpByAffId(int partnerId)
        {
            var criteria = new GetIpByAffiliateIDCriteria
            {
                PartnerID = partnerId
            };
            return baseService.GetData<PartnerIP>(criteria).OrderBy(s => s.Name).ToList();
        }

        public DataCollection GetPartnerRequestStatisticReturnDataCollection(RequestStatisticCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                return baseService.GetData(criteria);
            }

            return null;
        }

        public CommandResult Insert(InsertAffiliateCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                return affilliateRepository.Insert(ref criteria);
            }

            return null;
        }
    }
}