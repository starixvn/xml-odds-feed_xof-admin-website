// --------------------------------------------------------------------------
// <copyright file="AffiliateService.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web.Mvc;
    using Data.Repository;
    using Fanex.Caching;
    using Fanex.Common.Helper;
    using Fanex.Databank.Repositories;
    using Fanex.OddsFeed.Core.Command;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Repository.Impl;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class AffiliateService.
    /// </summary>
    public class AffiliateService : BaseService, IAffiliateService
    {
        #region members

        private readonly PartnerIPRepository _partnerIpRepository;
        private readonly PartnerStatisticRepository _partnerStatisticRepository;
        private readonly IAffiliateRepository _affilliateRepository;
        private readonly SelectListRepository _selectListRepository;
        private readonly ICache _cache;

        #endregion members

        #region ctors

        public AffiliateService()
            : this(null, null, null, null, null)
        {
        }

        public AffiliateService(
                                    PartnerIPRepository partnerIpRepository,
                                    IAffiliateRepository affilliateRepository,
                                    PartnerStatisticRepository partnerStatisticRepository,
                                    SelectListRepository selectListRepository,
                                    ICache cache)
        {
            _partnerIpRepository = partnerIpRepository ?? new PartnerIPRepository();
            _affilliateRepository = affilliateRepository ?? new AffiliateRepository();
            _partnerStatisticRepository = partnerStatisticRepository ?? new PartnerStatisticRepository();
            _selectListRepository = selectListRepository ?? new SelectListRepository();
            _cache = cache ?? CacheService.CreateInstance(CacheType.MemoryCache);
        }

        #endregion ctors

        #region Get

        public IAffiliate GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Affiliate> GetByNameAndSystemId(byte systemId, string name)
        {
            try
            {
                var criteria = new GetAffiliateByNameCriteria
                {
                    SystemId = systemId,
                    UserName = name
                };

                if (criteria.IsValid())
                {
                    IEnumerable<IAffiliate> affList = _affilliateRepository.Fetch(criteria).OrderBy(s => s.Name);

                    if (affList != null)
                    {
                        return affList.Select(s => new Affiliate
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Site = s.Site,
                            IsAllowed = s.IsAllowed,
                            CreatedDate = Convert.ToDateTime(s.CreatedDate),
                            LastUpdate = Convert.ToDateTime(s.LastUpdate),
                            Creator = s.Creator,
                            ContactPerson = s.ContactPerson,
                            ContactEmail = s.ContactEmail
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IDataReader GetAffiliateListReturnDataReader(byte systemId, string name)
        {
            try
            {
                var criteria = new GetAffiliateByNameCriteria
                {
                    SystemId = systemId,
                    UserName = name
                };

                if (criteria.IsValid())
                {
                    return _affilliateRepository.ReturnDataReader(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public DataCollection GetAffiliateListReturnDataCollection(GetAffiliateByNameCriteria criteria)
        {
            try
            {
                // check parameters
                Check.Argument.IsNotNull(criteria, "criteria");

                if (criteria.IsValid())
                {
                    return _affilliateRepository.ReturnDataCollection(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public DataCollection GetAffiliateListReturnDataCollection(CheckExistAffiliateCriteria criteria)
        {
            try
            {
                // check parameters
                Check.Argument.IsNotNull(criteria, "criteria");

                if (criteria.IsValid())
                {
                    return _affilliateRepository.ReturnDataCollection(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public bool GetStatusById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SelectListItem> GetDomain(bool? isActive = null, bool? isOddsDomain = null, int selectedId = 0)
        {
            try
            {
                var criteria = new GetDomainBySystemIDCriteria
                {
                    SystemID = ServiceContext.SystemId,
                    IsActive = isActive,
                    IsOddsDomain = isOddsDomain
                };

                if (criteria.IsValid())
                {
                    return _selectListRepository.Get(criteria).OrderBy(s => s.Text);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return new List<SelectListItem>();
        }

        public IEnumerable<SelectListItem> GetDomainByAffId(
            int partnerId,
            bool? isActive = null,
            bool? isOddsDomain = null,
            int selectedId = 0)
        {
            try
            {
                var criteria = new GetDomainByAffiliateIDCriteria
                {
                    PartnerID = partnerId,
                    IsActive = isActive,
                    IsOddsDomain = isOddsDomain
                };

                if (criteria.IsValid())
                {
                    return _selectListRepository.Get(criteria).OrderBy(s => s.Text);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IList<PartnerIP> GetPartnerIpByAffId(int partnerId)
        {
            try
            {
                var criteria = new GetIpByAffiliateIDCriteria
                {
                    PartnerID = partnerId
                };

                if (criteria.IsValid())
                {
                    IEnumerable<PartnerIP> list = _partnerIpRepository.Fetch(criteria).OrderBy(s => s.Name);

                    if (list != null)
                    {
                        return list.Select(s => new PartnerIP
                        {
                            Id = s.Id,
                            IPFrom = s.IPFrom,
                            IPTo = s.IPTo
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return new List<PartnerIP>();
        }

        public IEnumerable<RequestStatistic> GetPartnerRequestStatistic(RequestStatisticCriteria criteria)
        {
            try
            {
                // check parameters
                Check.Argument.IsNotNull(criteria, "criteria");

                if (criteria.IsValid())
                {
                    var list = _partnerStatisticRepository.Fetch(criteria);

                    return list;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IDataReader GetPartnerRequestStatisticReturnDataReader(RequestStatisticCriteria criteria)
        {
            try
            {
                // check parameters
                Check.Argument.IsNotNull(criteria, "criteria");

                if (criteria.IsValid())
                {
                    return _partnerStatisticRepository.ReturnDataReader(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public DataCollection GetPartnerRequestStatisticReturnDataCollection(RequestStatisticCriteria criteria)
        {
            try
            {
                // check parameters
                Check.Argument.IsNotNull(criteria, "criteria");

                if (criteria.IsValid())
                {
                    return _affilliateRepository.ReturnDataCollection(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetOddsTypeByAffId(int partnerId)
        {
            try
            {
                var criteria = new GetTypeSettingByAffiliateIDCriteria
                {
                    Type = "OddsType",
                    PartnerID = partnerId
                };

                if (criteria.IsValid())
                {
                    return _selectListRepository.GetAndGenerateCheck(criteria, false).OrderBy(s => s.Text);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetOddsType(int selectedId = 0)
        {
            try
            {
                var criteria = new GetTypeSettingCriteria
                {
                    SystemID = ServiceContext.SystemId,
                    Type = "OddsType"
                };

                if (criteria.IsValid())
                {
                    return _selectListRepository.Get(criteria, false).OrderBy(s => s.Text);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public string GetCurrentOddsStyleByAffId(int partnerId)
        {
            try
            {
                var criteria = new GetOddStyleByAffiliateIDCriteria
                {
                    PartnerID = partnerId
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

        public IEnumerable<SelectListItem> GetOddsStyle(int selectedId = 0)
        {
            try
            {
                var criteria = new GetTypeSettingCriteria
                {
                    SystemID = ServiceContext.SystemId,
                    Type = "OddsRegion"
                };

                if (criteria.IsValid())
                {
                    return _selectListRepository.Get(criteria, false).OrderBy(s => s.Text);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetBettypeByAffId(int partnerId)
        {
            try
            {
                var criteria = new GetBettypeByAffiliateIDCriteria
                {
                    PartnerID = partnerId
                };

                if (criteria.IsValid())
                {
                    return _selectListRepository.GetAndGenerateCheck(criteria).OrderBy(s => s.Text);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetBettypeBySystemId(int selectedId = 0)
        {
            try
            {
                var criteria = new GetBettypeBySystemIDCriteria
                {
                    SystemID = ServiceContext.SystemId
                };

                if (criteria.IsValid())
                {
                    return SelectListRepository.GetWithGroup(criteria).OrderBy(s => s.Text);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetCountryByAffId(int partnerId)
        {
            try
            {
                var criteria = new GetCountryByAffiliateIDCriteria
                {
                    PartnerID = partnerId
                };

                if (criteria.IsValid())
                {
                    return _selectListRepository.GetAndGenerateCheck(criteria).OrderBy(s => s.Text);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetCountryBySystemId(string selectedId = "")
        {
            try
            {
                var criteria = new GetCountryBySystemIDCriteria
                {
                    SystemID = ServiceContext.SystemId
                };

                if (criteria.IsValid())
                {
                    return _selectListRepository.Get(criteria).OrderBy(s => s.Text);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetSportByAffId(int partnerId)
        {
            try
            {
                var criteria = new GetSportByAffiliateIDCriteria
                {
                    PartnerID = partnerId
                };

                if (criteria.IsValid())
                {
                    return _selectListRepository.GetAndGenerateCheck(criteria).OrderBy(s => s.Text);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetSportBySystemId(byte selectedId = 0)
        {
            try
            {
                var criteria = new GetSportBySystemIDCriteria
                {
                    SystemID = ServiceContext.SystemId
                };

                if (criteria.IsValid())
                {
                    return SelectListRepository.GetWithGroup(criteria).OrderBy(s => s.Text);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetLanguageBySystemId(string selectedId = "")
        {
            try
            {
                var criteria = new GetLanguageBySystemIDCriteria
                {
                    SystemID = ServiceContext.SystemId
                };

                if (criteria.IsValid())
                {
                    return _selectListRepository.Get(criteria).OrderBy(s => s.Text);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetLanguageByAffId(int partnerId)
        {
            try
            {
                var criteria = new GetLanguageByAffiliateIDCriteria
                {
                    PartnerID = partnerId
                };

                if (criteria.IsValid())
                {
                    return _selectListRepository.GetAndGenerateCheck(criteria).OrderBy(s => s.Text);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IList<Affiliate> GetAllAffiliates()
        {
            try
            {
                var criteria = new GetAllAffiliateCriteria
                {
                };

                if (criteria.IsValid())
                {
                    IEnumerable<IAffiliate> list = _affilliateRepository.Fetch(criteria).OrderBy(s => s.Name);

                    if (list != null)
                    {
                        return list.Select(s => new Affiliate
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Site = s.Site,
                            IsAllowed = s.IsAllowed,
                            CreatedDate = Convert.ToDateTime(s.CreatedDate),
                            LastUpdate = Convert.ToDateTime(s.LastUpdate),
                            Creator = s.Creator,
                            ContactPerson = s.ContactPerson,
                            ContactEmail = s.ContactEmail
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IList<Affiliate> GetBySystemId(byte systemId)
        {
            try
            {
                var criteria = new GetAffiliateBySystemIDCriteria
                {
                    SystemId = systemId
                };

                if (criteria.IsValid())
                {
                    IEnumerable<IAffiliate> affList = _affilliateRepository.Fetch(criteria).OrderBy(s => s.Name);

                    if (affList != null)
                    {
                        return affList.Select(s => new Affiliate
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Site = s.Site,
                            Remark = s.Remark,
                            IsAllowed = s.IsAllowed,
                            CreatedDate = Convert.ToDateTime(s.CreatedDate),
                            LastUpdate = Convert.ToDateTime(s.LastUpdate),
                            Creator = s.Creator,
                            ContactPerson = s.ContactPerson,
                            ContactEmail = s.ContactEmail
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public DataSet GetTop10AffReturnDataSet(GetTop10AffiliateCriteria criteria)
        {
            try
            {
                // check parameters
                Check.Argument.IsNotNull(criteria, "criteria");

                if (criteria.IsValid())
                {
                    return _affilliateRepository.ReturnDataSet(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        #endregion Get

        #region Insert/Update

        public ICommandResult Insert(InsertAffiliateCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    return _affilliateRepository.Insert(ref criteria);
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
            ICommandResult result = new CommandResult
            {
                Code = 1,
                Message = "Internal Server Error !"
            };

            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    result = _affilliateRepository.Update(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return result;
        }

        #endregion Insert/Update

        #region Delete

        public ICommandResult Delete(CriteriaBase criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    return _affilliateRepository.Delete(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        #endregion Delete
    }
}