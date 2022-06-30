namespace Fanex.OddsFeed.Core.Service.Impl
{
    using System;
    using Fanex.Common.Helper;
    using Fanex.OddsFeed.Core.Criteria.Licensee;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Repository.Impl;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class LicenseeService : BaseService, ILicenseeService
    {
        private readonly ILicenseeRepository _licenseeRepository;

        public LicenseeService()
        {
            _licenseeRepository = _licenseeRepository ?? new LicenseeRepository();
        }

        public string GetCurrentOddsStyleBySystemId(int systemId)
        {
            try
            {
                var criteria = new GetOddStyleByLicenseeIDCriteria
                {
                    SystemID = systemId
                };

                if (criteria.IsValid())
                {
                    return BaseRepository.ReturnSingleValue(criteria).ToString();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public ICommandResult Update(CriteriaBase criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    return this._licenseeRepository.Update(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public bool GetActiveBySystemId(int systemId)
        {
            try
            {
                var criteria = new GetActivateByLicenseeIDCriteria
                {
                    SystemID = systemId
                };

                if (criteria.IsValid())
                {
                    return bool.Parse(BaseRepository.ReturnSingleValue(criteria).ToString());
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }

        public PartnersProfile GetProfileAllPartnersBySystemId(int systemId)
        {
            try
            {
                var criteria = new GetProfileAllPartnersBySystemIdCriteria
                {
                    SystemID = systemId
                };

                if (criteria.IsValid())
                {
                    return _licenseeRepository.GetProfileAllPartnersBySystemId(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
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