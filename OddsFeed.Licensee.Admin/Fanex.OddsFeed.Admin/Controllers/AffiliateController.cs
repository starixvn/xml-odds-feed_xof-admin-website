namespace Fanex.OddsFeed.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.OddsFeed.Core;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Criteria.Licensee;
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Model.ValueObject;
    using Fanex.OddsFeed.Core.Service.Impl;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Core.Web.Attribute;

#pragma warning disable S1541 // Methods and properties should not be too complex
    [IsAuthorize]
    public class AffiliateController : BaseController
    {
        private const string MajorGroup = "Major";
        private const string MinorGroup = "Minor";
        private const string URLPrefix = "http://";
        private const string DefaultController = "Affiliate";
        private const string OddsRegionSettingType = "OddsRegion";
        private const string OddsTypeSettingType = "OddsType";
        private const byte OutrightDelayTimesIndex = 2;

        private readonly IAffiliateService affiliateService;
        private readonly ILicenseeService licenseeService;

        public AffiliateController()
            : this(null, null)
        {
        }

        public AffiliateController(
            IAffiliateService affiliateService,
            ILicenseeService licenseeService)
        {
            this.affiliateService = affiliateService ?? new AffiliateService();
            this.licenseeService = licenseeService ?? new LicenseeService();
        }

        #region Add New

        [HttpGet]
        public ActionResult Add()
        {
            var criteria = new InsertAffiliateCriteria();
            ViewBag.VirtualPath = GetVirtualPath();

            // Domain
            var domainList = affiliateService.GetDomain();
            ViewData["slAssignedDomain"] = domainList;
            ViewData["slDomain"] = new List<SelectListItem>();

            // Profile
            var licenseeProfile = baseService.GetDataEntity<LicenseeProfile>(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = currentSession.SystemId });
            if (licenseeProfile != null)
            {
                criteria.OddsRegion = licenseeProfile.OddsRegion;
                criteria.NumberOfIPs = licenseeProfile.NumberOfIPs;

                // Delay Times
                var delayTimes = licenseeProfile.OddsDelayTimes.Split(',');
                criteria.DeadballDelayTimes = delayTimes.Length > 0 ? delayTimes[0] : "0";
                criteria.LiveDelayTimes = delayTimes.Length > 1 ? delayTimes[1] : "0";
                criteria.OutrightDelayTimes = delayTimes.Length > OutrightDelayTimesIndex ? delayTimes[OutrightDelayTimesIndex] : "0";
                criteria.ResultDelayTimes = licenseeProfile.ResultDelayTimes;
            }

            if (IsAccountManagerRole())
            {
                ViewBag.HasAccountManagerPermission = true;
                AddAffiliateWithAccountManagerRole(licenseeProfile, ref criteria);
            }
            else if (IsLicenseeAdminRole())
            {
                AddAffiliateWithLicenseeAdminRole(licenseeProfile);
            }
            else
            {
                AddAffiliateWithSubAccountRole();
            }

            return View(criteria);
        }

        [HttpPost]
        public JsonResult AddNew(InsertAffiliateCriteria criteria)
        {
            var result = new CommandResult();
            CustomizeDataBeforeAddNew(ref criteria);
            if (IsValidListOfIPs(criteria.IpsFrom, criteria.IpsTo))
            {
                result = affiliateService.Insert(criteria);
                if (result.IsSuccessful)
                {
                    result.Message = ViewResources.ActionResult_AddAffiliateSuccess;
                }
            }
            else
            {
                result.Code = 1;
                result.Message = ViewResources.Error_InvalidInput;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckExistAffiliate(InsertAffiliateCriteria criteria)
        {
            var result = new CommandResult { Code = 0 };
            if (string.IsNullOrEmpty(criteria.UserName))
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var affiliates = baseService.GetData<Affiliate>(new CheckExistAffiliateCriteria
            {
                UserName = criteria.UserName,
                PageIndex = 1,
                PageSize = 1,
                SystemId = currentSession.SystemId
            });
            if (affiliates != null && affiliates.Any())
            {
                result.Code = 1;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Add New

        #region Edit

        [HttpGet]
        public ActionResult EditAffiliateInfo(int id, string userName)
        {
            if (id == 0)
            {
                return RedirectToAction("List", DefaultController);
            }

            var criteria = new UpdateAffiliateCriteria
            {
                PartnerID = id,
                UserName = userName
            };
            var affiliates = baseService.GetData<Affiliate>(new GetAffiliateByNameCriteria
            {
                SystemId = currentSession.SystemId,
                UserName = userName,
                PageIndex = 1,
                PageSize = 1
            });
            if (affiliates != null && affiliates.Any())
            {
                var affiliate = affiliates.First();
                ViewBag.CurrentUser = " of \"" + userName + "\"";
                criteria.Email = affiliate.ContactEmail;
                criteria.Website = affiliate.Site;
                criteria.Note = affiliate.Remark;
            }

            return View(criteria);
        }

        [HttpPost]
        public JsonResult EditAffiliateInfo(UpdateAffiliateCriteria criteria)
        {
            var result = new CommandResult();
            if (ModelState.IsValid)
            {
                criteria.UpdatedBy = currentSession.UserId;
                criteria.Email = criteria.Email ?? string.Empty;
                criteria.Website = criteria.Website ?? string.Empty;
                criteria.Note = criteria.Note ?? string.Empty;

                result = baseService.ExecuteNonQuery(criteria);
                if (result.IsSuccessful)
                {
                    result.Message = ViewResources.ActionResult_UpdateAffiliateInfoSuccess;
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditSetting(int id, string name)
        {
            if (id == 0)
            {
                return RedirectToAction("List", DefaultController);
            }

            ViewBag.CurrentUser = " of \"" + name + "\"";
            ViewBag.AffId = id;

            var affiliateProfile = baseService.GetDataEntity<LicenseeProfile>(new GetAffiliateProfileByIDCriteria { PartnerID = id });
            if (affiliateProfile != null)
            {
                ViewBag.NumberOfIPs = affiliateProfile.NumberOfIPs;
            }

            // Domain
            var domainSelList = affiliateService.GetDomainByAffId(id);
            ViewData["slAssignedDomain"] = domainSelList;

            var domainList = affiliateService.GetDomain();
            domainList = domainList.Where(d => !domainSelList.Any(s => s.Value == d.Value)).ToList();
            ViewData["slDomain"] = domainList;

            // IP
            ViewBag.IpList = affiliateService.GetPartnerIpByAffId(id);

            return View();
        }

        [HttpPost]
        public JsonResult EditSetting(int affId, string domains, string listIPs)
        {
            var result = new CommandResult();
            var fromIPs = new StringBuilder();
            var toIPs = new StringBuilder();

            if (!string.IsNullOrEmpty(listIPs))
            {
                var arrIps = listIPs.Split(',');
                const byte Iterator = 2;
                for (int i = 0; i < arrIps.Length; i += Iterator)
                {
                    fromIPs.Append(arrIps[i] + ",");
                    toIPs.Append(arrIps[i + 1] + ",");
                }
            }

            if (!IsValidListOfIPs(fromIPs.ToString(), toIPs.ToString()))
            {
                result.Code = 1;
                result.Message = ViewResources.Error_InvalidInput;
            }
            else
            {
                var criteria = new UpdateSettingCriteria
                {
                    PartnerID = affId,
                    Domains = domains,
                    IpsFrom = fromIPs.ToString(),
                    IpsTo = toIPs.ToString()
                };

                result = baseService.ExecuteNonQuery(criteria);
                if (result.IsSuccessful)
                {
                    ViewBag.Success = true;
                    ViewBag.PartnerID = criteria.PartnerID;
                    result.Message = ViewResources.ActionResult_UpdateAffiliateInfoSuccess;
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string EditIPSetting(int affId, string fromIP, string toIP, string fromIPOld, string toIPOld)
        {
            var criteria = new UpdateSettingIPCriteria
            {
                PartnerID = affId,
                IpFrom = fromIP,
                IpTo = toIP,
                IpFromOld = fromIPOld,
                IpToOld = toIPOld
            };

            var result = baseService.ExecuteNonQuery(criteria);
            if (result != null && result.IsSuccessful)
            {
                ViewBag.Success = true;
                ViewBag.PartnerID = criteria.PartnerID;
                return ViewResources.SuccessfulMessage;
            }
            else
            {
                return result != null ? result.Message : ViewResources.InternalErrorMessage;
            }
        }

        [HttpPost]
        public JsonResult DeleteIPRange_EditSetting_Partner(DeleteSettingIP_AffiliateCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.IPFromOld = string.IsNullOrEmpty(criteria.IPFromOld) ? string.Empty : criteria.IPFromOld;
            criteria.IPToOld = string.IsNullOrEmpty(criteria.IPToOld) ? string.Empty : criteria.IPToOld;
            criteria.IP = currentContext.IP;

            CommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result.IsSuccessful)
            {
                result.Message = ViewResources.ActionResult_Success_DeleteIPSetting;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditProfile(int id, string name)
        {
            if (id == 0)
            {
                return RedirectToAction("List", DefaultController);
            }

            var criteria = new UpdateProfileCriteria { PartnerID = id };
            ViewBag.CurrentUser = $" of \"{name}\"";

            // Profile
            var affiliateProfile = baseService.GetDataEntity<LicenseeProfile>(new GetAffiliateProfileByIDCriteria { PartnerID = id });
            if (affiliateProfile != null)
            {
                criteria.OddsType = affiliateProfile.OddsType;
                criteria.OddsRegion = affiliateProfile.OddsRegion;
                criteria.LanguageList = affiliateProfile.LanguageList;
                criteria.SportIDList = affiliateProfile.SportIdList;
                criteria.CountryCodeList = affiliateProfile.CountryCodeList;
                criteria.BetTypeIDList = affiliateProfile.BetTypeIdList;
            }

            if (IsAccountManagerRole())
            {
                ViewBag.HasAccountManagerPermission = true;
                GetAffiliateProfileWithAccountManageRole(affiliateProfile, ref criteria);
            }
            else if (IsLicenseeAdminRole())
            {
                GetAffiliateProfileWithLicenseeAdminRole(ref criteria);
            }
            else
            {
                // Sub-Acc
                GetAffiliateProfileWithSubAccountRole(ref criteria);
            }

            return View(criteria);
        }

        [HttpPost]
        public JsonResult EditProfile(UpdateProfileCriteria criteria)
        {
            var result = new CommandResult();
            if (ModelState.IsValid)
            {
                CustomizeDataBeforeEditProfile(ref criteria);

                result = baseService.ExecuteNonQuery(criteria);
                if (result.IsSuccessful)
                {
                    result.Message = ViewResources.ActionResult_UpdateAffiliateInfoSuccess;
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string EditActive(int affId, bool status)
        {
            var criteria = new UpdateActiveCriteria
            {
                PartnerID = affId,
                IsAllowed = status,
                Creator = currentSession.UserId
            };
            var result = baseService.ExecuteNonQuery(criteria);

            return result.IsSuccessful ? ViewResources.SuccessfulMessage : result.Message;
        }

        #endregion Edit

        #region Get

        [HttpGet]
        [Compress]
        public ActionResult List()
        {
            ViewBag.SystemName = currentSession.SystemName;
            ViewBag.IsActivateSystem = licenseeService.GetActiveBySystemId(currentSession.SystemId);
            return View("AffiliateList");
        }

        [Compress]
        public JsonResult GetAffiliateList(GetAffiliateByNameCriteria criteria)
        {
            criteria.SystemId = currentSession.SystemId;
            criteria.UserName = (criteria.UserName == null || criteria.UserName == "Username") ? string.Empty : criteria.UserName.Trim();
            criteria.PageIndex++;

            var dr = affiliateService.GetAffiliateListReturnDataCollection(criteria);
            var result = FanexGridHelper.DataForFanexGrid_PreventContainQuote(dr);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string ListExistedAff(string username)
        {
            var criteria = new GetTop10AffiliateCriteria
            {
                SystemId = currentSession.SystemId,
                Username = username
            };
            var affiliates = baseService.GetData<Affiliate>(criteria);
            return string.Join(",", affiliates.Select(x => x.Name));
        }

        public JsonResult ListExistedAffJson(string username)
        {
            var criteria = new GetTop10AffiliateCriteria
            {
                SystemId = currentSession.SystemId,
                Username = username
            };
            var affiliates = baseService.GetData<Affiliate>(criteria);
            return Json(affiliates.Select(x => new JSONSource { Value = x.Id.ToString(), Label = x.Name }).ToList());
        }

        public JsonResult ListExistedDomainsJson(string domainName)
        {
            var criteria = new GetTop10DomainCriteria
            {
                SystemId = currentSession.SystemId,
                DomainName = domainName
            };
            var domains = baseService.GetData<Domain>(criteria);
            return Json(domains.Select(x => new JSONSource { Value = x.DomainId.ToString(), Label = x.DomainName }).ToList());
        }

        [HttpGet]
        public ActionResult Requests(string curmonth = "")
        {
            ViewBag.SystemName = currentSession.SystemName;
            if (curmonth != string.Empty)
            {
                var curDate = DateTime.Now;
                ViewBag.StartDateConfig = new DateTime(curDate.Year, curDate.Month, 1);
            }

            return View("AffiliateRequest", new RequestStatisticCriteria());
        }

        public JsonResult GetPartnerRequestStatistic(RequestStatisticCriteria criteria)
        {
            if (string.IsNullOrEmpty(criteria.PartnerName) || criteria.PartnerName.ToLowerInvariant() == ViewResources.Affiliate_Name.ToLowerInvariant())
            {
                criteria.PartnerName = string.Empty;
            }

            if (string.IsNullOrEmpty(criteria.DomainName) || criteria.DomainName.ToLowerInvariant() == ViewResources.Domain_Name.ToLowerInvariant())
            {
                criteria.DomainName = string.Empty;
            }

            criteria.SystemId = currentSession.SystemId;
            criteria.PageIndex++;

            var dr = affiliateService.GetPartnerRequestStatisticReturnDataCollection(criteria);
            var result = FanexGridHelper.DataForFanexGrid(dr);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewXMLURLs_Edit(int id, string name)
        {
            if (id == 0)
            {
                return RedirectToAction("List", DefaultController);
            }

            var affiliateProfile = baseService.GetDataEntity<LicenseeProfile>(new GetAffiliateProfileByIDCriteria { PartnerID = id });
            var sports = baseService.GetData<Sport>(new GetSupportedSportCriteria());
            if (affiliateProfile.SportIdList.Equals("all"))
            {
                ViewBag.SportIds = sports.Select(item => item.SportType.ToString()).ToList();
            }
            else
            {
                ViewBag.SportIds = affiliateProfile.SportIdList.Split(',');
            }

            ViewBag.CurrentUser = name;
            ViewBag.AffiliateName = name;
            ViewBag.AssignedDomain = affiliateService.GetDomainByAffId(id);
            return View();
        }

        public string GetXMLOddsFeedLink(string domainURL, string affiliateName)
        {
            return string.IsNullOrEmpty(domainURL)
                ? string.Empty
                : URLPrefix + domainURL.Replace(URLPrefix, string.Empty) + "/" + GetVirtualPath() + affiliateName + "/odds.xml";
        }

        public string GetXMLOddsFeed_HelpLink(string domainURL, string affiliateName)
        {
            return string.IsNullOrEmpty(domainURL) ?
                string.Empty :
                URLPrefix + domainURL.Replace(URLPrefix, string.Empty) + "/" + GetVirtualPath() + affiliateName + "/help.htm";
        }

        public string GetBetRadarLink(string domainURL, string affiliateName)
        {
            return string.IsNullOrEmpty(domainURL) ?
                string.Empty :
                URLPrefix + domainURL.Replace(URLPrefix, string.Empty) + "/" + GetVirtualPath() + affiliateName + "/oddsfeed.xml";
        }

        #endregion Get

        #region Delete

        public string Delete(int affId)
        {
            string resultMessage;
            var criteria = new DeleteAffiliateCriteria
            {
                Creator = currentSession.UserId,
                PartnerID = affId
            };

            var result = baseService.ExecuteNonQuery(criteria);
            if (result != null)
            {
                resultMessage = result.IsSuccessful ? ViewResources.SuccessfulMessage : result.Message;
            }
            else
            {
                resultMessage = ViewResources.InternalErrorMessage;
            }

            return resultMessage;
        }

        #endregion Delete

        #region Common

        private bool IsValidListOfIPs(string ipsFrom, string ipsTo)
        {
            uint totalIPs = 0;
            var arrIpsFrom = ipsFrom.Split(',');
            var arrIpsTo = ipsTo.Split(',');

            var config = baseService.GetData<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = "Config" });
            var maxNumOfIPs = Converter.ToInt(config.First(m => m.Key == "MaximumNumberOfIPs").Value);

            for (int i = 0; i < arrIpsFrom.Length; i++)
            {
                var ipFrom = Utilities.ConvertIPToUint(arrIpsFrom[i]);
                var ipTo = Utilities.ConvertIPToUint(arrIpsTo[i]);

                if (ipTo > 0 && ipFrom > 0)
                {
                    totalIPs += ipTo - ipFrom + 1;
                }
            }

            return totalIPs <= maxNumOfIPs;
        }

        private void CustomizeDataBeforeAddNew(ref InsertAffiliateCriteria criteria)
        {
            var fromIPs = new StringBuilder();
            var toIPs = new StringBuilder();

            if (!string.IsNullOrEmpty(criteria.IpsFrom))
            {
                var arrIps = criteria.IpsFrom.Split(',');
                const byte Iterator = 2;
                for (int i = 0; i < arrIps.Length; i += Iterator)
                {
                    fromIPs.Append(arrIps[i] + ",");
                    toIPs.Append(arrIps[i + 1] + ",");
                }
            }

            criteria.SystemID = currentSession.SystemId;
            criteria.Creator = currentSession.UserId;
            criteria.IpsFrom = fromIPs.ToString();
            criteria.IpsTo = toIPs.ToString();
            criteria.Ip = currentContext.IP;
            criteria.Content = criteria.Content ?? string.Empty;
            criteria.Note = criteria.Note ?? string.Empty;
            criteria.Domains = criteria.Domains ?? string.Empty;
            criteria.BetTypeIDList = criteria.BetTypeIDList ?? string.Empty;
            criteria.LeagueIDList = criteria.LeagueIDList ?? string.Empty;
            criteria.OutRightIDList = criteria.OutRightIDList ?? string.Empty;
            criteria.LeagueKeyword = criteria.LeagueKeyword ?? string.Empty;
            criteria.LeagueKeyword_O = criteria.LeagueKeyword_O ?? string.Empty;

            criteria.SportIDList = UpdateSportIDListForCricket(criteria.SportIDList);
            criteria.LeagueIDList = GetLeagueIds(criteria.SportIDList, criteria.CountryCodeList, criteria.LeagueIDList);
            criteria.OutRightIDList = GetOutRightIds(criteria.SportIDList, criteria.CountryCodeList, criteria.OutRightIDList);
            criteria.SportIDList = GetSportIds(criteria.SportIDList);
            criteria.CountryCodeList = GetCountryCodes(criteria.CountryCodeList);
            criteria.BetTypeIDList = GetBetTypeIds(criteria.BetTypeIDList);
        }

        private void CustomizeDataBeforeEditProfile(ref UpdateProfileCriteria criteria)
        {
            criteria.OddsType = criteria.OddsType ?? string.Empty;
            criteria.OddsRegion = criteria.OddsRegion ?? string.Empty;
            criteria.LanguageList = criteria.LanguageList ?? string.Empty;
            criteria.SportIDList = criteria.SportIDList ?? string.Empty;
            criteria.CountryCodeList = criteria.CountryCodeList ?? string.Empty;
            criteria.BetTypeIDList = criteria.BetTypeIDList ?? string.Empty;

            criteria.Creator = currentSession.UserId;

            criteria.SportIDList = UpdateSportIDListForCricket(criteria.SportIDList);

            criteria.LeagueIDList = GetLeagueIds(criteria.SportIDList, criteria.CountryCodeList, criteria.LeagueIDList);
            criteria.OutRightIDList = GetOutRightIds(criteria.SportIDList, criteria.CountryCodeList, criteria.OutRightIDList);
            criteria.SportIDList = GetSportIds(criteria.SportIDList);
            criteria.CountryCodeList = GetCountryCodes(criteria.CountryCodeList);
            criteria.BetTypeIDList = GetBetTypeIds(criteria.BetTypeIDList);
        }

        private static string UpdateSportIDListForCricket(string sportIDList)
        {
            if (sportIDList == null)
            {
                return null;
            }

            var sportList = sportIDList.Split(',').ToList();

            if (sportList.Contains("27") && !sportList.Contains("50"))
            {
                sportList.Add("50");
            }

            if (!sportList.Contains("27") && sportList.Contains("50"))
            {
                sportList.Remove("50");
            }

            return string.Join(",", sportList.ToArray());
        }

        private string GetLeagueIds(string sportIds, string countryCodes, string leagueIds)
        {
            if (!string.IsNullOrWhiteSpace(leagueIds))
            {
                var leagues = baseService.GetData<League>(new GetLeagueCriteria
                {
                    SportIDList = sportIds,
                    CountryCodeList = countryCodes
                });
                var leagueList = leagues.Select(x => new SelectListItem { Value = x.LeagueId.ToString(), Text = x.LeagueName }).ToList();
                if (Utilities.IsExistInSelectListItem(leagueIds, leagueList))
                {
                    return "all";
                }
            }

            return leagueIds;
        }

        private string GetOutRightIds(string sportIds, string countryCodes, string outRightIds)
        {
            if (!string.IsNullOrWhiteSpace(outRightIds))
            {
                var outrights = baseService.GetData<League>(new GetOutRightCriteria
                {
                    SportIDList = sportIds,
                    CountryCodeList = countryCodes
                });
                var outrightList = outrights.Select(x => new SelectListItem { Value = x.LeagueId.ToString(), Text = x.LeagueName }).ToList();
                if (Utilities.IsExistInSelectListItem(outRightIds, outrightList))
                {
                    return "all";
                }
            }

            return outRightIds;
        }

        private string GetSportIds(string sportIds)
        {
            if (!string.IsNullOrWhiteSpace(sportIds))
            {
                var sports = baseService.GetData<Sport>(new GetSupportedSportCriteria());
                var sportList = sports.Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();
                if (Utilities.IsExistInSelectListItem(sportIds, sportList))
                {
                    return "all";
                }
            }

            return sportIds;
        }

        private string GetCountryCodes(string countryCodes)
        {
            if (!string.IsNullOrWhiteSpace(countryCodes))
            {
                var countries = baseService.GetData<Country>(new GetAllCountryCriteria());
                var countryList = countries.Select(x => new SelectListItem { Value = x.CountryCode, Text = x.CountryName }).ToList();
                if (Utilities.IsExistInSelectListItem(countryCodes, countryList))
                {
                    return "all";
                }
            }

            return countryCodes;
        }

        private string GetBetTypeIds(string betTypeIds)
        {
            if (!string.IsNullOrWhiteSpace(betTypeIds))
            {
                var bettypes = baseService.GetData<BetType>(new GetSupportedBettypeCriteria());
                var bettypeList = bettypes.Select(x => new SelectListItem { Value = x.TypeId.ToString(), Text = x.TypeNameE }).ToList();
                if (Utilities.IsExistInSelectListItem(betTypeIds, bettypeList))
                {
                    return "all";
                }
            }

            return betTypeIds;
        }

        private void ViewBagForFlags(
            bool oddsTypeFlag,
            bool oddsStyleFlag,
            bool languagesFlag,
            bool sportsFlag,
            bool countriesFlag,
            bool marketsFlag)
        {
            ViewBag.OddsTypeFlag = oddsTypeFlag;
            ViewBag.OddsStyleFlag = oddsStyleFlag;
            ViewBag.LanguagesFlag = languagesFlag;
            ViewBag.SportsFlag = sportsFlag;
            ViewBag.CountriesFlag = countriesFlag;
            ViewBag.MarketsFlag = marketsFlag;
        }

        #region Account Manager

        private void AddAffiliateWithAccountManagerRole(LicenseeProfile licenseeProfile, ref InsertAffiliateCriteria criteria)
        {
            if (licenseeProfile != null)
            {
                criteria.OddsType = licenseeProfile.OddsType;
                criteria.LanguageList = licenseeProfile.LanguageList;
                criteria.SportIDList = licenseeProfile.SportIdList;
                criteria.CountryCodeList = licenseeProfile.CountryCodeList;
                criteria.LeagueIDList = licenseeProfile.LeagueIdList;
                criteria.OutRightIDList = licenseeProfile.OutrightIdList;
                criteria.BetTypeIDList = licenseeProfile.BetTypeIdList;
                criteria.LeagueKeyword = licenseeProfile.LeagueKeyword;
                criteria.LeagueKeyword_O = licenseeProfile.LeagueKeyword_O;

                ViewBag.NumberOfIPs = licenseeProfile.NumberOfIPs;

                ViewBagForFlags(true, true, true, true, true, true);

                ProfileForAddAffiliateWithAccountManagerRole(ref criteria);
            }
        }

        private void ProfileForAddAffiliateWithAccountManagerRole(ref InsertAffiliateCriteria criteria)
        {
            var oddsTypes = baseService.GetData<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = OddsTypeSettingType })
                .Where(x => x.Key != "Live" || (x.Key == "Live" && IsAccountManagerRole())).ToList();
            ViewBag.OddsTypeList = oddsTypes.Select(x => new SelectListItem { Value = x.Value, Text = x.Key }).ToList();

            var oddsStyles = baseService.GetData<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = OddsRegionSettingType });
            ViewBag.OddsStyleList = oddsStyles.Select(x => new SelectListItem { Value = x.Value, Text = x.Key }).ToList();

            var languages = baseService.GetData<Language>(new GetAllLanguageCriteria());
            ViewBag.LanguageIDList = languages.Select(x => new SelectListItem { Value = x.Code, Text = x.Name_EN }).ToList();

            ViewBag.DelayTimeList = GenerateDelayTime();
            ViewBag.ResultDelayTimeList = licenseeService.GetResultDelayTimeList();

            // Country
            var countries = baseService.GetData<Country>(new GetAllCountryCriteria());
            ViewBag.CountriesList = countries.Select(x => new SelectListItem { Value = x.CountryCode, Text = x.CountryName }).ToList();
            if (criteria.CountryCodeList.Equals("all"))
            {
                criteria.CountryCodeList = string.Join(",", countries.Select(item => item.CountryCode));
            }

            // Sport
            var sports = UpdateSportListForCricket(baseService.GetData<Sport>(new GetSupportedSportCriteria()));
            if (criteria.SportIDList.Equals("all"))
            {
                criteria.SportIDList = string.Join(",", sports.Select(item => item.SportType));
            }

            ViewBag.MajorSportList = sports.Where(sport => sport.Group == MajorGroup)
                .Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();
            ViewBag.MinorSportList = sports.Where(sport => sport.Group == MinorGroup)
                .Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();

            // Market
            var bettypes = baseService.GetData<BetType>(new GetSupportedBettypeCriteria());
            if (criteria.BetTypeIDList.Equals("all"))
            {
                criteria.BetTypeIDList = string.Join(",", bettypes.Select(item => item.TypeId));
            }

            ViewBag.MajorBettypesList = bettypes.Where(x => x.Group == MajorGroup)
                .Select(x => new SelectListItem { Value = x.TypeId.ToString(), Text = x.TypeNameE }).ToList();
            ViewBag.MinorBettypesList = bettypes.Where(x => x.Group == MinorGroup)
                .Select(x => new SelectListItem { Value = x.TypeId.ToString(), Text = x.TypeNameE }).ToList();

            // League
            var leagues = baseService.GetData<League>(new GetLeagueCriteria
            {
                SportIDList = criteria.SportIDList,
                CountryCodeList = criteria.CountryCodeList
            });
            ViewBag.LeagueList = leagues.Select(x => new SelectListItem { Value = x.LeagueId.ToString(), Text = x.LeagueName }).ToList();
            if (criteria.LeagueIDList.Equals("all"))
            {
                criteria.LeagueIDList = string.Join(",", leagues.Select(item => item.LeagueId));
            }

            // OutRight
            var outrights = baseService.GetData<League>(new GetOutRightCriteria
            {
                SportIDList = criteria.SportIDList,
                CountryCodeList = criteria.CountryCodeList
            });
            ViewBag.OutrightList = outrights.Select(x => new SelectListItem { Value = x.LeagueId.ToString(), Text = x.LeagueName }).ToList();
            if (criteria.OutRightIDList.Equals("all"))
            {
                criteria.OutRightIDList = string.Join(",", outrights.Select(item => item.LeagueId));
            }
        }

        private void GetAffiliateProfileWithAccountManageRole(LicenseeProfile affiliateProfile, ref UpdateProfileCriteria criteria)
        {
            if (affiliateProfile != null)
            {
                // Delay Times
                var delayTimes = affiliateProfile.OddsDelayTimes.Split(',');
                criteria.DeadballDelayTimes = delayTimes.Length > 0 ? delayTimes[0] : "0";
                criteria.LiveDelayTimes = delayTimes.Length > 1 ? delayTimes[1] : "0";
                criteria.OutrightDelayTimes = delayTimes.Length > OutrightDelayTimesIndex ? delayTimes[OutrightDelayTimesIndex] : "0";
                criteria.ResultDelayTimes = affiliateProfile.ResultDelayTimes;

                criteria.NumberOfIPs = affiliateProfile.NumberOfIPs;
                criteria.LeagueKeyword = affiliateProfile.LeagueKeyword;
                criteria.LeagueKeyword_O = affiliateProfile.LeagueKeyword_O;
                criteria.LeagueIDList = affiliateProfile.LeagueIdList;
                criteria.OutRightIDList = affiliateProfile.OutrightIdList;
                ViewBag.ActualNumberOfIps = affiliateProfile.ActualNumberOfIPs;
            }

            var licenseeProfile = baseService.GetDataEntity<LicenseeProfile>(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = currentSession.SystemId });
            if (licenseeProfile != null)
            {
                ViewBag.NumberOfIPs = licenseeProfile.NumberOfIPs;
                ViewBagForFlags(true, true, true, true, true, true);

                ProfileForEditAffiliateWithAccountManagerRole(ref criteria);
            }
        }

        private void ProfileForEditAffiliateWithAccountManagerRole(ref UpdateProfileCriteria criteria)
        {
            var oddsTypes = baseService.GetData<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = OddsTypeSettingType });
            ViewBag.OddsTypeList = oddsTypes.Select(x => new SelectListItem { Value = x.Value, Text = x.Key }).ToList();

            var oddsStyles = baseService.GetData<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = OddsRegionSettingType });
            ViewBag.OddsStyleList = oddsStyles.Select(x => new SelectListItem { Value = x.Value, Text = x.Key }).ToList();

            var languages = baseService.GetData<Language>(new GetAllLanguageCriteria());
            ViewBag.LanguageIDList = languages.Select(x => new SelectListItem { Value = x.Code, Text = x.Name_EN }).ToList();

            ViewBag.DelayTimeList = GenerateDelayTime();
            ViewBag.ResultDelayTimeList = licenseeService.GetResultDelayTimeList();

            // Sport
            var sports = UpdateSportListForCricket(baseService.GetData<Sport>(new GetSupportedSportCriteria()));
            if (criteria.SportIDList.Equals("all"))
            {
                criteria.SportIDList = string.Join(",", sports.Select(item => item.SportType));
            }

            ViewBag.MajorSportList = sports.Where(sport => sport.Group == MajorGroup)
                .Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();
            ViewBag.MinorSportList = sports.Where(sport => sport.Group == MinorGroup)
                .Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();

            // Market
            var bettypes = baseService.GetData<BetType>(new GetSupportedBettypeCriteria());
            if (criteria.BetTypeIDList.Equals("all"))
            {
                criteria.BetTypeIDList = string.Join(",", bettypes.Select(item => item.TypeId));
            }

            ViewBag.MajorBettypesList = bettypes.Where(x => x.Group == MajorGroup)
                .Select(x => new SelectListItem { Value = x.TypeId.ToString(), Text = x.TypeNameE }).ToList();
            ViewBag.MinorBettypesList = bettypes.Where(x => x.Group == MinorGroup)
                .Select(x => new SelectListItem { Value = x.TypeId.ToString(), Text = x.TypeNameE }).ToList();

            // Country
            var countries = baseService.GetData<Country>(new GetAllCountryCriteria());
            ViewBag.CountriesList = countries.Select(x => new SelectListItem { Value = x.CountryCode, Text = x.CountryName }).ToList();
            if (criteria.CountryCodeList.Equals("all"))
            {
                criteria.CountryCodeList = string.Join(",", countries.Select(item => item.CountryCode));
            }

            // OutRight
            var outrights = baseService.GetData<League>(new GetOutRightCriteria
            {
                SportIDList = criteria.SportIDList,
                CountryCodeList = criteria.CountryCodeList
            });
            ViewBag.OutrightList = outrights.Select(x => new SelectListItem { Value = x.LeagueId.ToString(), Text = x.LeagueName }).ToList();
            if (criteria.OutRightIDList.Equals("all"))
            {
                criteria.OutRightIDList = string.Join(",", outrights.Select(item => item.LeagueId));
            }

            // League
            var leagues = baseService.GetData<League>(new GetLeagueCriteria
            {
                SportIDList = criteria.SportIDList,
                CountryCodeList = criteria.CountryCodeList
            });
            ViewBag.LeagueList = leagues.Select(x => new SelectListItem { Value = x.LeagueId.ToString(), Text = x.LeagueName }).ToList();
            if (criteria.LeagueIDList.Equals("all"))
            {
                criteria.LeagueIDList = string.Join(",", leagues.Select(item => item.LeagueId));
            }
        }

        #endregion Account Manager

        #region LicenseeAdmin/Sub-Acc

        private void AddAffiliateWithLicenseeAdminRole(LicenseeProfile licenseeProfile)
        {
            if (licenseeProfile != null)
            {
                var oddStyles = licenseeProfile.OddsType;
                ViewBag.NumberOfIPs = licenseeProfile.NumberOfIPs;
                ViewBagForFlags(
                    licenseeProfile.OddsTypeFlag,
                    licenseeProfile.OddsRegionFlag,
                    licenseeProfile.LanguageListFlag,
                    licenseeProfile.SportIdListFlag,
                    licenseeProfile.CountryCodeListFlag,
                    licenseeProfile.BetTypeIdListFlag && (oddStyles != "8" && !string.IsNullOrWhiteSpace(oddStyles)));

                ProfileForAddAffiliateWithLicenseeAdminAndSubAccountRole();
            }
        }

        private void GetAffiliateProfileWithLicenseeAdminRole(ref UpdateProfileCriteria criteria)
        {
            var licenseeProfile = baseService.GetDataEntity<LicenseeProfile>(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = currentSession.SystemId });
            if (licenseeProfile != null)
            {
                var oddStyles = licenseeProfile.OddsType;
                ViewBag.NumberOfIPs = licenseeProfile.NumberOfIPs;
                ViewBag.OddsTypeFlag = licenseeProfile.OddsTypeFlag;
                ViewBag.OddsStyleFlag = licenseeProfile.OddsRegionFlag;
                ViewBag.LanguagesFlag = licenseeProfile.LanguageListFlag;
                ViewBag.SportsFlag = licenseeProfile.SportIdListFlag;
                ViewBag.CountriesFlag = licenseeProfile.CountryCodeListFlag;
                ViewBag.MarketsFlag = licenseeProfile.BetTypeIdListFlag && (oddStyles != "8" && !string.IsNullOrWhiteSpace(oddStyles));

                ProfileForEditAffiliateWithLicenseeAdminAndSubAccountRole(ref criteria);
            }
        }

        private void AddAffiliateWithSubAccountRole()
        {
            var userProfile = baseService.GetDataEntity<PermissionFlag>(new UserGetPermissionCriteria
            {
                SystemID = currentSession.SystemId,
                UserID = currentSession.UserId
            });

            if (userProfile != null)
            {
                ViewBag.NumberOfIPs = userProfile.NumberOfIPs;
                ViewBagForFlags(
                    userProfile.OddsTypeFlag,
                    userProfile.OddsRegionFlag,
                    userProfile.LanguageListFlag,
                    userProfile.SportIDListFlag,
                    userProfile.CountryCodeListFlag,
                    userProfile.BetTypeIDListFlag);

                ProfileForAddAffiliateWithLicenseeAdminAndSubAccountRole();
            }
        }

        private void GetAffiliateProfileWithSubAccountRole(ref UpdateProfileCriteria criteria)
        {
            var userProfile = baseService.GetDataEntity<PermissionFlag>(new UserGetPermissionCriteria
            {
                SystemID = currentSession.SystemId,
                UserID = currentSession.UserId
            });

            if (userProfile != null)
            {
                ViewBag.NumberOfIPs = userProfile.NumberOfIPs;
                ViewBag.OddsTypeFlag = userProfile.OddsTypeFlag;
                ViewBag.OddsStyleFlag = userProfile.OddsRegionFlag;
                ViewBag.LanguagesFlag = userProfile.LanguageListFlag;
                ViewBag.SportsFlag = userProfile.SportIDListFlag;
                ViewBag.CountriesFlag = userProfile.CountryCodeListFlag;
                ViewBag.MarketsFlag = userProfile.BetTypeIDListFlag;

                ProfileForEditAffiliateWithLicenseeAdminAndSubAccountRole(ref criteria);
            }
        }

        private void ProfileForAddAffiliateWithLicenseeAdminAndSubAccountRole()
        {
            // OddsType
            ViewBag.OddsTypeList = baseService.GetSelectList(
                    new GetTypeSettingCriteria { SystemID = currentSession.SystemId, Type = OddsTypeSettingType },
                    false)
                .Where(item => item.Text != "Live" || (item.Text == "Live" && IsAccountManagerRole()));

            // OddsStyle
            ViewBag.OddsStyleList = baseService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = OddsRegionSettingType }, false);

            // Language
            ViewBag.LanguageIDList = affiliateService.GetLanguageBySystemId();

            // Market
            var bettypesList = affiliateService.GetBettypeBySystemId();
            ViewBag.MajorBetTypesList = bettypesList.Where(bettype => bettype.Group.Name == MajorGroup);
            ViewBag.MinorBetTypesList = bettypesList.Where(bettype => bettype.Group.Name == MinorGroup);

            // Country
            ViewBag.CountriesList = affiliateService.GetCountryBySystemId();

            // Sport
            var sports = UpdateSportListForCricket(baseService.GetData<Sport>(new GetSportBySystemIDCriteria { SystemID = currentSession.SystemId }).OrderBy(s => s.SportName));
            ViewBag.MajorSportList = sports.Where(sport => sport.Group == MajorGroup)
                .Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();
            ViewBag.MinorSportList = sports.Where(sport => sport.Group == MinorGroup)
                .Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();
        }

        private void ProfileForEditAffiliateWithLicenseeAdminAndSubAccountRole(ref UpdateProfileCriteria criteria)
        {
            // OddsType
            ViewBag.OddsTypeList = baseService
                .GetSelectList(new GetTypeSettingCriteria { SystemID = currentSession.SystemId, Type = OddsTypeSettingType }, false)
                .Where(item => item.Text != "Live" || (item.Text == "Live" && IsAccountManagerRole()));

            // OddsStyle
            ViewBag.OddsStyleList = baseService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = OddsRegionSettingType }, false);

            // Language
            ViewBag.LanguageIDList = affiliateService.GetLanguageBySystemId();

            // Sport
            var sports = UpdateSportListForCricket(baseService.GetData<Sport>(new GetSportBySystemIDCriteria { SystemID = currentSession.SystemId }).OrderBy(s => s.SportName));
            if (criteria.SportIDList.Equals("all"))
            {
                criteria.SportIDList = string.Join(",", sports.Select(item => item.SportType));
            }
            ViewBag.MajorSportList = sports.Where(sport => sport.Group == MajorGroup)
                .Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();
            ViewBag.MinorSportList = sports.Where(sport => sport.Group == MinorGroup)
                .Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();

            // Market
            var bettypes = affiliateService.GetBettypeBySystemId();
            if (criteria.BetTypeIDList.Equals("all"))
            {
                criteria.BetTypeIDList = string.Join(",", bettypes.Select(item => item.Value));
            }
            ViewBag.MajorBetTypesList = bettypes.Where(betType => betType.Group.Name == MajorGroup);
            ViewBag.MinorBetTypesList = bettypes.Where(betType => betType.Group.Name == MinorGroup);

            // Country
            var countries = affiliateService.GetCountryBySystemId();
            ViewBag.CountriesList = countries;
            if (criteria.CountryCodeList.Equals("all"))
            {
                criteria.CountryCodeList = string.Join(",", countries.Select(item => item.Value));
            }
        }

        private static IEnumerable<Sport> UpdateSportListForCricket(IEnumerable<Sport> sports)
        {
            const byte Cricket = 50;
            return sports.Where(s => s.SportType != Cricket).ToList();
        }

        #endregion LicenseeAdmin/Sub-Acc

        #endregion Common
    }
}