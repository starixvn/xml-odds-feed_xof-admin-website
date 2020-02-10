// --------------------------------------------------------------------------
// <copyright file="LicenseeController.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.2.7">Created</change>
// </history>

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

    /// <summary>
    /// Class LicenseeController.
    /// </summary>
    [IsAuthorize]
    public class LicenseeController : BaseController
    {
        private const string MajorGroup = "Major";
        private const string MinorGroup = "Minor";

        #region Members

        /// <summary>
        /// The _licensee service.
        /// </summary>
        private readonly ILicenseeService _licenseeService;

        /// <summary>
        /// The _affiliate service.
        /// </summary>
        private readonly IAffiliateService _affiliateService;

        #endregion Members

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseeController"/> class.
        /// </summary>
        public LicenseeController()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseeController"/> class.
        /// </summary>
        /// <param name="licenseeService">The licensee service.</param>
        /// <param name="affiliateService">The affiliate service.</param>
        public LicenseeController(ILicenseeService licenseeService, IAffiliateService affiliateService)
        {
            _licenseeService = licenseeService ?? new LicenseeService { ServiceContext = ServiceContext };
            _affiliateService = affiliateService ?? new AffiliateService { ServiceContext = ServiceContext };
        }

        #endregion Constructors

        #region AddNew

        [HttpGet]
        public ActionResult Add()
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            ////OddsType
            ViewBag.OddsTypeList = _licenseeService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "OddsType" }, false);

            ////OddsStyle
            ViewBag.OddsStyleList = _licenseeService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "OddsRegion" }, false);

            //// Config
            var config = _licenseeService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "Config" }, false);
            ViewBag.DelayTimeList = GenerateDelayTime(config);
            ViewBag.NumberOfIPs = GetMaxNumberOfIPsFromConfig(config);

            ////Domain
            ViewBag.DomainList = _licenseeService.GetSelectList(new GetAllDomainCriteria
            {
                SystemID = 0,
                IsActive = null,
                IsOddsDomain = null
            });

            ////Market
            var bettypes = _affiliateService.GetSelectListWithGroup(new GetSupportedBettypeCriteria());

            ViewBag.MajorBettypesList = bettypes.Where(betType => betType.Group.Name == MajorGroup);
            ViewBag.MinorBettypesList = bettypes.Where(betType => betType.Group.Name == MinorGroup);

            ////Country
            ViewBag.CountriesList = _licenseeService.GetSelectList(new GetAllCountryCriteria());

            ////Sport
            var sports = _affiliateService.GetSelectListWithGroup(new GetSupportedSportCriteria());
            var sportList = UpdateSportListForCricket(sports);

            ViewBag.MajorSportList = sportList.Where(sport => sport.Group.Name == MajorGroup);
            ViewBag.MinorSportList = sportList.Where(sport => sport.Group.Name == MinorGroup);

            ////Language
            ViewBag.LanguageIDList = _licenseeService.GetSelectList(new GetAllLanguageCriteria());

            //// Licensees
            ViewBag.LicenseesList = _licenseeService.GetSelectList("Odds_LicenseeSel");

            var criteria = new CreateLicenseeCriteria();
            return View(criteria);
        }

        /// <summary>
        /// Insert licensee.
        /// </summary>
        /// <param name="criteria">The licensee's criteria.</param>
        /// <returns>Return 0 if success else return error code.</returns>
        public int CreateLicensee(CreateLicenseeCriteria criteria)
        {
            if (!criteria.AddExistingLicensee)
            {
                criteria.LicenseeID = 0;
            }

            criteria.SystemName = criteria.SystemName.Trim();
            criteria.ContactEmail = criteria.ContactEmail.Trim();
            criteria.Creator = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;
            criteria.BetTypeIDList = criteria.BetTypeIDList ?? string.Empty;
            criteria.LeagueKeyword = criteria.LeagueKeyword ?? string.Empty;
            criteria.LeagueKeyword_O = criteria.LeagueKeyword_O ?? string.Empty;

            criteria.SportIDList = UpdateSportIDListForCricket(criteria.SportIDList);
            var res = _licenseeService.UpdateData(criteria);
            return (int)res.Code;
        }

        #endregion AddNew

        #region Edit

        /// <summary>
        /// Edits the licensee.
        /// </summary>
        /// <param name="criteria">The licensee's criteria.</param>
        /// <returns>Return 0 if success else return error code.</returns>
        public int EditLicensee(EditLicenseeCriteria criteria)
        {
            criteria.Creator = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;
            var res = _licenseeService.UpdateData(criteria);
            return (int)res.Code;
        }

        /// <summary>
        /// Edits the active.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Return 0 if success else return error code.</returns>
        public int EditActive(UpdateLicenseeActiveCriteria criteria)
        {
            criteria.Creator = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;
            var res = _licenseeService.UpdateData(criteria);
            return (int)res.Code;
        }

        /// <summary>
        /// Display the page for editing the licensee's info.
        /// </summary>
        /// <param name="criteria">The licensee's info.</param>
        /// <returns>The licensee editing page.</returns>
        [HttpGet]
        public ActionResult EditLicenseeInfo(EditLicenseeInfoCriteria criteria)
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            ////var licensee = _licenseeService.GetDataSet(new GetLicenseeByLicenseeIDCriteria() { SystemID = criteria.SystemID });
            ////if (licensee != null && licensee.Tables.Count > 0 && licensee.Tables[0].Rows.Count > 0)
            ////{
            ////    DataRow row = licensee.Tables[0].Rows[0];
            ////    criteria.SystemName = row["SystemName"].ToString();
            ////    criteria.LicenseeID = int.Parse(row["LicenseeID"].ToString());
            ////    //criteria.IsActive = bool.Parse(row["IsActive"].ToString());
            ////    //criteria.AddExistingLicensee = criteria.LicenseeID != 0;

            ////    //ViewBag.LicenseesList = _licenseeService.GetSelectList("Odds_LicenseeSel");
            ////    ViewBag.CurrentSystem = " of \"" + criteria.SystemName + "\"";
            ////}

            ViewBag.CurrentSystem = " of \"" + criteria.SystemName + "\"";
            return View(criteria);
        }

        /// <summary>
        /// Edit licensee's profile.
        /// </summary>
        /// <param name="criteria">The licensee's info.</param>
        /// <returns>Return 0 if success else return error code.</returns>
        [HttpPost]
        public int EditLicenseeInfoPost(EditLicenseeInfoCriteria criteria)
        {
            if (!criteria.AddExistingLicensee)
            {
                criteria.LicenseeID = 0;
            }

            criteria.Creator = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;
            var res = _licenseeService.UpdateData(criteria);
            return (int)res.Code;
        }

        /// <summary>
        /// Display the licensee's setting page for editing.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The page for editing.</returns>
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

            var licensee = _licenseeService.GetDataSet(new GetLicenseeSettingByLicenseeIDCriteria { SystemID = criteria.SystemID });
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
            var selectedDomains = _licenseeService.GetSelectList(new GetDomainBySystemIDCriteria
            {
                SystemID = (byte)criteria.SystemID
            });

            ViewBag.SelectedDomainList = selectedDomains;
            ViewBag.DomainList = _licenseeService.GetSelectList(new GetAllDomainCriteria
            {
                SystemID = 0,
                IsActive = null,
                IsOddsDomain = null
            }).Where(d => !selectedDomains.Any(s => s.Value == d.Value));

            ViewBag.CurrentSystem = " of \"" + criteria.SystemName + "\"";

            return View(criteria);
        }

        /// <summary>
        /// Edits the licensee's setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Return 0 if success else return error code.</returns>
        [HttpPost]
        public int EditSettingPost(EditLicenseeSettingCriteria criteria)
        {
            criteria.Creator = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;
            var res = _licenseeService.UpdateData(criteria);
            return (int)res.Code;
        }

        /// <summary>
        /// Displays the licensee's profile for editing.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The page for editing.</returns>
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

            ViewBag.LicenseesList = _licenseeService.GetSelectList("Odds_LicenseeSel");
            ViewBag.CurrentSystem = " of \"" + criteria.SystemName + "\"";

            return View(criteria);
        }

        /// <summary>
        /// Edits the licensee's profile.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Represents a JSON-formatted content for editing.</returns>
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

        /// <summary>
        /// Updates the IP range_ edit setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Represents a JSON-formatted content for editing.</returns>
        [HttpPost]
        public JsonResult UpdateIPRange_EditSetting(UpdateSettingIP_LicenseeCriteria criteria)
        {
            criteria.Creator = CurrentSession.UserId;
            criteria.IpFromOld = string.IsNullOrEmpty(criteria.IpFromOld) ? string.Empty : criteria.IpFromOld;
            criteria.IpToOld = string.IsNullOrEmpty(criteria.IpToOld) ? string.Empty : criteria.IpToOld;

            var resultMessage = string.Empty;
            var result = _licenseeService.Update(criteria);
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

        /// <summary>
        /// Lists the licensee.
        /// </summary>
        /// <returns>The licensee list page.</returns>
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
            ViewBag.LeagueList = _licenseeService.GetSelectList(criteria);

            return PartialView("_LeaguePartialView", criteria);
        }

        public ActionResult GetOutright(GetOutRightCriteria criteria)
        {
            criteria.SportIDList = UpdateSportIDListForCricket(criteria.SportIDList);
            ViewBag.OutrightList = _licenseeService.GetSelectList(criteria);

            return PartialView("_OutRightPartialView", criteria);
        }

        /// <summary>
        /// Display the licensees match with name.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Represents a JSON-formatted content.</returns>
        public JsonResult GetLicenseeByName(GetLicenseeByPageCriteria criteria)
        {
            criteria.PageIndex++;
            if (criteria.SystemName == null || criteria.SystemName == "Licensee Name")
            {
                criteria.SystemName = string.Empty;
            }

            var dr = _licenseeService.GetData(criteria);
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

            var dr = _licenseeService.GetData(criteria);
            return FanexGridHelper.DataForFanexGrid(dr);
        }

        /// <summary>
        /// Gets the licensee requests detail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="date">The date of request.</param>
        /// <param name="year">The year of request.</param>
        /// <returns>The content of licensee requests detail.</returns>
        public string GetLicenseeRequestsDetail(RequestStatisticCriteria criteria, string date, int year)
        {
            const int numberDaysInWeek = 7;

            if (!string.IsNullOrEmpty(date))
            {
                var dateStart = new DateTime();
                var dateEnd = new DateTime();
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

        /// <summary>
        /// Lists the existed systems.
        /// </summary>
        /// <param name="systemName">Name of the system.</param>
        /// <returns>Represents a JSON-formatted content.</returns>
        public JsonResult ListExistedSystems(string systemName)
        {
            var criteria = new GetTop10LicenseeCriteria
            {
                Systemname = systemName
            };

            var top10SystemList = _licenseeService.GetDataSet(criteria).Tables[0];
            return JsonForAutoComplete(top10SystemList, "SystemID", "SystemName");
        }

        public string LicenseeList_GetTop10(string systemName)
        {
            var criteria = new GetTop10LicenseeCriteria
            {
                Systemname = systemName
            };

            var top10SystemList = _licenseeService.GetDataSet(criteria).Tables[0];
            return DataForAutoComplete(top10SystemList, "SystemName");
        }

        /// <summary>
        /// Checks the name of the exist licensee.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns><c>true</c> if 0, <c>false</c> otherwise.</returns>
        public bool CheckExistLicenseeName(CheckExistLicenseeCriteria criteria)
        {
            const int DefaultPageSize = 10;

            criteria.PageIndex = 1;
            criteria.PageSize = DefaultPageSize;
            var res = _licenseeService.GetDataSet(criteria);
            return res.Tables[0].Rows.Count == 0;
        }

        #endregion Get

        #region Delete

        /// <summary>
        /// Deletes the licensee.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Return 0 if success else return error code.</returns>
        public int DeleteLicensee(DeleteLicenseeCriteria criteria)
        {
            criteria.Creator = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;
            var res = _licenseeService.UpdateData(criteria);
            return (int)res.Code;
        }

        /// <summary>
        /// Edits the IP range.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Represents a JSON-formatted content.</returns>
        [HttpPost]
        public JsonResult DeleteIPRange_EditSetting(DeleteSettingIP_LicenseeCriteria criteria)
        {
            var deleteResult = new OddsFeedActionResult();
            criteria.Creator = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;
            criteria.IPFromOld = string.IsNullOrEmpty(criteria.IPFromOld) ? string.Empty : criteria.IPFromOld;
            criteria.IPToOld = string.IsNullOrEmpty(criteria.IPToOld) ? string.Empty : criteria.IPToOld;

            var result = this._licenseeService.Update(criteria);
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

        /// <summary>
        /// Gets the missing profile.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The structure of MissingPartnersProfile.</returns>
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

        /// <summary>
        /// Determines whether is missing profile.
        /// </summary>
        /// <param name="missingPartnersProfile">The missing partners profile.</param>
        /// <returns><c>true</c> if is missing profile; otherwise, <c>false</c>.</returns>
        private bool IsMissingProfile(MissingPartnersProfile missingPartnersProfile)
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
                                                                    ? criteria.LanguageList
                                                                    : missingPartnersProfile.AffsLanguageList + "," + criteria.LanguageList;
            missingPartnersProfile.AffsSportIDList = string.IsNullOrEmpty(missingPartnersProfile.AffsSportIDList)
                                                                ? criteria.SportIDList : missingPartnersProfile.AffsSportIDList + "," + criteria.SportIDList;
            missingPartnersProfile.AffsCountryCodeList = string.IsNullOrEmpty(missingPartnersProfile.AffsCountryCodeList)
                                                                        ? criteria.CountryCodeList
                                                                        : missingPartnersProfile.AffsCountryCodeList + "," + criteria.CountryCodeList;
            missingPartnersProfile.AffsLeagueIDList = string.IsNullOrEmpty(missingPartnersProfile.AffsLeagueIDList)
                                                                    ? criteria.LeagueIDList
                                                                    : missingPartnersProfile.AffsLeagueIDList + "," + criteria.LeagueIDList;
            missingPartnersProfile.AffsOutrightIDList = string.IsNullOrEmpty(missingPartnersProfile.AffsOutrightIDList)
                                                                    ? criteria.OutRightIDList
                                                                    : missingPartnersProfile.AffsOutrightIDList + "," + criteria.OutRightIDList;
            missingPartnersProfile.AffsBettypeIDList = string.IsNullOrEmpty(missingPartnersProfile.AffsBettypeIDList)
                                                                    ? criteria.BetTypeIDList
                                                                    : missingPartnersProfile.AffsBettypeIDList + "," + criteria.BetTypeIDList;

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
            criteria.Creator = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;
            criteria.BettypeIDListFlag = criteria.BettypeIDListFlag && criteria.BetTypeIDList != null;
            var res = _licenseeService.UpdateData(criteria);

            var missingPartnersProfile = new MissingPartnersProfile
            {
                ErrCode = (int)res.Code
            };
            return Json(missingPartnersProfile, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the licensee profile.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        private void GetLicenseeProfile(ref EditLicenseeProfileCriteria criteria)
        {
            var licensee = _licenseeService.GetDataSet(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = criteria.SystemID });
            if (licensee != null && licensee.Tables.Count > 0 && licensee.Tables[0].Rows.Count > 0)
            {
                var row = licensee.Tables[0].Rows[0];
                criteria.NumberOfIPs = short.Parse(row["NumberOfIPs"].ToString());
                criteria.BetTypeIDList = row["BetTypeIDList"].ToString();
                criteria.BettypeIDListFlag = bool.Parse(row["BettypeIDListFlag"].ToString());
                criteria.CountryCodeList = row["CountryCodeList"].ToString();
                criteria.CountryCodeListFlag = bool.Parse(row["CountryCodeListFlag"].ToString());
                var delayTimes = row["DelayTimes"].ToString().Split(',');
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

                criteria.DelayTimesFlag = bool.Parse(row["DelayTimesFlag"].ToString());
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

        /// <summary>
        /// Declare view bags for edit licensee profile.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        private void ViewBagForEditLicenseeProfile(ref EditLicenseeProfileCriteria criteria)
        {
            ViewBag.OddsTypeList = _licenseeService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "OddsType" }, false);
            ViewBag.OddsStyleList = _licenseeService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "OddsRegion" }, false);
            ViewBag.LanguageIDList = _licenseeService.GetSelectList(new GetAllLanguageCriteria());
            var config = _licenseeService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "Config" }, false);
            ViewBag.DelayTimeList = GenerateDelayTime(config);

            ////Sport
            var sports = _affiliateService.GetSelectListWithGroup(new GetSupportedSportCriteria());
            var sportList = UpdateSportListForCricket(sports);
            if (criteria.SportIDList.Equals("all"))
            {
                criteria.SportIDList = string.Join(",", sports.Select(item => item.Value));
            }

            ViewBag.MajorSportList = sportList.Where(sport => sport.Group.Name == "Major");
            ViewBag.MinorSportList = sportList.Where(sport => sport.Group.Name == "Minor");

            ////Market
            var bettypes = _affiliateService.GetSelectListWithGroup(new GetSupportedBettypeCriteria());

            if (criteria.BetTypeIDList.Equals("all"))
            {
                criteria.BetTypeIDList = string.Join(",", bettypes.Select(item => item.Value));
            }

            ViewBag.MajorBettypesList = bettypes.Where(betType => betType.Group.Name == "Major");
            ViewBag.MinorBettypesList = bettypes.Where(betType => betType.Group.Name == "Minor");

            ////Country
            var countries = _affiliateService.GetSelectList(new GetAllCountryCriteria());
            ViewBag.CountriesList = countries;
            if (criteria.CountryCodeList.Equals("all"))
            {
                criteria.CountryCodeList = string.Join(",", countries.Select(item => item.Value));
            }
            //// League
            var leagues = _affiliateService.GetSelectList(new GetLeagueCriteria
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
            var outrights = _affiliateService.GetSelectList(new GetOutRightCriteria
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

            return String.Join(",", sportList.ToArray());
        }

        #endregion Common

    }
}