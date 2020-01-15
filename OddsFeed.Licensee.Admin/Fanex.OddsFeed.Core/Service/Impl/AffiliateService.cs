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

        /// <summary>
        /// Initializes a new instance of the <see cref="AffiliateService"/> class.
        /// </summary>
        public AffiliateService()
            : this(null, null, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AffiliateService"/> class.
        /// </summary>
        /// <param name="partnerIpRepository">The partner IP repository.</param>
        /// <param name="affilliateRepository">The affiliate repository.</param>
        /// <param name="partnerStatisticRepository">The partner statistic repository.</param>
        /// <param name="selectListRepository">The select list repository.</param>
        /// <param name="cache">The cache.</param>
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

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Object IAffiliate.</returns>
        /// <exception cref="System.NotImplementedException">Object NotImplementedException.</exception>
        public IAffiliate GetById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the by name and system identifier.
        /// </summary>
        /// <param name="systemId">The system identifier.</param>
        /// <param name="name">The affiliate name.</param>
        /// <returns>IList {Affiliate}.</returns>
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

        /// <summary>
        /// Gets the affiliate list return data reader.
        /// </summary>
        /// <param name="systemId">The system identifier.</param>
        /// <param name="name">The affiliate name.</param>
        /// <returns>IData Reader.</returns>
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

        /// <summary>
        /// Gets the affiliate list return data collection.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Data Collection.</returns>
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

        /// <summary>
        /// Gets the affiliate list return data collection.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Data Collection.</returns>
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

        /// <summary>
        /// Gets the domain.
        /// </summary>
        /// <param name="isActive">If set to <c>true</c> [is active].</param>
        /// <param name="isOddsDomain">If set to <c>true</c> [is odds domain].</param>
        /// <param name="selectedId">The selected identifier.</param>
        /// <returns>IEnumerable {SelectListItem}.</returns>
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

                    ////IEnumerable<DomainSetting> domainList = _domainRepository.Fetch(criteria).OrderBy(s => s.Name);

                    ////if (domainList != null)
                    ////{
                    ////    return domainList.Select(s => new SelectListItem
                    ////    {
                    ////        Text = s.Name,
                    ////        Value = s.Id.ToString(),
                    ////        Selected = s.Id == selectedId
                    ////    }).ToList();
                    ////}
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the domain by affiliate identifier.
        /// </summary>
        /// <param name="partnerId">The partner identifier.</param>
        /// <param name="isActive">If set to <c>true</c> [is active].</param>
        /// <param name="isOddsDomain">If set to <c>true</c> [is odds domain].</param>
        /// <param name="selectedId">The selected identifier.</param>
        /// <returns>IEnumerable {SelectListItem}.</returns>
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

        /// <summary>
        /// Gets the partner IP by affiliate identifier.
        /// </summary>
        /// <param name="partnerId">The partner identifier.</param>
        /// <returns>IList {PartnerIP}.</returns>
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

            return null;
        }

        /// <summary>
        /// Gets the partner request statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IEnumerable {RequestStatistic}.</returns>
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

        /// <summary>
        /// Gets the partner request statistic return data reader.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Object IDataReader.</returns>
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

        /// <summary>
        /// Gets the partner request statistic return data collection.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Data Collection.</returns>
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

        /// <summary>
        /// Gets the odds type by affiliate identifier.
        /// </summary>
        /// <param name="partnerId">The partner identifier.</param>
        /// <returns>IEnumerable {SelectListItem}.</returns>
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

        /// <summary>
        /// Gets the type of the odds.
        /// </summary>
        /// <param name="selectedId">The selected identifier.</param>
        /// <returns>IEnumerable {SelectListItem}.</returns>
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

        /// <summary>
        /// Gets the current odds style by affiliate identifier.
        /// </summary>
        /// <param name="partnerId">The partner identifier.</param>
        /// <returns>System String.</returns>
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

        /// <summary>
        /// Gets the odds style.
        /// </summary>
        /// <param name="selectedId">The selected identifier.</param>
        /// <returns>IEnumerable {SelectListItem}.</returns>
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

        /// <summary>
        /// Gets the BETTYPE by affiliate identifier.
        /// </summary>
        /// <param name="partnerId">The partner identifier.</param>
        /// <returns>IEnumerable {SelectListItem}.</returns>
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

        /// <summary>
        /// Gets the BETTYPE by system identifier.
        /// </summary>
        /// <param name="selectedId">The selected identifier.</param>
        /// <returns>IEnumerable {SelectListItem}.</returns>
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
                    return _selectListRepository.GetWithGroup(criteria).OrderBy(s => s.Text);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the country by affiliate identifier.
        /// </summary>
        /// <param name="partnerId">The partner identifier.</param>
        /// <returns>IEnumerable {SelectListItem}.</returns>
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

        /// <summary>
        /// Gets the country by system identifier.
        /// </summary>
        /// <param name="selectedId">The selected identifier.</param>
        /// <returns>IEnumerable {SelectListItem}.</returns>
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

        /// <summary>
        /// Gets the sport by affiliate identifier.
        /// </summary>
        /// <param name="partnerId">The partner identifier.</param>
        /// <returns>IEnumerable {SelectListItem}.</returns>
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

        /// <summary>
        /// Gets the sport by system identifier.
        /// </summary>
        /// <param name="selectedId">The selected identifier.</param>
        /// <returns>IEnumerable {SelectListItem}.</returns>
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
                    return _selectListRepository.GetWithGroup(criteria).OrderBy(s => s.Text);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the language by system identifier.
        /// </summary>
        /// <param name="selectedId">The selected identifier.</param>
        /// <returns>IEnumerable {SelectListItem}.</returns>
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

        /// <summary>
        /// Gets the language by affiliate identifier.
        /// </summary>
        /// <param name="partnerId">The partner identifier.</param>
        /// <returns>IEnumerable {SelectListItem}.</returns>
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

        /// <summary>
        /// Gets all affiliates.
        /// </summary>
        /// <returns>IList {Affiliate}.</returns>
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

        /// <summary>
        /// Gets the by system identifier.
        /// </summary>
        /// <param name="systemId">The system identifier.</param>
        /// <returns>IList {Affiliate}.</returns>
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

        /// <summary>
        /// Gets the top10 affiliate return data set.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The DataSet.</returns>
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

        /// <summary>
        /// Updates the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>ICommand Result.</returns>
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

        /// <summary>
        /// Deletes the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>ICommand Result.</returns>
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