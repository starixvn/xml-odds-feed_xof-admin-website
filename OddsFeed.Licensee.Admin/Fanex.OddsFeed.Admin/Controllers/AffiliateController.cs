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
        private const string OddsRegionCollumn = "OddsRegion";
        private const string OddsTypeCollumn = "OddsType";
        private const string NumberOfIpsCollumn = "NumberOfIPs";
        private const string LanguageListFlagCollumn = "LanguageListFlag";
        private const string CountryCodeListFlagCollumn = "CountryCodeListFlag";
        private const string BettypeIDListFlagCollumn = "BettypeIDListFlag";
        private const string SportIDListFlagCollumn = "SportIDListFlag";
        private const string OddsTypeFlagCollumn = "OddsTypeFlag";
        private const string OddsRegionFlagCollumn = "OddsRegionFlag";
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
            var licenseeProfile = baseService.GetDataSet(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = currentSession.SystemId });
            if (licenseeProfile != null && licenseeProfile.Tables.Count > 0 && licenseeProfile.Tables[0].Rows.Count > 0)
            {
                var row = licenseeProfile.Tables[0].Rows[0];
                criteria.OddsRegion = row[OddsRegionCollumn].ToString();
                criteria.NumberOfIPs = (short)row[NumberOfIpsCollumn];

                // Delay Times
                var delayTimes = row["OddsDelayTimes"].ToString().Split(',');
                criteria.DeadballDelayTimes = delayTimes.Length > 0 ? delayTimes[0] : "0";
                criteria.LiveDelayTimes = delayTimes.Length > 1 ? delayTimes[1] : "0";
                criteria.OutrightDelayTimes = delayTimes.Length > OutrightDelayTimesIndex ? delayTimes[OutrightDelayTimesIndex] : "0";
                criteria.ResultDelayTimes = row["ResultDelayTimes"].ToString();
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
            var actionResult = new OddsFeedActionResult();
            CustomizeDataBeforeAddNew(ref criteria);

            if (IsValidListOfIPs(criteria.IpsFrom, criteria.IpsTo))
            {
                var result = affiliateService.Insert(criteria);
                if (result != null && result.IsSuccessful)
                {
                    actionResult.IsSuccess = true;
                    actionResult.ResultMessage = ViewResources.ActionResult_AddAffiliateSuccess;
                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.ResultMessage = result != null ? result.Message : ViewResources.InternalErrorMessage;
                }
            }
            else
            {
                actionResult.IsSuccess = false;
                actionResult.ResultMessage = ViewResources.Error_InvalidInput;
            }

            return Json(actionResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckExistAffiliate(InsertAffiliateCriteria criteria)
        {
            var checkResult = new OddsFeedActionResult();
            if (string.IsNullOrEmpty(criteria.UserName))
            {
                checkResult.IsSuccess = true;
                return Json(checkResult, JsonRequestBehavior.AllowGet);
            }

            var res = affiliateService.GetAffiliateListReturnDataCollection(new CheckExistAffiliateCriteria
            {
                UserName = criteria.UserName,
                PageIndex = 1,
                PageSize = 1,
                SystemId = currentSession.SystemId
            });

            if (res.Data.Tables.Count > 0 &&
                res.Data.Tables[0].Rows.Count > 0)
            {
                checkResult.IsSuccess = false;
            }

            return Json(checkResult, JsonRequestBehavior.AllowGet);
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

            var dr = affiliateService.GetAffiliateListReturnDataCollection(new GetAffiliateByNameCriteria
            {
                SystemId = currentSession.SystemId,
                UserName = userName,
                PageIndex = 1,
                PageSize = 1
            });

            if (dr.Data != null &&
                dr.Data.Tables.Count > 0 &&
                dr.Data.Tables[0].Rows.Count > 0)
            {
                var rawData = dr.Data.Tables[0].Rows[0];
                ViewBag.CurrentUser = " of \"" + userName + "\"";
                criteria.Email = Convert.ToString(rawData["ContactEmail"]);
                criteria.Website = Convert.ToString(rawData["Site"]);
                criteria.Note = Convert.ToString(rawData["Remark"]);
            }

            return View(criteria);
        }

        [HttpPost]
        public JsonResult EditAffiliateInfo(UpdateAffiliateCriteria criteria)
        {
            var actionResult = new OddsFeedActionResult();
            if (ModelState.IsValid)
            {
                criteria.UpdatedBy = currentSession.UserId;
                criteria.Email = criteria.Email ?? string.Empty;
                criteria.Website = criteria.Website ?? string.Empty;
                criteria.Note = criteria.Note ?? string.Empty;

                var result = baseService.ExecuteNonQuery(criteria);
                if (result != null && result.IsSuccessful)
                {
                    actionResult.IsSuccess = true;
                    actionResult.ResultMessage = ViewResources.ActionResult_UpdateAffiliateInfoSuccess;
                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.ResultMessage = result != null ? result.Message : ViewResources.InternalErrorMessage;
                }
            }

            return Json(actionResult, JsonRequestBehavior.AllowGet);
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

            var affiliateProfile = baseService.GetDataSet(new GetAffiliateProfileByIDCriteria { PartnerID = id });
            if (affiliateProfile != null && affiliateProfile.Tables.Count > 0 && affiliateProfile.Tables[0].Rows.Count > 0)
            {
                ViewBag.NumberOfIPs = int.Parse(affiliateProfile.Tables[0].Rows[0][NumberOfIpsCollumn].ToString());
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
            var actionResult = new OddsFeedActionResult();
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
                actionResult.IsSuccess = false;
                actionResult.ResultMessage = ViewResources.Error_InvalidInput;
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

                var result = baseService.ExecuteNonQuery(criteria);
                if (result != null && result.IsSuccessful)
                {
                    ViewBag.Success = true;
                    ViewBag.PartnerID = criteria.PartnerID;
                    actionResult.IsSuccess = true;
                    actionResult.ResultMessage = ViewResources.ActionResult_UpdateAffiliateInfoSuccess;
                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.ResultMessage = result != null ? result.Message : ViewResources.InternalErrorMessage;
                }
            }

            return Json(actionResult, JsonRequestBehavior.AllowGet);
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
            var deleteResult = new OddsFeedActionResult();
            criteria.Creator = currentSession.UserId;
            criteria.IPFromOld = string.IsNullOrEmpty(criteria.IPFromOld) ? string.Empty : criteria.IPFromOld;
            criteria.IPToOld = string.IsNullOrEmpty(criteria.IPToOld) ? string.Empty : criteria.IPToOld;
            criteria.IP = currentContext.IP;

            var result = baseService.ExecuteNonQuery(criteria);
            if (result != null && result.IsSuccessful)
            {
                deleteResult.IsSuccess = true;
                deleteResult.ResultMessage = ViewResources.ActionResult_Success_DeleteIPSetting;
            }
            else
            {
                deleteResult.IsSuccess = false;
                deleteResult.ErrorCode = result != null ? result.Code : 1;
                deleteResult.ResultMessage = result != null ? result.Message : ViewResources.InternalErrorMessage;
            }

            return Json(deleteResult, JsonRequestBehavior.AllowGet);
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
            var affiliateProfile = baseService.GetDataSet(new GetAffiliateProfileByIDCriteria { PartnerID = id });
            if (affiliateProfile != null && affiliateProfile.Tables.Count > 0 && affiliateProfile.Tables[0].Rows.Count > 0)
            {
                var row = affiliateProfile.Tables[0].Rows[0];
                criteria.OddsType = row[OddsTypeCollumn].ToString();
                criteria.OddsRegion = row[OddsRegionCollumn].ToString();
                criteria.LanguageList = row["LanguageList"].ToString();
                criteria.SportIDList = row["SportIDList"].ToString();
                criteria.CountryCodeList = row["CountryCodeList"].ToString();
                criteria.BetTypeIDList = row["BetTypeIDList"].ToString();
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
            var actionResult = new OddsFeedActionResult();
            if (ModelState.IsValid)
            {
                CustomizeDataBeforeEditProfile(ref criteria);

                var result = baseService.ExecuteNonQuery(criteria);
                if (result != null && result.IsSuccessful)
                {
                    actionResult.IsSuccess = true;
                    actionResult.ResultMessage = ViewResources.ActionResult_UpdateAffiliateInfoSuccess;
                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.ResultMessage = result != null ? result.Message : ViewResources.InternalErrorMessage;
                }
            }

            return Json(actionResult, JsonRequestBehavior.AllowGet);
        }

        public string EditActive(int affId, bool status)
        {
            string resultMessage;
            var criteria = new UpdateActiveCriteria
            {
                PartnerID = affId,
                IsAllowed = status,
                Creator = currentSession.UserId
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
            var top10AffList = affiliateService.GetTop10AffReturnDataSet(criteria).Tables[0];
            return DataForAutoComplete(top10AffList, "username");
        }

        public JsonResult ListExistedAffJson(string username)
        {
            var criteria = new GetTop10AffiliateCriteria
            {
                SystemId = currentSession.SystemId,
                Username = username
            };
            var top10DomainList = baseService.GetDataSet(criteria).Tables[0];
            return JsonForAutoComplete(top10DomainList, "PartnerID", "UserName");
        }

        public JsonResult ListExistedDomainsJson(string domainName)
        {
            var criteria = new GetTop10DomainCriteria
            {
                SystemId = currentSession.SystemId,
                DomainName = domainName
            };
            var top10DomainList = baseService.GetDataSet(criteria).Tables[0];
            return JsonForAutoComplete(top10DomainList, "DomainID", "DomainName");
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

            var config = baseService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "Config" }, false);
            var maxNumOfIPs = Converter.ToInt(config.Where(m => m.Text == "MaximumNumberOfIPs").ToList()[0].Value);

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
                var leagues = baseService.GetSelectList(new GetLeagueCriteria
                {
                    SportIDList = sportIds,
                    CountryCodeList = countryCodes
                });
                if (Utilities.IsExistInSelectListItem(leagueIds, leagues))
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
                var outrights = baseService.GetSelectList(new GetOutRightCriteria
                {
                    SportIDList = sportIds,
                    CountryCodeList = countryCodes
                });
                if (Utilities.IsExistInSelectListItem(outRightIds, outrights))
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
                var sports = baseService.GetSelectList(new GetSupportedSportCriteria());
                if (Utilities.IsExistInSelectListItem(sportIds, sports))
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
                var countries = baseService.GetSelectList(new GetAllCountryCriteria());
                if (Utilities.IsExistInSelectListItem(countryCodes, countries))
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
                var bettypes = baseService.GetSelectList(new GetSupportedBettypeCriteria());
                if (Utilities.IsExistInSelectListItem(betTypeIds, bettypes))
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

        private void AddAffiliateWithAccountManagerRole(DataSet licenseeProfile, ref InsertAffiliateCriteria criteria)
        {
            if (licenseeProfile != null && licenseeProfile.Tables.Count > 0 && licenseeProfile.Tables[0].Rows.Count > 0)
            {
                var row = licenseeProfile.Tables[0].Rows[0];

                criteria.OddsType = row[OddsTypeCollumn].ToString();
                criteria.LanguageList = row["LanguageList"].ToString();
                criteria.SportIDList = row["SportIDList"].ToString();
                criteria.CountryCodeList = row["CountryCodeList"].ToString();
                criteria.LeagueIDList = row["LeagueIDList"].ToString();
                criteria.OutRightIDList = row["OutRightIDList"].ToString();
                criteria.BetTypeIDList = row["BetTypeIDList"].ToString();
                criteria.LeagueKeyword = row["LeagueKeyword"].ToString();
                criteria.LeagueKeyword_O = row["LeagueKeyword_O"].ToString();

                ViewBag.NumberOfIPs = int.Parse(row[NumberOfIpsCollumn].ToString());

                ViewBagForFlags(true, true, true, true, true, true);

                ProfileForAddAffiliateWithAccountManagerRole(ref criteria);
            }
        }

        private void ProfileForAddAffiliateWithAccountManagerRole(ref InsertAffiliateCriteria criteria)
        {
            ViewBag.OddsTypeList = baseService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = OddsTypeSettingType }, false)
                .Where(item => item.Text != "Live" || (item.Text == "Live" && IsAccountManagerRole()));
            ViewBag.OddsStyleList = baseService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = OddsRegionSettingType }, false);
            ViewBag.LanguageIDList = baseService.GetSelectList(new GetAllLanguageCriteria());
            ViewBag.DelayTimeList = GenerateDelayTime(baseService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "Config" }, false));
            ViewBag.ResultDelayTimeList = licenseeService.GetResultDelayTimeList();

            // Sport
            var sports = baseService.GetSelectListWithGroup(new GetSupportedSportCriteria());
            var sportList = UpdateSportListForCricket(sports);
            if (criteria.SportIDList.Equals("all"))
            {
                criteria.SportIDList = string.Join(",", sports.Select(item => item.Value));
            }

            ViewBag.MajorSportList = sportList.Where(sport => sport.Group.Name == MajorGroup);
            ViewBag.MinorSportList = sportList.Where(sport => sport.Group.Name == MinorGroup);

            // Market
            var bettypes = baseService.GetSelectListWithGroup(new GetSupportedBettypeCriteria());

            if (criteria.BetTypeIDList.Equals("all"))
            {
                criteria.BetTypeIDList = string.Join(",", bettypes.Select(item => item.Value));
            }

            ViewBag.MajorBettypesList = bettypes.Where(betType => betType.Group.Name == MajorGroup);
            ViewBag.MinorBettypesList = bettypes.Where(betType => betType.Group.Name == MinorGroup);

            // Country
            var countries = baseService.GetSelectList(new GetAllCountryCriteria());
            ViewBag.CountriesList = countries;
            if (criteria.CountryCodeList.Equals("all"))
            {
                criteria.CountryCodeList = string.Join(",", countries.Select(item => item.Value));
            }

            // League
            var leagues = baseService.GetSelectList(new GetLeagueCriteria
            {
                SportIDList = criteria.SportIDList,
                CountryCodeList = criteria.CountryCodeList
            });
            ViewBag.LeagueList = leagues;
            if (criteria.LeagueIDList.Equals("all"))
            {
                criteria.LeagueIDList = string.Join(",", leagues.Select(item => item.Value));
            }

            // OutRight
            var outrights = baseService.GetSelectList(new GetOutRightCriteria
            {
                SportIDList = criteria.SportIDList,
                CountryCodeList = criteria.CountryCodeList
            });
            ViewBag.OutrightList = outrights;
            if (criteria.OutRightIDList.Equals("all"))
            {
                criteria.OutRightIDList = string.Join(",", outrights.Select(item => item.Value));
            }
        }

        private void GetAffiliateProfileWithAccountManageRole(DataSet affiliateProfile, ref UpdateProfileCriteria criteria)
        {
            if (affiliateProfile != null && affiliateProfile.Tables.Count > 0 && affiliateProfile.Tables[0].Rows.Count > 0)
            {
                var row = affiliateProfile.Tables[0].Rows[0];
                // Delay Times
                var delayTimes = row["OddsDelayTimes"].ToString().Split(',');
                criteria.DeadballDelayTimes = delayTimes.Length > 0 ? delayTimes[0] : "0";
                criteria.LiveDelayTimes = delayTimes.Length > 1 ? delayTimes[1] : "0";
                criteria.OutrightDelayTimes = delayTimes.Length > OutrightDelayTimesIndex ? delayTimes[OutrightDelayTimesIndex] : "0";
                criteria.ResultDelayTimes = row["ResultDelayTimes"].ToString();

                criteria.NumberOfIPs = !(row[NumberOfIpsCollumn] is DBNull) ? (short)row[NumberOfIpsCollumn] : (short)0;
                criteria.LeagueKeyword = row["LeagueKeyword"].ToString();
                criteria.LeagueKeyword_O = row["LeagueKeyword_O"].ToString();
                criteria.LeagueIDList = row["LeagueIDList"].ToString();
                criteria.OutRightIDList = row["OutrightIDList"].ToString();
                ViewBag.ActualNumberOfIps = int.Parse(row["ActualNumberOfIps"].ToString());
            }

            var licenseeProfile = baseService.GetDataSet(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = currentSession.SystemId });
            if (licenseeProfile != null && licenseeProfile.Tables.Count > 0 && licenseeProfile.Tables[0].Rows.Count > 0)
            {
                var row = licenseeProfile.Tables[0].Rows[0];
                ViewBag.NumberOfIPs = int.Parse(row[NumberOfIpsCollumn].ToString());
                ViewBagForFlags(true, true, true, true, true, true);

                ProfileForEditAffiliateWithAccountManagerRole(ref criteria);
            }
        }

        private void ProfileForEditAffiliateWithAccountManagerRole(ref UpdateProfileCriteria criteria)
        {
            ViewBag.OddsTypeList = baseService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = OddsTypeSettingType }, false);
            ViewBag.OddsStyleList = baseService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = OddsRegionSettingType }, false);
            ViewBag.LanguageIDList = baseService.GetSelectList(new GetAllLanguageCriteria());
            var config = baseService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "Config" }, false);
            ViewBag.DelayTimeList = GenerateDelayTime(config);
            ViewBag.ResultDelayTimeList = licenseeService.GetResultDelayTimeList();
            // Sport
            var sports = baseService.GetSelectListWithGroup(new GetSupportedSportCriteria());
            var sportList = UpdateSportListForCricket(sports);
            if (criteria.SportIDList.Equals("all"))
            {
                criteria.SportIDList = string.Join(",", sports.Select(item => item.Value));
            }

            ViewBag.MajorSportList = sportList.Where(sport => sport.Group.Name == MajorGroup);
            ViewBag.MinorSportList = sportList.Where(sport => sport.Group.Name == MinorGroup);

            // Market
            var bettypes = baseService.GetSelectListWithGroup(new GetSupportedBettypeCriteria());

            if (criteria.BetTypeIDList.Equals("all"))
            {
                criteria.BetTypeIDList = string.Join(",", bettypes.Select(item => item.Value));
            }

            ViewBag.MajorBettypesList = bettypes.Where(betType => betType.Group.Name == MajorGroup);
            ViewBag.MinorBettypesList = bettypes.Where(betType => betType.Group.Name == MinorGroup);

            // Country
            var countries = affiliateService.GetSelectList(new GetAllCountryCriteria());
            ViewBag.CountriesList = countries;
            if (criteria.CountryCodeList.Equals("all"))
            {
                criteria.CountryCodeList = string.Join(",", countries.Select(item => item.Value));
            }

            // League
            var leagues = baseService.GetSelectList(new GetLeagueCriteria
            {
                SportIDList = criteria.SportIDList,
                CountryCodeList = criteria.CountryCodeList
            });
            ViewBag.LeagueList = leagues;
            if (criteria.LeagueIDList.Equals("all"))
            {
                criteria.LeagueIDList = string.Join(",", leagues.Select(item => item.Value));
            }

            // OutRight
            var outrights = baseService.GetSelectList(new GetOutRightCriteria
            {
                SportIDList = criteria.SportIDList,
                CountryCodeList = criteria.CountryCodeList
            });
            ViewBag.OutrightList = outrights;
            if (criteria.OutRightIDList.Equals("all"))
            {
                criteria.OutRightIDList = string.Join(",", outrights.Select(item => item.Value));
            }
        }

        #endregion Account Manager

        #region LicenseeAdmin/Sub-Acc

        private void AddAffiliateWithLicenseeAdminRole(DataSet licenseeProfile)
        {
            if (licenseeProfile != null && licenseeProfile.Tables.Count > 0 && licenseeProfile.Tables[0].Rows.Count > 0)
            {
                var row = licenseeProfile.Tables[0].Rows[0];
                var oddStyles = row[OddsTypeCollumn].ToString();
                ViewBag.NumberOfIPs = int.Parse(row[NumberOfIpsCollumn].ToString());
                ViewBagForFlags(
                    bool.Parse(row[OddsTypeFlagCollumn].ToString()),
                    bool.Parse(row[OddsRegionFlagCollumn].ToString()),
                    bool.Parse(row[LanguageListFlagCollumn].ToString()),
                    bool.Parse(row[SportIDListFlagCollumn].ToString()),
                    bool.Parse(row[CountryCodeListFlagCollumn].ToString()),
                    bool.Parse(row[BettypeIDListFlagCollumn].ToString()) && (oddStyles != "8" && oddStyles != string.Empty));

                ProfileForAddAffiliateWithLicenseeAdminAndSubAccountRole();
            }
        }

        private void GetAffiliateProfileWithLicenseeAdminRole(ref UpdateProfileCriteria criteria)
        {
            var licenseeProfile = baseService.GetDataSet(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = currentSession.SystemId });
            if (licenseeProfile != null && licenseeProfile.Tables.Count > 0 && licenseeProfile.Tables[0].Rows.Count > 0)
            {
                var row = licenseeProfile.Tables[0].Rows[0];
                var oddStyles = row[OddsTypeCollumn].ToString();
                ViewBag.NumberOfIPs = int.Parse(row[NumberOfIpsCollumn].ToString());
                ViewBag.OddsTypeFlag = bool.Parse(row[OddsTypeFlagCollumn].ToString());
                ViewBag.OddsStyleFlag = bool.Parse(row[OddsRegionFlagCollumn].ToString());
                ViewBag.LanguagesFlag = bool.Parse(row[LanguageListFlagCollumn].ToString());
                ViewBag.SportsFlag = bool.Parse(row[SportIDListFlagCollumn].ToString());
                ViewBag.CountriesFlag = bool.Parse(row[CountryCodeListFlagCollumn].ToString());
                ViewBag.MarketsFlag = bool.Parse(row[BettypeIDListFlagCollumn].ToString()) && (oddStyles != "8" && oddStyles != string.Empty);

                ProfileForEditAffiliateWithLicenseeAdminAndSubAccountRole(ref criteria);
            }
        }

        private void AddAffiliateWithSubAccountRole()
        {
            var userProfile = baseService.GetDataSet(new UserGetPermissionCriteria
            {
                SystemID = currentSession.SystemId,
                UserID = currentSession.UserId
            });

            if (userProfile != null && userProfile.Tables.Count > 0 && userProfile.Tables[0].Rows.Count > 0)
            {
                var row = userProfile.Tables[0].Rows[0];
                ViewBag.NumberOfIPs = int.Parse(row[NumberOfIpsCollumn].ToString());
                ViewBagForFlags(
                    bool.Parse(row[OddsTypeFlagCollumn].ToString()),
                    bool.Parse(row[OddsRegionFlagCollumn].ToString()),
                    bool.Parse(row[LanguageListFlagCollumn].ToString()),
                    bool.Parse(row[SportIDListFlagCollumn].ToString()),
                    bool.Parse(row[CountryCodeListFlagCollumn].ToString()),
                    bool.Parse(row[BettypeIDListFlagCollumn].ToString()));

                ProfileForAddAffiliateWithLicenseeAdminAndSubAccountRole();
            }
        }

        private void GetAffiliateProfileWithSubAccountRole(ref UpdateProfileCriteria criteria)
        {
            var userProfile = baseService.GetDataSet(new UserGetPermissionCriteria
            {
                SystemID = currentSession.SystemId,
                UserID = currentSession.UserId
            });

            if (userProfile != null && userProfile.Tables.Count > 0 && userProfile.Tables[0].Rows.Count > 0)
            {
                var row = userProfile.Tables[0].Rows[0];
                ViewBag.NumberOfIPs = int.Parse(row[NumberOfIpsCollumn].ToString());
                ViewBag.OddsTypeFlag = bool.Parse(row[OddsTypeFlagCollumn].ToString());
                ViewBag.OddsStyleFlag = bool.Parse(row[OddsRegionFlagCollumn].ToString());
                ViewBag.LanguagesFlag = bool.Parse(row[LanguageListFlagCollumn].ToString());
                ViewBag.SportsFlag = bool.Parse(row[SportIDListFlagCollumn].ToString());
                ViewBag.CountriesFlag = bool.Parse(row[CountryCodeListFlagCollumn].ToString());
                ViewBag.MarketsFlag = bool.Parse(row[BettypeIDListFlagCollumn].ToString());

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
            var sportList = UpdateSportListForCricket(affiliateService.GetSportBySystemId());
            ViewBag.MajorSportList = sportList.Where(sport => sport.Group.Name == MajorGroup);
            ViewBag.MinorSportList = sportList.Where(sport => sport.Group.Name == MinorGroup);
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
            var sports = UpdateSportListForCricket(affiliateService.GetSportBySystemId());
            if (criteria.SportIDList.Equals("all"))
            {
                criteria.SportIDList = string.Join(",", sports.Select(item => item.Value));
            }
            ViewBag.MajorSportList = sports.Where(sport => sport.Group.Name == MajorGroup);
            ViewBag.MinorSportList = sports.Where(sport => sport.Group.Name == MinorGroup);

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

        private static IEnumerable<SelectListItem> UpdateSportListForCricket(IEnumerable<SelectListItem> sports)
        {
            return sports.Where(s => s.Value != "50");
        }

        #endregion LicenseeAdmin/Sub-Acc

        #endregion Common
    }
}