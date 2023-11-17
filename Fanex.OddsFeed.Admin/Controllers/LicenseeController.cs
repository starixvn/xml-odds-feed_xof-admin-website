namespace Fanex.OddsFeed.Admin.Controllers
{
    using System.Text;
    using System.Web.WebPages.Html;
    using Core;
    using Core.Criteria.Affiliate;
    using Core.Criteria.Licensee;
    using Core.Model;
    using Core.Model.ValueObject;
    using Core.Service.Impl;
    using Core.Service.Interface;
    using Core.Web.Attribute;
    using Core.Web.Context;
    using Core.Web.Session;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Utility;

    [IsAuthorize]
    public class LicenseeController : BaseController
    {
        private const string MajorGroup = "Major";
        private const string MinorGroup = "Minor";

        private readonly ILicenseeService _licenseeService;

        public LicenseeController(ILicenseeService licenseeService,
            ICurrentContext currentContext,
            ICurrentSession currentSession,
            IBaseService baseService,
            AppSettings appSettings) : base(currentContext, currentSession, baseService, appSettings)
        {
            _licenseeService = licenseeService;
        }

        #region AddNew

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            // OddsType
            var oddsTypes = await baseService.GetDataAsync<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = "OddsType" });
            ViewBag.OddsTypeList = oddsTypes.Select(x => new SelectListItem { Value = x.Value, Text = x.Key }).ToList();

            // OddsStyle
            var oddsStyles = await baseService.GetDataAsync<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = "OddsRegion" });
            ViewBag.OddsStyleList = oddsStyles.Select(x => new SelectListItem { Value = x.Value, Text = x.Key }).ToList();

            // Config
            ViewBag.DelayTimeList = GenerateDelayTime();
            ViewBag.ResultDelayTimeList = _licenseeService.GetResultDelayTimeList();
            ViewBag.NumberOfIPs = GetMaxNumberOfIPs();

            // Domain
            var domains = await baseService.GetDataAsync<Domain>(new GetAllDomainCriteria { SystemID = 0 });
            ViewBag.DomainList = domains.Select(x => new SelectListItem { Value = x.DomainId.ToString(), Text = x.DomainName }).ToList();

            // Market
            var bettypes = await baseService.GetDataAsync<BetType>(new GetSupportedBettypeCriteria());
            ViewBag.MajorBettypesList = bettypes.Where(x => x.Group == MajorGroup)
                .Select(x => new SelectListItem { Value = x.TypeId.ToString(), Text = x.TypeNameE }).ToList();
            ViewBag.MinorBettypesList = bettypes.Where(x => x.Group == MinorGroup)
                .Select(x => new SelectListItem { Value = x.TypeId.ToString(), Text = x.TypeNameE }).ToList();

            // Country
            var countries = await baseService.GetDataAsync<Country>(new GetAllCountryCriteria());
            ViewBag.CountriesList = countries.Select(x => new SelectListItem { Value = x.CountryCode, Text = x.CountryName }).ToList();

            // Sport
            var sports = UpdateSportListForCricket(await baseService.GetDataAsync<Sport>(new GetSupportedSportCriteria()));
            ViewBag.MajorSportList = sports.Where(sport => sport.Group == MajorGroup)
                .Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();
            ViewBag.MinorSportList = sports.Where(sport => sport.Group == MinorGroup)
                .Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();

            // Language
            var languages = await baseService.GetDataAsync<Language>(new GetAllLanguageCriteria());
            ViewBag.LanguageIDList = languages.Select(x => new SelectListItem { Value = x.Code, Text = x.Name_EN }).ToList();

            // Licensees
            var licensees = await baseService.GetDataAsync<Licensee>(new GetLicenseeCriteria());
            ViewBag.LicenseesList = licensees.Select(x => new SelectListItem { Value = x.LicenseeID.ToString(), Text = x.LicenseeName }).ToList();

            var criteria = new CreateLicenseeCriteria();
            return View(criteria);
        }

        public async Task<JsonResult> CreateLicensee([FromBody] CreateLicenseeCriteria criteria)
        {
            criteria.SystemName = criteria.SystemName.Trim();
            criteria.ContactEmail = criteria.ContactEmail.Trim();
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            criteria.BetTypeIDList = criteria.BetTypeIDList ?? string.Empty;
            criteria.LeagueKeyword = criteria.LeagueKeyword ?? string.Empty;
            criteria.LeagueKeyword_O = criteria.LeagueKeyword_O ?? string.Empty;

            criteria.SportIDList = UpdateSportIDListForCricket(criteria.SportIDList);
            var result = await baseService.ExecuteNonQueryAsync(criteria);
            return Json(result.Code);
        }

        #endregion AddNew

        #region Edit

        public async Task<JsonResult> EditLicensee([FromBody] EditLicenseeCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            var result = await baseService.ExecuteNonQueryAsync(criteria);
            return Json(result.Code);
        }

        public async Task<JsonResult> EditActive([FromBody] UpdateLicenseeActiveCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            var result = await baseService.ExecuteNonQueryAsync(criteria);
            return Json(result.Code);
        }

        [HttpGet]
        public ActionResult EditLicenseeInfo([FromBody] EditLicenseeInfoCriteria criteria)
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            ViewBag.CurrentSystem = " of \"" + criteria.SystemName + "\"";
            return View(criteria);
        }

        [HttpPost]
        public async Task<JsonResult> EditLicenseeInfoPost([FromBody] EditLicenseeInfoCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            var result = await baseService.ExecuteNonQueryAsync(criteria);
            return Json(result.Code);
        }

        [HttpGet]
        public async Task<IActionResult> EditSetting([FromBody] EditLicenseeSettingCriteria criteria)
        {
            if (criteria.SystemID == 0)
            {
                return RedirectToAction("ListLicensee", "Licensee");
            }

            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            var licensees =await baseService.GetDataAsync<CompanyIP>(new GetLicenseeSettingByLicenseeIDCriteria { SystemID = criteria.SystemID });
            var listIPFrom = new StringBuilder();
            var listIPTo = new StringBuilder();
            foreach (var record in licensees)
            {
                listIPFrom.AppendFormat("{0},", record.IPFrom);
                listIPTo.AppendFormat("{0},", record.IPTo);
            }

            criteria.ListIPFrom = listIPFrom.ToString().Trim(',');
            criteria.ListIPTo = listIPTo.ToString().Trim(',');

            // Domain
            var selectedDomains = await baseService.GetDataAsync<Domain>(new GetDomainBySystemIDCriteria { SystemID = (byte)criteria.SystemID });
            ViewBag.SelectedDomainList = selectedDomains.Select(x => new SelectListItem { Value = x.DomainId.ToString(), Text = x.DomainName }).ToList();

            var domainList = await baseService.GetDataAsync<Domain>(new GetAllDomainCriteria { SystemID = 0 });
            var domains = domainList.Where(d => !selectedDomains.Any(s => s.DomainId == d.DomainId)).ToList();
            ViewBag.DomainList = domains.Select(x => new SelectListItem { Value = x.DomainId.ToString(), Text = x.DomainName }).ToList();

            ViewBag.NumberOfIPs = GetMaxNumberOfIPs();
            ViewBag.CurrentSystem = " of \"" + criteria.SystemName + "\"";

            return View(criteria);
        }

        [HttpPost]
        public async Task<JsonResult> EditSettingPost([FromBody] EditLicenseeSettingCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            var result = await baseService.ExecuteNonQueryAsync(criteria);
            return Json(result.Code);
        }

        //[HttpGet]
        //public ActionResult EditProfile(EditLicenseeProfileCriteria criteria)
        //{
        //    if (criteria.SystemID == 0)
        //    {
        //        return RedirectToAction("ListLicensee", "Licensee");
        //    }

        //    if (!this.IsAccountManagerRole())
        //    {
        //        return this.NoAuthorization();
        //    }

        //    GetLicenseeProfile(ref criteria);
        //    ViewBagForEditLicenseeProfile(ref criteria);

        //    ViewBag.CurrentSystem = " of \"" + criteria.SystemName + "\"";

        //    return View(criteria);
        //}

        //[HttpPost]
        //public JsonResult EditProfilePost(EditLicenseeProfileCriteria criteria)
        //{
        //    criteria.BetTypeIDList = criteria.BetTypeIDList ?? string.Empty;
        //    criteria.SportIDList = UpdateSportIDListForCricket(criteria.SportIDList);

        //    if (criteria.IsForceUpdate)
        //    {
        //        return SaveLicenseeProfile(criteria);
        //    }
        //    else
        //    {
        //        var missingPartnersProfile = GetMissingProfile(criteria);
        //        if (IsMissingProfile(missingPartnersProfile))
        //        {
        //            // Assign a error code in case some affiliates under this licensee have the number of selections greater than the current licensee's profile
        //            CustomizeMissProfileData(criteria, ref missingPartnersProfile);
        //            return Json(missingPartnersProfile);
        //        }
        //        else
        //        {
        //            return SaveLicenseeProfile(criteria);
        //        }
        //    }
        //}

        [HttpPost]
        public async Task<JsonResult> UpdateIPRange_EditSetting([FromBody] UpdateSettingIP_LicenseeCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.IpFromOld = string.IsNullOrEmpty(criteria.IpFromOld) ? string.Empty : criteria.IpFromOld;
            criteria.IpToOld = string.IsNullOrEmpty(criteria.IpToOld) ? string.Empty : criteria.IpToOld;

            string resultMessage;
            var result =await baseService.ExecuteNonQueryAsync(criteria);
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

            return Json(resultMessage);
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

        public async Task<IActionResult> GetLeague([FromBody] GetLeagueCriteria criteria)
        {
            criteria.SportIDList = UpdateSportIDListForCricket(criteria.SportIDList);
            var leagues = await baseService.GetDataAsync<League>(criteria);
            ViewBag.LeagueList = leagues.Select(x => new SelectListItem { Value = x.LeagueId.ToString(), Text = x.LeagueName }).ToList();

            return PartialView("_LeaguePartialView", criteria);
        }

        public async Task<IActionResult> GetOutright([FromBody] GetOutRightCriteria criteria)
        {
            criteria.SportIDList = UpdateSportIDListForCricket(criteria.SportIDList);
            var outrights = await baseService.GetDataAsync<League>(criteria);
            ViewBag.OutrightList = outrights.Select(x => new SelectListItem { Value = x.LeagueId.ToString(), Text = x.LeagueName }).ToList();

            return PartialView("_OutRightPartialView", criteria);
        }

        public async Task<JsonResult> GetLicenseeByName([FromBody] GetLicenseeByPageCriteria criteria)
        {
            criteria.PageIndex++;
            if (criteria.SystemName == null || criteria.SystemName == "Licensee Name")
            {
                criteria.SystemName = string.Empty;
            }

            var result = await baseService.GetDataAsync<Licensee>(criteria);
            return new JsonResult(new { data = result, totalItems = criteria.RowCount });
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

        public async Task<JsonResult> GetAllLicenseeRequest([FromBody] LicenseeRequestCriteria criteria)
        {
            criteria.PageIndex++;
            if (criteria.SystemName == null || criteria.SystemName == "Licensee name")
            {
                criteria.SystemName = string.Empty;
            }

            var result = await baseService.GetDataAsync<LicenseeRequestStatistic>(criteria);
            return new JsonResult(new { data = result, totalItems = criteria.RowCount });
        }

        public async Task<JsonResult> GetLicenseeRequestsDetail([FromBody] RequestStatisticCriteria criteria, string date, int year)
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
            var result = await baseService.GetDataAsync<LicenseeRequestDetailStatistic>(criteria);
            return new JsonResult(new { data = result, totalItems = criteria.RowCount });
        }

        public async Task<JsonResult> ListExistedSystems(string systemName)
        {
            var criteria = new GetTop10LicenseeCriteria
            {
                Systemname = systemName
            };
            var systems = await baseService.GetDataAsync<Licensee>(criteria);
            return Json(systems.Select(x => new JSONSource { Value = x.SystemID.ToString(), Label = x.SystemName }).ToList());
        }

        public async Task<string> LicenseeList_GetTop10(string systemName)
        {
            var criteria = new GetTop10LicenseeCriteria
            {
                Systemname = systemName
            };
            var systems = await baseService.GetDataAsync<Licensee>(criteria);
            return string.Join(",", systems.Select(x => x.SystemName));
        }

        public async Task<bool> CheckExistLicenseeName([FromBody] CheckExistLicenseeCriteria criteria)
        {
            const int DefaultPageSize = 10;

            criteria.PageIndex = 1;
            criteria.PageSize = DefaultPageSize;
            var licensees = await baseService.GetDataAsync<Licensee>(criteria);
            return !licensees.Any();
        }

        #endregion Get

        #region Delete

        public async Task<JsonResult> DeleteLicensee([FromBody] DeleteLicenseeCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            CommandResult result = await baseService.ExecuteNonQueryAsync(criteria);
            return Json(result.Code);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteIPRange_EditSetting([FromBody] DeleteSettingIP_LicenseeCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            criteria.IPFromOld = string.IsNullOrEmpty(criteria.IPFromOld) ? string.Empty : criteria.IPFromOld;
            criteria.IPToOld = string.IsNullOrEmpty(criteria.IPToOld) ? string.Empty : criteria.IPToOld;

            CommandResult result = await baseService.ExecuteNonQueryAsync(criteria);
            if (result.IsSuccessful)
            {
                result.Message = ViewResources.ActionResult_Success_DeleteIPSetting;
            }

            return Json(result);
        }

        #endregion Delete

        #region Common

        //private MissingPartnersProfile GetMissingProfile(EditLicenseeProfileCriteria criteria)
        //{
        //    var missingProfile = new MissingPartnersProfile();

        //    ////get profile of all affs
        //    var partnerProfile = _licenseeService.GetProfileAllPartnersBySystemId(criteria.SystemID);

        //    missingProfile.AffsOddsType = Utilities.ProducesDifferenceTwoString(partnerProfile.AffsOddsType, criteria.OddsType);
        //    missingProfile.AffsLanguageList = Utilities.ProducesDifferenceTwoString(partnerProfile.AffsLanguageList, criteria.LanguageList);
        //    missingProfile.AffsSportIDList = criteria.SportIDList == "all"
        //        ? string.Empty : Utilities.ProducesDifferenceTwoString(partnerProfile.AffsSportIDList, criteria.SportIDList);
        //    missingProfile.AffsCountryCodeList = criteria.CountryCodeList == "all"
        //        ? string.Empty : Utilities.ProducesDifferenceTwoString(partnerProfile.AffsCountryCodeList, criteria.CountryCodeList);
        //    missingProfile.AffsLeagueIDList = criteria.LeagueIDList == "all"
        //        ? string.Empty : Utilities.ProducesDifferenceTwoString(partnerProfile.AffsLeagueIDList, criteria.LeagueIDList);
        //    missingProfile.AffsOutrightIDList = criteria.OutRightIDList == "all"
        //        ? string.Empty : Utilities.ProducesDifferenceTwoString(partnerProfile.AffsOutrightIDList, criteria.OutRightIDList);
        //    missingProfile.AffsBettypeIDList = criteria.BetTypeIDList == "all"
        //        ? string.Empty : Utilities.ProducesDifferenceTwoString(partnerProfile.AffsBettypeIDList, criteria.BetTypeIDList);

        //    return missingProfile;
        //}

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

        private async Task<JsonResult> SaveLicenseeProfile(EditLicenseeProfileCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            criteria.BettypeIDListFlag = criteria.BettypeIDListFlag && criteria.BetTypeIDList != null;
            CommandResult result = await baseService.ExecuteNonQueryAsync(criteria);

            var missingPartnersProfile = new MissingPartnersProfile
            {
                ErrCode = result.Code
            };
            return Json(missingPartnersProfile);
        }

        //private void GetLicenseeProfile(ref EditLicenseeProfileCriteria criteria)
        //{
        //    var licensee = baseService.GetDataEntity<LicenseeProfile>(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = currentSession.SystemId });
        //    if (licensee != null)
        //    {
        //        criteria.NumberOfIPs = licensee.NumberOfIPs;
        //        criteria.BetTypeIDList = licensee.BetTypeIdList;
        //        criteria.BettypeIDListFlag = licensee.BetTypeIdListFlag;
        //        criteria.CountryCodeList = licensee.CountryCodeList;
        //        criteria.CountryCodeListFlag = licensee.CountryCodeListFlag;
        //        var delayTimes = licensee.OddsDelayTimes.Split(',');
        //        if (delayTimes.Length > 0)
        //        {
        //            criteria.DeadballDelayTimes = delayTimes[0];
        //        }

        //        if (delayTimes.Length > 1)
        //        {
        //            criteria.LiveDelayTimes = delayTimes[1];
        //        }

        //        const byte OutrightDelayTimesIndex = 2;
        //        if (delayTimes.Length > OutrightDelayTimesIndex)
        //        {
        //            criteria.OutrightDelayTimes = delayTimes[OutrightDelayTimesIndex];
        //        }

        //        criteria.OddsDelayTimesFlag = licensee.OddsDelayTimesFlag;

        //        criteria.ResultDelayTimes = licensee.ResultDelayTimes;
        //        criteria.ResultDelayTimesFlag = licensee.ResultDelayTimesFlag;

        //        criteria.LanguageList = licensee.LanguageList;
        //        criteria.LanguageListFlag = licensee.LanguageListFlag;
        //        criteria.LeagueIDList = licensee.LeagueIdList;
        //        criteria.LeagueIDListFlag = licensee.LeagueIdListFlag;
        //        criteria.LeagueKeyword = licensee.LeagueKeyword;
        //        criteria.LeagueKeyword_O = licensee.LeagueKeyword_O;
        //        criteria.LeagueKeyword_OFlag = licensee.LeagueKeyword_OFlag;
        //        criteria.LeagueKeywordFlag = licensee.LeagueKeywordFlag;
        //        criteria.OddsRegion = licensee.OddsRegion;
        //        criteria.OddsRegionFlag = licensee.OddsRegionFlag;
        //        criteria.OddsType = licensee.OddsType;
        //        criteria.OddsTypeFlag = licensee.OddsTypeFlag;
        //        criteria.OutRightIDList = licensee.OutrightIdList;
        //        criteria.SportIDList = licensee.SportIdList;
        //        criteria.SportIDListFlag = licensee.SportIdListFlag;
        //    }
        //}

        //private void ViewBagForEditLicenseeProfile(ref EditLicenseeProfileCriteria criteria)
        //{
        //    var oddsTypes = baseService.GetData<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = "OddsType" });
        //    ViewBag.OddsTypeList = oddsTypes.Select(x => new SelectListItem { Value = x.Value, Text = x.Key }).ToList();

        //    var oddsStyles = baseService.GetData<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = "OddsRegion" });
        //    ViewBag.OddsStyleList = oddsStyles.Select(x => new SelectListItem { Value = x.Value, Text = x.Key }).ToList();

        //    var languages = baseService.GetData<Language>(new GetAllLanguageCriteria());
        //    ViewBag.LanguageIDList = languages.Select(x => new SelectListItem { Value = x.Code, Text = x.Name_EN }).ToList();

        //    ViewBag.DelayTimeList = GenerateDelayTime();
        //    ViewBag.ResultDelayTimeList = _licenseeService.GetResultDelayTimeList();

        //    Sport
        //   var sports = UpdateSportListForCricket(baseService.GetData<Sport>(new GetSupportedSportCriteria()));
        //    if (criteria.SportIDList.Equals("all"))
        //    {
        //        criteria.SportIDList = string.Join(",", sports.Select(item => item.SportType));
        //    }

        //    ViewBag.MajorSportList = sports.Where(sport => sport.Group == MajorGroup)
        //        .Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();
        //    ViewBag.MinorSportList = sports.Where(sport => sport.Group == MinorGroup)
        //        .Select(x => new SelectListItem { Value = x.SportType.ToString(), Text = x.SportName }).ToList();

        //    Market
        //   var bettypes = baseService.GetData<BetType>(new GetSupportedBettypeCriteria());
        //    if (criteria.BetTypeIDList.Equals("all"))
        //    {
        //        criteria.BetTypeIDList = string.Join(",", bettypes.Select(item => item.TypeId));
        //    }

        //    ViewBag.MajorBettypesList = bettypes.Where(x => x.Group == MajorGroup)
        //        .Select(x => new SelectListItem { Value = x.TypeId.ToString(), Text = x.TypeNameE }).ToList();
        //    ViewBag.MinorBettypesList = bettypes.Where(x => x.Group == MinorGroup)
        //        .Select(x => new SelectListItem { Value = x.TypeId.ToString(), Text = x.TypeNameE }).ToList();

        //    Country
        //   var countries = baseService.GetData<Country>(new GetAllCountryCriteria());
        //    ViewBag.CountriesList = countries.Select(x => new SelectListItem { Value = x.CountryCode, Text = x.CountryName }).ToList();
        //    if (criteria.CountryCodeList.Equals("all"))
        //    {
        //        criteria.CountryCodeList = string.Join(",", countries.Select(item => item.CountryCode));
        //    }

        //    League
        //   var leagues = baseService.GetData<League>(new GetLeagueCriteria
        //   {
        //       SportIDList = criteria.SportIDList,
        //       CountryCodeList = criteria.CountryCodeList
        //   });
        //    ViewBag.LeagueList = leagues.Select(x => new SelectListItem { Value = x.LeagueId.ToString(), Text = x.LeagueName }).ToList();
        //    if (criteria.LeagueIDList.Equals("all"))
        //    {
        //        criteria.LeagueIDList = string.Join(",", leagues.Select(item => item.LeagueId));
        //    }

        //    OutRight
        //   var outrights = baseService.GetData<League>(new GetOutRightCriteria
        //   {
        //       SportIDList = criteria.SportIDList,
        //       CountryCodeList = criteria.CountryCodeList
        //   });
        //    ViewBag.OutrightList = outrights.Select(x => new SelectListItem { Value = x.LeagueId.ToString(), Text = x.LeagueName }).ToList();
        //    if (criteria.OutRightIDList.Equals("all"))
        //    {
        //        criteria.OutRightIDList = string.Join(",", outrights.Select(item => item.LeagueId));
        //    }
        //}

        private static IEnumerable<Sport> UpdateSportListForCricket(IEnumerable<Sport> sports)
        {
            const byte Cricket = 50;
            return sports.Where(s => s.SportType != Cricket).ToList();
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
