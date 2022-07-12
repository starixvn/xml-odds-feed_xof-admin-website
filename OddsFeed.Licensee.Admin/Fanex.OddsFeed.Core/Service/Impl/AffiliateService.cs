namespace Fanex.OddsFeed.Core.Service.Impl
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Common.Helper;
    using Fanex.OddsFeed.Core.Repository;
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
        private readonly ISelectListRepository selectListRepository;

        public AffiliateService()
            : this(null, null, null, null)
        {
        }

        public AffiliateService(
            ICurrentSession currentSession,
            IBaseService baseService,
            IAffiliateRepository affilliateRepository,
            ISelectListRepository selectListRepository)
        {
            this.currentSession = currentSession ?? new CurrentSession();
            this.baseService = baseService ?? new BaseService();
            this.affilliateRepository = affilliateRepository ?? new AffiliateRepository();
            this.selectListRepository = selectListRepository ?? new SelectListRepository();
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

        public DataCollection GetAffiliateListReturnDataCollection(CheckExistAffiliateCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                return baseService.GetData(criteria);
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetDomain(bool? isActive = null, bool? isOddsDomain = null, int selectedId = 0)
        {
            var criteria = new GetDomainBySystemIDCriteria
            {
                SystemID = currentSession.SystemId,
                IsActive = isActive,
                IsOddsDomain = isOddsDomain
            };

            if (criteria.IsValid())
            {
                return selectListRepository.Get(criteria).OrderBy(s => s.Text);
            }

            return new List<SelectListItem>();
        }

        public IEnumerable<SelectListItem> GetDomainByAffId(
            int partnerId,
            bool? isActive = null,
            bool? isOddsDomain = null,
            int selectedId = 0)
        {
            var criteria = new GetDomainByAffiliateIDCriteria
            {
                PartnerID = partnerId,
                IsActive = isActive,
                IsOddsDomain = isOddsDomain
            };

            if (criteria.IsValid())
            {
                return selectListRepository.Get(criteria).OrderBy(s => s.Text);
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

        public IEnumerable<SelectListItem> GetOddsTypeByAffId(int partnerId)
        {
            var criteria = new GetTypeSettingByAffiliateIDCriteria
            {
                Type = "OddsType",
                PartnerID = partnerId
            };

            if (criteria.IsValid())
            {
                return selectListRepository.GetAndGenerateCheck(criteria, false).OrderBy(s => s.Text);
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetOddsType(int selectedId = 0)
        {
            var criteria = new GetTypeSettingCriteria
            {
                SystemID = currentSession.SystemId,
                Type = "OddsType"
            };

            if (criteria.IsValid())
            {
                return selectListRepository.Get(criteria, false).OrderBy(s => s.Text);
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetOddsStyle(int selectedId = 0)
        {
            var criteria = new GetTypeSettingCriteria
            {
                SystemID = currentSession.SystemId,
                Type = "OddsRegion"
            };

            if (criteria.IsValid())
            {
                return selectListRepository.Get(criteria, false).OrderBy(s => s.Text);
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetBettypeByAffId(int partnerId)
        {
            var criteria = new GetBettypeByAffiliateIDCriteria
            {
                PartnerID = partnerId
            };

            if (criteria.IsValid())
            {
                return selectListRepository.GetAndGenerateCheck(criteria).OrderBy(s => s.Text);
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetBettypeBySystemId(int selectedId = 0)
        {
            var criteria = new GetBettypeBySystemIDCriteria
            {
                SystemID = currentSession.SystemId
            };

            if (criteria.IsValid())
            {
                return selectListRepository.GetWithGroup(criteria).OrderBy(s => s.Text);
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetCountryByAffId(int partnerId)
        {
            var criteria = new GetCountryByAffiliateIDCriteria
            {
                PartnerID = partnerId
            };

            if (criteria.IsValid())
            {
                return selectListRepository.GetAndGenerateCheck(criteria).OrderBy(s => s.Text);
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetCountryBySystemId(string selectedId = "")
        {
            var criteria = new GetCountryBySystemIDCriteria
            {
                SystemID = currentSession.SystemId
            };

            if (criteria.IsValid())
            {
                return selectListRepository.Get(criteria).OrderBy(s => s.Text);
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetSportByAffId(int partnerId)
        {
            var criteria = new GetSportByAffiliateIDCriteria
            {
                PartnerID = partnerId
            };

            if (criteria.IsValid())
            {
                return selectListRepository.GetAndGenerateCheck(criteria).OrderBy(s => s.Text);
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetLanguageBySystemId(string selectedId = "")
        {
            var criteria = new GetLanguageBySystemIDCriteria
            {
                SystemID = currentSession.SystemId
            };

            if (criteria.IsValid())
            {
                return selectListRepository.Get(criteria).OrderBy(s => s.Text);
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetLanguageByAffId(int partnerId)
        {
            var criteria = new GetLanguageByAffiliateIDCriteria
            {
                PartnerID = partnerId
            };

            if (criteria.IsValid())
            {
                return selectListRepository.GetAndGenerateCheck(criteria).OrderBy(s => s.Text);
            }

            return null;
        }

        public IList<Affiliate> GetAllAffiliates()
        {
            var criteria = new GetAllAffiliateCriteria();
            return baseService.GetData<Affiliate>(criteria).OrderBy(s => s.Name).ToList();
        }

        public IList<Affiliate> GetBySystemId(byte systemId)
        {
            var criteria = new GetAffiliateBySystemIDCriteria
            {
                SystemId = systemId
            };
            return baseService.GetData<Affiliate>(criteria).OrderBy(s => s.Name).ToList();
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