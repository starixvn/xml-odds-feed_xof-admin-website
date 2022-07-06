namespace Fanex.OddsFeed.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Criteria.Licensee;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Service.Impl;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Core.Web.Attribute;

    [IsAuthorize]
    public class LicenseeController : BaseController
    {
        private const string MajorGroup = "Major";
        private const string MinorGroup = "Minor";

        private readonly ILicenseeService _licenseeService;
        private readonly IAffiliateService _affiliateService;

        public LicenseeController()
            : this(null, null)
        {
        }

        public LicenseeController(ILicenseeService licenseeService, IAffiliateService affiliateService)
        {
            _licenseeService = licenseeService ?? new LicenseeService();
            _affiliateService = affiliateService ?? new AffiliateService();
        }

        #region AddNew

        [HttpGet]
        public ActionResult Add()
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            // OddsType
            ViewBag.OddsTypeList = baseService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "OddsType" }, false);

            // OddsStyle
            ViewBag.OddsStyleList = baseService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "OddsRegion" }, false);

            // Config
            var config = baseService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "Config" }, false);
            ViewBag.DelayTimeList = GenerateDelayTime(config);
            ViewBag.ResultDelayTimeList = _licenseeService.GetResultDelayTimeList();
            ViewBag.NumberOfIPs = GetMaxNumberOfIPsFromConfig(config);

            // Domain
            ViewBag.DomainList = baseService.GetSelectList(new GetAllDomainCriteria
            {
                SystemID = 0,
                IsActive = null,
                IsOddsDomain = null
            });

            // Market
            var bettypes = baseService.GetSelectListWithGroup(new GetSupportedBettypeCriteria());

            ViewBag.MajorBettypesList = bettypes.Where(betType => betType.Group.Name == MajorGroup);
            ViewBag.MinorBettypesList = bettypes.Where(betType => betType.Group.Name == MinorGroup);

            // Country
            ViewBag.CountriesList = baseService.GetSelectList(new GetAllCountryCriteria());

            // Sport
            var sports = baseService.GetSelectListWithGroup(new GetSupportedSportCriteria());
            var sportList = UpdateSportListForCricket(sports);

            ViewBag.MajorSportList = sportList.Where(sport => sport.Group.Name == MajorGroup);
            ViewBag.MinorSportList = sportList.Where(sport => sport.Group.Name == MinorGroup);

            // Language
            ViewBag.LanguageIDList = baseService.GetSelectList(new GetAllLanguageCriteria());

            // Licensees
            ViewBag.LicenseesList = baseService.GetSelectList("Odds_LicenseeSel");

            var criteria = new CreateLicenseeCriteria();
            return View(criteria);
        }

        public int CreateLicensee(CreateLicenseeCriteria criteria)
        {
            if (!criteria.AddExistingLicensee)
            {
                criteria.LicenseeID = 0;
            }

            criteria.SystemName = criteria.SystemName.Trim();
            criteria.ContactEmail = criteria.ContactEmail.Trim();
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            criteria.BetTypeIDList = criteria.BetTypeIDList ?? string.Empty;
            criteria.LeagueKeyword = criteria.LeagueKeyword ?? string.Empty;
            criteria.LeagueKeyword_O = criteria.LeagueKeyword_O ?? string.Empty;

            criteria.SportIDList = UpdateSportIDListForCricket(criteria.SportIDList);
            var res = baseService.ExecuteNonQuery(criteria);
            return (int)res.Code;
        }

        #endregion AddNew

        #region Edit

        public int EditLicensee(EditLicenseeCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            var res = baseService.ExecuteNonQuery(criteria);
            return (int)res.Code;
        }

        public int EditActive(UpdateLicenseeActiveCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            var res = baseService.ExecuteNonQuery(criteria);
            return (int)res.Code;
        }

        [HttpGet]
        public ActionResult EditLicenseeInfo(EditLicenseeInfoCriteria criteria)
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            ViewBag.CurrentSystem = " of \"" + criteria.SystemName + "\"";
            return View(criteria);
        }

        [HttpPost]
        public int EditLicenseeInfoPost(EditLicenseeInfoCriteria criteria)
        {
            if (!criteria.AddExistingLicensee)
            {
                criteria.LicenseeID = 0;
            }

            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            var res = baseService.ExecuteNonQuery(criteria);
            return (int)res.Code;
        }

        [HttpGet]
        public ActionResult EditSetting(EditLicenseeSettingCriteria criteria)
        {
            if (criteria.SystemID == 0)
            {
                return RedirectToAction("ListLicensee", "Licensee");
            }

            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            var licensee = baseService.GetDataSet(new GetLicenseeSettingByLicenseeIDCriteria { SystemID = criteria.SystemID });
            var listIPFrom = new StringBuilder();
            var listIPTo = new StringBuilder();
            foreach (DataRow row in licensee.Tables[0].Rows)
            {
                listIPFrom.Append(row["IPFrom"] + ",");
                listIPTo.Append(row["IPTo"] + ",");
            }

            criteria.ListIPFrom = listIPFrom.ToString().Trim(',');
            criteria.ListIPTo = listIPTo.ToString().Trim(',');

            ////Domain
            var selectedDomains = baseService.GetSelectList(new GetDomainBySystemIDCriteria
            {
                SystemID = (byte)criteria.SystemID
            });

            ViewBag.SelectedDomainList = selectedDomains;
            ViewBag.NumberOfIPs = GetMaxNumberOfIPs();
            ViewBag.DomainList = baseService.GetSelectList(new GetAllDomainCriteria
            {
                SystemID = 0,
                IsActive = null,
                IsOddsDomain = null
            }).Where(d => !selectedDomains.Any(s => s.Value == d.Value));

            ViewBag.CurrentSystem = " of \"" + criteria.SystemName + "\"";

            return View(criteria);
        }

        [HttpPost]
        public int EditSettingPost(EditLicenseeSettingCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            var res = baseService.ExecuteNonQuery(criteria);
            return (int)res.Code;
        }

        [HttpGet]
        public ActionResult EditProfile(EditLicenseeProfileCriteria criteria)
        {
            if (criteria.SystemID == 0)
            {
                return RedirectToAction("ListLicensee", "Licensee");
            }

            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            GetLicenseeProfile(ref criteria);
            ViewBagForEditLicenseeProfile(ref criteria);

            ViewBag.LicenseesList = baseService.GetSelectList("Odds_LicenseeSel");
            ViewBag.CurrentSystem = " of \"" + criteria.SystemName + "\"";

            return View(criteria);
        }

        [HttpPost]
        public JsonResult EditProfilePost(EditLicenseeProfileCriteria criteria)
        {
            criteria.BetTypeIDList = criteria.BetTypeIDList ?? string.Empty;
            criteria.SportIDList = UpdateSportIDListForCricket(criteria.SportIDList);

            if (criteria.IsForceUpdate)
            {
                return SaveLicenseeProfile(criteria);
            }
            else
            {
                var missingPartnersProfile = GetMissingProfile(criteria);
                if (IsMissingProfile(missingPartnersProfile))
                {
                    // Assign a error code in case some affiliates under this licensee have the number of selections greater than the current licensee's profile
                    CustomizeMissProfileData(criteria, ref missingPartnersProfile);
                    return Json(missingPartnersProfile, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return SaveLicenseeProfile(criteria);
                }
            }
        }

        [HttpPost]
        public JsonResult UpdateIPRange_EditSetting(UpdateSettingIP_LicenseeCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.IpFromOld = string.IsNullOrEmpty(criteria.IpFromOld) ? string.Empty : criteria.IpFromOld;
            criteria.IpToOld = string.IsNullOrEmpty(criteria.IpToOld) ? string.Empty : criteria.IpToOld;

            string resultMessage;
            var result = baseService.ExecuteNonQuery(criteria);
            if (result != null)
            {
                ViewBag.Success = true;
                resultMessage = result.IsSuccessful
                    ? ViewResources.SuccessfulMessage
                    : result.Message;
            }
            else
            {
                resultMessage = ViewResources.InternalErrorMessage;
            }

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        #endregion Edit

        #region Get

        public ActionResult ListLicensee()
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            return View();
        }

        public ActionResult GetLeague(GetLeagueCriteria criteria)
        {
            criteria.SportIDList = UpdateSportIDListForCricket(criteria.SportIDList);
            ViewBag.LeagueList = baseService.GetSelectList(criteria);

            return PartialView("_LeaguePartialView", criteria);
        }

        public ActionResult GetOutright(GetOutRightCriteria criteria)
        {
            criteria.SportIDList = UpdateSportIDListForCricket(criteria.SportIDList);
            ViewBag.OutrightList = baseService.GetSelectList(criteria);

            return PartialView("_OutRightPartialView", criteria);
        }

        public JsonResult GetLicenseeByName(GetLicenseeByPageCriteria criteria)
        {
            criteria.PageIndex++;
            if (criteria.SystemName == null || criteria.SystemName == "Licensee Name")
            {
                criteria.SystemName = string.Empty;
            }

            var dr = baseService.GetData(criteria);
            var result = FanexGridHelper.DataForFanexGrid(dr);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Requests()
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            var model = new LicenseeRequestCriteria();
            return View("LicenseeRequest", model);
        }

        public string GetAllLicenseeRequest(LicenseeRequestCriteria criteria)
        {
            criteria.PageIndex++;
            if (criteria.SystemName == null || criteria.SystemName == "Licensee name")
            {
                criteria.SystemName = string.Empty;
            }

            var dr = baseService.GetData(criteria);
            return FanexGridHelper.DataForFanexGrid(dr);
        }

        public string GetLicenseeRequestsDetail(RequestStatisticCriteria criteria, string date, int year)
        {
            const int numberDaysInWeek = 7;

            if (!string.IsNullOrEmpty(date))
            {
                DateTime dateStart;
                DateTime dateEnd;
                switch (criteria.PeriodOption.ToString())
                {
                    case "Week":
                        var dt = new DateTime(year, 1, 1).AddDays(((int.Parse(date) - 1) * 7) - (int.Parse(date) == 1 ? 0 : 1));
                        dt = dt.AddDays(-(int)dt.DayOfWeek);

                        dateStart = dt.AddDays(1);
                        dateEnd = dt.AddDays(numberDaysInWeek);
                        break;

                    case "Year":
                        dateStart = DateTime.Parse(date);
                        dateStart = new DateTime(dateStart.Year, dateStart.Month, 1);
                        dateEnd = DateTime.Parse(date).AddMonths(1).AddDays(-1); ////get last day of month
                        break;

                    default:
                        dateStart = DateTime.Parse(date);
                        dateEnd = DateTime.Parse(date);
                        break;
                }

                criteria.DateStart = dateStart;
                criteria.DateEnd = dateEnd;
            }

            criteria.PageIndex++;
            var dr = _affiliateService.GetPartnerRequestStatisticReturnDataCollection(criteria);
            return FanexGridHelper.DataForFanexGrid(dr);
        }

        public JsonResult ListExistedSystems(string systemName)
        {
            var criteria = new GetTop10LicenseeCriteria
            {
                Systemname = systemName
            };

            var top10SystemList = baseService.GetDataSet(criteria).Tables[0];
            return JsonForAutoComplete(top10SystemList, "SystemID", "SystemName");
        }

        public string LicenseeList_GetTop10(string systemName)
        {
            var criteria = new GetTop10LicenseeCriteria
            {
                Systemname = systemName
            };

            var top10SystemList = baseService.GetDataSet(criteria).Tables[0];
            return DataForAutoComplete(top10SystemList, "SystemName");
        }

        public bool CheckExistLicenseeName(CheckExistLicenseeCriteria criteria)
        {
            const int DefaultPageSize = 10;

            criteria.PageIndex = 1;
            criteria.PageSize = DefaultPageSize;
            var res = baseService.GetDataSet(criteria);
            return res.Tables[0].Rows.Count == 0;
        }

        #endregion Get

        #region Delete

        public int DeleteLicensee(DeleteLicenseeCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            var res = baseService.ExecuteNonQuery(criteria);
            return (int)res.Code;
        }

        [HttpPost]
        public JsonResult DeleteIPRange_EditSetting(DeleteSettingIP_LicenseeCriteria criteria)
        {
            var deleteResult = new OddsFeedActionResult();
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            criteria.IPFromOld = string.IsNullOrEmpty(criteria.IPFromOld) ? string.Empty : criteria.IPFromOld;
            criteria.IPToOld = string.IsNullOrEmpty(criteria.IPToOld) ? string.Empty : criteria.IPToOld;

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

        #endregion Delete

        #region Common

        private MissingPartnersProfile GetMissingProfile(EditLicenseeProfileCriteria criteria)
        {
            var missingProfile = new MissingPartnersProfile();

            ////get profile of all affs
            var partnerProfile = _licenseeService.GetProfileAllPartnersBySystemId(criteria.SystemID);

            missingProfile.AffsOddsType = Utilities.ProducesDifferenceTwoString(partnerProfile.AffsOddsType, criteria.OddsType);
            missingProfile.AffsLanguageList = Utilities.ProducesDifferenceTwoString(partnerProfile.AffsLanguageList, criteria.LanguageList);
            missingProfile.AffsSportIDList = criteria.SportIDList == "all"
                ? string.Empty : Utilities.ProducesDifferenceTwoString(partnerProfile.AffsSportIDList, criteria.SportIDList);
            missingProfile.AffsCountryCodeList = criteria.CountryCodeList == "all"
                ? string.Empty : Utilities.ProducesDifferenceTwoString(partnerProfile.AffsCountryCodeList, criteria.CountryCodeList);
            missingProfile.AffsLeagueIDList = criteria.LeagueIDList == "all"
                ? string.Empty : Utilities.ProducesDifferenceTwoString(partnerProfile.AffsLeagueIDList, criteria.LeagueIDList);
            missingProfile.AffsOutrightIDList = criteria.OutRightIDList == "all"
                ? string.Empty : Utilities.ProducesDifferenceTwoString(partnerProfile.AffsOutrightIDList, criteria.OutRightIDList);
            missingProfile.AffsBettypeIDList = criteria.BetTypeIDList == "all"
                ? string.Empty : Utilities.ProducesDifferenceTwoString(partnerProfile.AffsBettypeIDList, criteria.BetTypeIDList);

            return missingProfile;
        }

        private static bool IsMissingProfile(MissingPartnersProfile missingPartnersProfile)
        {
            StringBuilder missingSummary = new StringBuilder();
            missingSummary.Append(missingPartnersProfile.AffsOddsType);
            missingSummary.Append(missingPartnersProfile.AffsLanguageList);
            missingSummary.Append(missingPartnersProfile.AffsSportIDList);
            missingSummary.Append(missingPartnersProfile.AffsCountryCodeList);
            missingSummary.Append(missingPartnersProfile.AffsLeagueIDList);
            missingSummary.Append(missingPartnersProfile.AffsOutrightIDList);
            missingSummary.Append(missingPartnersProfile.AffsBettypeIDList);

            return !string.IsNullOrEmpty(missingSummary.ToString());
        }

        private static void CustomizeMissProfileData(EditLicenseeProfileCriteria criteria, ref MissingPartnersProfile missingPartnersProfile)
        {
            const int defaultErrCode = 10701;

            missingPartnersProfile.ErrCode = defaultErrCode;

            missingPartnersProfile.AffsOddsType = string.IsNullOrEmpty(missingPartnersProfile.AffsOddsType)
                ? criteria.OddsType : missingPartnersProfile.AffsOddsType + "," + criteria.OddsType;
            missingPartnersProfile.AffsLanguageList = string.IsNullOrEmpty(missingPartnersProfile.AffsLanguageList)
                ? criteria.LanguageList : missingPartnersProfile.AffsLanguageList + "," + criteria.LanguageList;
            missingPartnersProfile.AffsSportIDList = string.IsNullOrEmpty(missingPartnersProfile.AffsSportIDList)
                ? criteria.SportIDList : missingPartnersProfile.AffsSportIDList + "," + criteria.SportIDList;
            missingPartnersProfile.AffsCountryCodeList = string.IsNullOrEmpty(missingPartnersProfile.AffsCountryCodeList)
                ? criteria.CountryCodeList : missingPartnersProfile.AffsCountryCodeList + "," + criteria.CountryCodeList;
            missingPartnersProfile.AffsLeagueIDList = string.IsNullOrEmpty(missingPartnersProfile.AffsLeagueIDList)
                ? criteria.LeagueIDList : missingPartnersProfile.AffsLeagueIDList + "," + criteria.LeagueIDList;
            missingPartnersProfile.AffsOutrightIDList = string.IsNullOrEmpty(missingPartnersProfile.AffsOutrightIDList)
                ? criteria.OutRightIDList : missingPartnersProfile.AffsOutrightIDList + "," + criteria.OutRightIDList;
            missingPartnersProfile.AffsBettypeIDList = string.IsNullOrEmpty(missingPartnersProfile.AffsBettypeIDList)
                ? criteria.BetTypeIDList : missingPartnersProfile.AffsBettypeIDList + "," + criteria.BetTypeIDList;

            MinimizeMissProfileData(ref missingPartnersProfile);
        }

        private static void MinimizeMissProfileData(ref MissingPartnersProfile missingProfile)
        {
            if (missingProfile.AffsSportIDList.Contains("all"))
            {
                missingProfile.AffsSportIDList = "all";
            }

            if (missingProfile.AffsCountryCodeList.Contains("all"))
            {
                missingProfile.AffsCountryCodeList = "all";
            }

            if (missingProfile.AffsLeagueIDList.Contains("all"))
            {
                missingProfile.AffsLeagueIDList = "all";
            }

            if (missingProfile.AffsOutrightIDList.Contains("all"))
            {
                missingProfile.AffsOutrightIDList = "all";
            }

            if (missingProfile.AffsBettypeIDList.Contains("all"))
            {
                missingProfile.AffsBettypeIDList = "all";
            }
        }

        private JsonResult SaveLicenseeProfile(EditLicenseeProfileCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            criteria.BettypeIDListFlag = criteria.BettypeIDListFlag && criteria.BetTypeIDList != null;
            var res = baseService.ExecuteNonQuery(criteria);

            var missingPartnersProfile = new MissingPartnersProfile
            {
                ErrCode = (int)res.Code
            };
            return Json(missingPartnersProfile, JsonRequestBehavior.AllowGet);
        }

        private void GetLicenseeProfile(ref EditLicenseeProfileCriteria criteria)
        {
            var licensee = baseService.GetDataSet(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = criteria.SystemID });
            if (licensee != null && licensee.Tables.Count > 0 && licensee.Tables[0].Rows.Count > 0)
            {
                var row = licensee.Tables[0].Rows[0];
                criteria.NumberOfIPs = short.Parse(row["NumberOfIPs"].ToString());
                criteria.BetTypeIDList = row["BetTypeIDList"].ToString();
                criteria.BettypeIDListFlag = bool.Parse(row["BettypeIDListFlag"].ToString());
                criteria.CountryCodeList = row["CountryCodeList"].ToString();
                criteria.CountryCodeListFlag = bool.Parse(row["CountryCodeListFlag"].ToString());
                var delayTimes = row["OddsDelayTimes"].ToString().Split(',');
                if (delayTimes.Length >= 1)
                {
                    criteria.DeadballDelayTimes = delayTimes[0];
                }

                if (delayTimes.Length >= 2)
                {
                    criteria.LiveDelayTimes = delayTimes[1];
                }

                if (delayTimes.Length >= 3)
                {
                    criteria.OutrightDelayTimes = delayTimes[2];
                }

                criteria.OddsDelayTimesFlag = bool.Parse(row["OddsDelayTimesFlag"].ToString());

                criteria.ResultDelayTimes = row["ResultDelayTimes"].ToString();
                criteria.ResultDelayTimesFlag = bool.Parse(row["ResultDelayTimesFlag"].ToString());

                criteria.LanguageList = row["LanguageList"].ToString();
                criteria.LanguageListFlag = bool.Parse(row["LanguageListFlag"].ToString());
                criteria.LeagueIDList = row["LeagueIDList"].ToString();
                criteria.LeagueIDListFlag = bool.Parse(row["LeagueIDListFlag"].ToString());
                criteria.LeagueKeyword = row["LeagueKeyword"].ToString();
                criteria.LeagueKeyword_O = row["LeagueKeyword_O"].ToString();
                criteria.LeagueKeyword_OFlag = bool.Parse(row["LeagueKeyword_OFlag"].ToString());
                criteria.LeagueKeywordFlag = bool.Parse(row["LeagueKeywordFlag"].ToString());
                criteria.OddsRegion = row["OddsRegion"].ToString();
                criteria.OddsRegionFlag = bool.Parse(row["OddsRegionFlag"].ToString());
                criteria.OddsType = row["OddsType"].ToString();
                criteria.OddsTypeFlag = bool.Parse(row["OddsTypeFlag"].ToString());
                criteria.OutRightIDList = row["OutRightIDList"].ToString();
                criteria.SportIDList = row["SportIDList"].ToString();
                criteria.SportIDListFlag = bool.Parse(row["SportIDListFlag"].ToString());
            }
        }

        private void ViewBagForEditLicenseeProfile(ref EditLicenseeProfileCriteria criteria)
        {
            ViewBag.OddsTypeList = baseService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "OddsType" }, false);
            ViewBag.OddsStyleList = baseService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "OddsRegion" }, false);
            ViewBag.LanguageIDList = baseService.GetSelectList(new GetAllLanguageCriteria());
            var config = baseService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "Config" }, false);
            ViewBag.DelayTimeList = GenerateDelayTime(config);
            ViewBag.ResultDelayTimeList = _licenseeService.GetResultDelayTimeList();

            ////Sport
            var sports = baseService.GetSelectListWithGroup(new GetSupportedSportCriteria());
            var sportList = UpdateSportListForCricket(sports);
            if (criteria.SportIDList.Equals("all"))
            {
                criteria.SportIDList = string.Join(",", sports.Select(item => item.Value));
            }

            ViewBag.MajorSportList = sportList.Where(sport => sport.Group.Name == "Major");
            ViewBag.MinorSportList = sportList.Where(sport => sport.Group.Name == "Minor");

            ////Market
            var bettypes = baseService.GetSelectListWithGroup(new GetSupportedBettypeCriteria());

            if (criteria.BetTypeIDList.Equals("all"))
            {
                criteria.BetTypeIDList = string.Join(",", bettypes.Select(item => item.Value));
            }

            ViewBag.MajorBettypesList = bettypes.Where(betType => betType.Group.Name == "Major");
            ViewBag.MinorBettypesList = bettypes.Where(betType => betType.Group.Name == "Minor");

            ////Country
            var countries = baseService.GetSelectList(new GetAllCountryCriteria());
            ViewBag.CountriesList = countries;
            if (criteria.CountryCodeList.Equals("all"))
            {
                criteria.CountryCodeList = string.Join(",", countries.Select(item => item.Value));
            }
            //// League
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
            ////OutRight
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

        private static IEnumerable<SelectListItem> UpdateSportListForCricket(IEnumerable<SelectListItem> sports)
        {
            return sports.Where(s => s.Value != "50");
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

        #endregion Common

    }
}