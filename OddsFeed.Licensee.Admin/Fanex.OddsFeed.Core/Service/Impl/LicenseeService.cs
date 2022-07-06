namespace Fanex.OddsFeed.Core.Service.Impl
{
    using Fanex.OddsFeed.Core.Criteria.Licensee;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Repository.Impl;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Core.Service.Interface;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class LicenseeService : ILicenseeService
    {
        private readonly IBaseService baseService;
        private readonly ILicenseeRepository licenseeRepository;

        public LicenseeService() : this(null, null)
        {
        }

        public LicenseeService(
            IBaseService baseService,
            ILicenseeRepository licenseeRepository)
        {
            this.baseService = baseService ?? new BaseService();
            this.licenseeRepository = licenseeRepository ?? new LicenseeRepository();
        }

        public string GetCurrentOddsStyleBySystemId(int systemId)
        {
            var criteria = new GetOddStyleByLicenseeIDCriteria
            {
                SystemID = systemId
            };

            if (criteria.IsValid())
            {
                return baseService.GetDataEntity<string>(criteria);
            }

            return null;
        }

        public bool GetActiveBySystemId(int systemId)
        {
            var criteria = new GetActivateByLicenseeIDCriteria
            {
                SystemID = systemId
            }; 
            return baseService.GetDataEntity<bool>(criteria);
        }

        public PartnersProfile GetProfileAllPartnersBySystemId(int systemId)
        {
            var criteria = new GetProfileAllPartnersBySystemIdCriteria
            {
                SystemID = systemId
            };

            if (criteria.IsValid())
            {
                return licenseeRepository.GetProfileAllPartnersBySystemId(criteria);
            }

            return null;
        }

        public IList<SelectListItem> GetResultDelayTimeList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "5", Text = "5" },
                new SelectListItem { Value = "10", Text = "10" },
                new SelectListItem { Value = "15", Text = "15" },
                new SelectListItem { Value = "20", Text = "20" },
                new SelectListItem { Value = "25", Text = "25" },
                new SelectListItem { Value = "30", Text = "30" }
            };
        }
    }
}