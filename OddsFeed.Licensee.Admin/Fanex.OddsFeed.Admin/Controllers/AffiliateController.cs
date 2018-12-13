// --------------------------------------------------------------------------
// <copyright file="AffiliateController.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

/// <summary>
/// The Controllers namespace.
/// </summary>
namespace Fanex.OddsFeed.Admin.Controllers
{
    using Fanex.OddsFeed.Core;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Criteria.Licensee;
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Service.Impl;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Core.Web.Attribute;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web.Mvc;
    /// <summary>
    /// Class AffiliateController.
    /// </summary>
    [IsAuthorize]
    ////[SuppressMessage(
    ////     "StyleCop.CSharp.StylecopPlus.Rules",
    ////     "SP2101:MethodMustNotContainMoreLinesThan",
    ////     Justification = "Reviewed. Suppression is OK here.")]
    public class AffiliateController : BaseController
    {
        #region Members

        /// <summary>
        /// The _affiliate service.
        /// </summary>
        private readonly IAffiliateService _affiliateService;

        /// <summary>
        /// The _licensee service.
        /// </summary>
        private readonly ILicenseeService _licenseeService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AffiliateController"/> class.
        /// </summary>
        public AffiliateController()
            : this(null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AffiliateController"/> class.
        /// </summary>
        /// <param name="affiliateService">The affiliate service.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="licenseeService">The licensee service.</param>
        public AffiliateController(
                                        IAffiliateService affiliateService = null,
                                        IUserService userService = null,
                                        ILicenseeService licenseeService = null)
        {
            _affiliateService = affiliateService ?? new AffiliateService { ServiceContext = ServiceContext };
            ////_userService = userService ?? new UserService();
            _licenseeService = licenseeService ?? new LicenseeService { ServiceContext = ServiceContext };
        }

        #endregion

        #region Add New

        /// <summary>
        /// Display the adding affiliate page.
        /// </summary>
        /// <returns>The page for adding affiliate.</returns>
        [HttpGet]
        public ActionResult Add()
        {
            var criteria = new InsertAffiliateCriteria();
            ViewBag.VirtualPath = GetVirtualPath();

            ////Domain
            IEnumerable<SelectListItem> domainList = new List<SelectListItem>();
            domainList = _affiliateService.GetDomain();
            ViewData["slAssignedDomain"] = domainList;
            ViewData["slDomain"] = new List<SelectListItem>();

            ////Profile                
            var licenseeProfile = _affiliateService.GetDataSet(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = CurrentSession.SystemId });
            if (licenseeProfile != null && licenseeProfile.Tables.Count > 0 && licenseeProfile.Tables[0].Rows.Count > 0)
            {
                var row = licenseeProfile.Tables[0].Rows[0];
                criteria.OddsRegion = row["OddsRegion"].ToString();
                criteria.NumberOfIPs = (short)row["NumberOfIPs"];
                ////Delay Times
                var delayTimes = row["DelayTimes"].ToString().Split(',');
                criteria.DeadballDelayTimes = delayTimes.Length > 0 ? delayTimes[0] : "0";
                criteria.LiveDelayTimes = delayTimes.Length > 1 ? delayTimes[1] : "0";
                criteria.OutrightDelayTimes = delayTimes.Length > 2 ? delayTimes[2] : "0";
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
                ////Sub-Acc
                AddAffiliateWithSubAccountRole();
            }

            return View(criteria);
        }

        /// <summary>
        /// Add new an affiliate.
        /// </summary>
        /// <param name="criteria">The affiliate's criteria.</param>
        /// <returns>Represents a JSON-formatted content for adding.</returns>
        [HttpPost]
        public JsonResult AddNew(InsertAffiliateCriteria criteria)
        {
            var actionResult = new OddsFeedActionResult();
            CustomizeDataBeforeAddNew(ref criteria);

            var result = _affiliateService.Insert(criteria);
            if (result != null && result.IsSuccessful)
            {
                actionResult.IsSuccess = true;
                actionResult.ResultMessage = ViewResources.ActionResult_AddAffiliateSuccess;
            }
            else
            {
                actionResult.IsSuccess = false;
                actionResult.ResultMessage = result.Message;
            }

            return Json(actionResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Check exist an affiliate.
        /// </summary>
        /// <param name="criteria">The affiliate's criteria.</param>
        /// <returns>Represents a JSON-formatted content for checking.</returns>
        [HttpPost]
        public JsonResult CheckExistAffiliate(InsertAffiliateCriteria criteria)
        {
            var checkResult = new OddsFeedActionResult();
            if (string.IsNullOrEmpty(criteria.UserName))
            {
                checkResult.IsSuccess = true;
                return Json(checkResult, JsonRequestBehavior.AllowGet);
            }

            var res = this._affiliateService.GetAffiliateListReturnDataCollection(new CheckExistAffiliateCriteria
            {
                UserName = criteria.UserName,
                PageIndex = 1,
                PageSize = 1,
                SystemId = CurrentSession.SystemId
            });

            if (res.Data.Tables.Count > 0 &&
                res.Data.Tables[0].Rows.Count > 0)
            {
                checkResult.IsSuccess = false;
            }

            return Json(checkResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Edit
        /// <summary>
        /// Display the affiliate info page for editing.
        /// </summary>
        /// <param name="id">The affiliate's id.</param>
        /// <param name="userName">The affiliate's username.</param>
        /// <returns>The page for editing the affiliate.</returns>
        [HttpGet]
        public ActionResult EditAffiliateInfo(int id, string userName)
        {
            if (id == 0)
            {
                return RedirectToAction("List", "Affiliate");
            }

            var criteria = new UpdateAffiliateCriteria
            {
                PartnerID = id,
                UserName = userName
            };

            var dr = _affiliateService.GetAffiliateListReturnDataCollection(new GetAffiliateByNameCriteria
            {
                SystemId = CurrentSession.SystemId,
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

        /// <summary>
        /// Editing the affiliate's info.
        /// </summary>
        /// <param name="criteria">The affiliate's criteria.</param>
        /// <returns>Represents a JSON-formatted content for editing.</returns>
        [HttpPost]
        public JsonResult EditAffiliateInfo(UpdateAffiliateCriteria criteria)
        {
            var actionResult = new OddsFeedActionResult();
            if (ModelState.IsValid)
            {
                criteria.UpdatedBy = CurrentSession.UserId;
                criteria.Email = criteria.Email ?? string.Empty;
                criteria.Website = criteria.Website ?? string.Empty;
                criteria.Note = criteria.Note ?? string.Empty;

                var result = _affiliateService.Update(criteria);
                if (result != null && result.IsSuccessful)
                {
                    actionResult.IsSuccess = true;
                    actionResult.ResultMessage = ViewResources.ActionResult_UpdateAffiliateInfoSuccess;
                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.ResultMessage = result.Message;
                }
            }

            return Json(actionResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Display the affiliate setting page for editing.
        /// </summary>
        /// <param name="id">The affiliate's id.</param>
        /// <param name="name">The affiliate's name.</param>
        /// <returns>The page for editing affiliate's setting.</returns>
        [HttpGet]
        public ActionResult EditSetting(int id, string name)
        {
            if (id == 0)
            {
                return RedirectToAction("List", "Affiliate");
            }

            ViewBag.CurrentUser = " of \"" + name + "\"";
            ViewBag.AffId = id;

            var affiliateProfile = _affiliateService.GetDataSet(new GetAffiliateProfileByIDCriteria { PartnerID = id });
            if (affiliateProfile != null && affiliateProfile.Tables.Count > 0 && affiliateProfile.Tables[0].Rows.Count > 0)
            {
                ViewBag.NumberOfIPs = int.Parse(affiliateProfile.Tables[0].Rows[0]["NumberOfIPs"].ToString());
            }

            ////Domain             
            var domainSelList = _affiliateService.GetDomainByAffId(id);
            ViewData["slAssignedDomain"] = domainSelList;

            var domainList = _affiliateService.GetDomain();
            domainList = domainList.Where(d => !domainSelList.Any(s => s.Value == d.Value)).ToList();
            ViewData["slDomain"] = domainList;

            ////IP
            IList<PartnerIP> listIPs = new List<PartnerIP>();
            listIPs = _affiliateService.GetPartnerIpByAffId(id);
            ViewBag.IpList = listIPs;

            return View();
        }

        /// <summary>
        /// Editing the affiliate's setting.
        /// </summary>
        /// <param name="affId">The affiliate's id.</param>
        /// <param name="domains">The list of domains.</param>
        /// <param name="listIPs">The list of IP.</param>
        /// <returns>Represents a JSON-formatted content for editing.</returns>
        [HttpPost]
        public JsonResult EditSetting(int affId, string domains, string listIPs)
        {
            var actionResult = new OddsFeedActionResult();
            var fromIPs = string.Empty;
            var toIPs = string.Empty;

            if (!string.IsNullOrEmpty(listIPs))
            {
                var arrIps = listIPs.Split(',');
                for (int i = 0; i < arrIps.Length; i += 2)
                {
                    fromIPs += arrIps[i] + ",";
                    toIPs += arrIps[i + 1] + ",";
                }
            }

            var criteria = new UpdateSettingCriteria
            {
                PartnerID = affId,
                Domains = domains,
                IpsFrom = fromIPs,
                IpsTo = toIPs
            };

            var result = _affiliateService.Update(criteria);
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
                actionResult.ResultMessage = result.Message;
            }

            return Json(actionResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Editing the affiliate's IP.
        /// </summary>
        /// <param name="affId">The affiliate's id.</param>
        /// <param name="fromIP">The IP from.</param>
        /// <param name="toIP">The IP to.</param>
        /// <param name="fromIPOld">The old IP from.</param>
        /// <param name="toIPOld">The old IP to.</param>
        /// <returns>Return "Successfully!" if success else return the error message.</returns>
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

            var result = _affiliateService.Update(criteria);
            if (result != null && result.IsSuccessful)
            {
                ViewBag.Success = true;
                ViewBag.PartnerID = criteria.PartnerID;
                return "Successfully!";
            }
            else
            {
                return result.Message;
            }
        }

        /// <summary>
        /// Editing the affiliate's IP range.
        /// </summary>
        /// <param name="criteria">The affiliate's IP range criteria.</param>
        /// <returns>Represents a JSON-formatted content for editing.</returns>
        [HttpPost]
        public JsonResult DeleteIPRange_EditSetting_Partner(DeleteSettingIP_AffiliateCriteria criteria)
        {
            var deleteResult = new OddsFeedActionResult();
            criteria.Creator = CurrentSession.UserId;
            criteria.IPFromOld = string.IsNullOrEmpty(criteria.IPFromOld) ? string.Empty : criteria.IPFromOld;
            criteria.IPToOld = string.IsNullOrEmpty(criteria.IPToOld) ? string.Empty : criteria.IPToOld;
            criteria.IP = CurrentContext.IP;

            var result = this._affiliateService.Update(criteria);
            if (result != null && result.IsSuccessful)
            {
                deleteResult.IsSuccess = true;
                deleteResult.ResultMessage = ViewResources.ActionResult_Success_DeleteIPSetting;
            }
            else
            {
                deleteResult.IsSuccess = false;
                deleteResult.ErrorCode = result.Code;
                deleteResult.ResultMessage = result.Message;
            }

            return Json(deleteResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Display the affiliate's profile for editing.
        /// </summary>
        /// <param name="id">The affiliate's id.</param>
        /// <param name="name">The affiliate's name.</param>
        /// <returns>The page for editing.</returns>
        [HttpGet]
        public ActionResult EditProfile(int id, string name)
        {
            if (id == 0)
            {
                return RedirectToAction("List", "Affiliate");
            }

            var sportIDList = string.Empty;
            var countryCodeList = string.Empty;
            var criteria = new UpdateProfileCriteria { PartnerID = id };
            ViewBag.CurrentUser = " of \"" + name + "\"";

            ////Profile
            var affiliateProfile = _affiliateService.GetDataSet(new GetAffiliateProfileByIDCriteria { PartnerID = id });
            if (affiliateProfile != null && affiliateProfile.Tables.Count > 0 && affiliateProfile.Tables[0].Rows.Count > 0)
            {
                var row = affiliateProfile.Tables[0].Rows[0];
                criteria.OddsType = row["OddsType"].ToString();
                criteria.OddsRegion = row["OddsRegion"].ToString();
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
                ////Sub-Acc
                GetAffiliateProfileWithSubAccountRole(ref criteria);
            }

            return View(criteria);
        }

        /// <summary>
        /// Editing the affiliate's profile.
        /// </summary>
        /// <param name="criteria">The affiliate's criteria.</param>
        /// <returns>Represents a JSON-formatted content for editing.</returns>
        [HttpPost]
        public JsonResult EditProfile(UpdateProfileCriteria criteria)
        {
            var actionResult = new OddsFeedActionResult();
            if (ModelState.IsValid)
            {
                CustomizeDataBeforeEditProfile(ref criteria);

                var result = _affiliateService.Update(criteria);
                if (result != null && result.IsSuccessful)
                {
                    actionResult.IsSuccess = true;
                    actionResult.ResultMessage = ViewResources.ActionResult_UpdateAffiliateInfoSuccess;
                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.ResultMessage = result.Message;
                }
            }

            return Json(actionResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Editing the affiliate's active status.
        /// </summary>
        /// <param name="affId">The affiliate's id.</param>
        /// <param name="status">The affiliate's active status.</param>
        /// <returns>Return "Successfully!" if success else return the error message.</returns>
        public string EditActive(int affId, bool status)
        {
            var criteria = new UpdateActiveCriteria
            {
                PartnerID = affId,
                IsAllowed = status,
                Creator = CurrentSession.UserId
            };

            var result = _affiliateService.Update(criteria);

            return (result != null && result.IsSuccessful) ? "Successfully!" : result.Message;
        }

        #endregion

        #region Get

        [HttpGet]
        [Compress]
        public ActionResult List()
        {
            ViewBag.SystemName = CurrentSession.SystemName;
            ViewBag.IsActivateSystem = _licenseeService.GetActiveBySystemId(CurrentSession.SystemId);
            return View("AffiliateList");
        }

        /// <summary>
        /// Display the list of affiliates.
        /// </summary>
        /// <param name="criteria">The criteria for displaying.</param>
        /// <returns>Represents a JSON-formatted content for displaying.</returns>
        [Compress]
        public JsonResult GetAffiliateList(GetAffiliateByNameCriteria criteria)
        {
            criteria.SystemId = CurrentSession.SystemId;
            criteria.UserName = (criteria.UserName == null || criteria.UserName == "Username") ? string.Empty : criteria.UserName.Trim();
            criteria.PageIndex++;

            var dr = _affiliateService.GetAffiliateListReturnDataCollection(criteria);
            var result = FanexGridHelper.DataForFanexGrid_PreventContainQuote(dr);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string ListExistedAff(string username)
        {
            var criteria = new GetTop10AffiliateCriteria
            {
                SystemId = CurrentSession.SystemId,
                Username = username
            };
            var top10AffList = _affiliateService.GetTop10AffReturnDataSet(criteria).Tables[0];
            return DataForAutoComplete(top10AffList, "username");
        }

        public JsonResult ListExistedAffJson(string username)
        {
            var criteria = new GetTop10AffiliateCriteria
            {
                SystemId = CurrentSession.SystemId,
                Username = username
            };
            var top10DomainList = _affiliateService.GetDataSet(criteria).Tables[0];
            return JsonForAutoComplete(top10DomainList, "PartnerID", "UserName");
        }

        public JsonResult ListExistedDomainsJson(string domainName)
        {
            var criteria = new GetTop10DomainCriteria
            {
                SystemId = CurrentSession.SystemId,
                DomainName = domainName
            };
            var top10DomainList = _affiliateService.GetDataSet(criteria).Tables[0];
            return JsonForAutoComplete(top10DomainList, "DomainID", "DomainName");
        }

        [HttpGet]
        public ActionResult Requests(string curmonth = "")
        {
            ViewBag.SystemName = CurrentSession.SystemName;
            if (curmonth != string.Empty)
            {
                var curDate = DateTime.Now;
                ViewBag.StartDateConfig = new DateTime(curDate.Year, curDate.Month, 1);
            }

            return View("AffiliateRequest", new RequestStatisticCriteria());
        }

        /// <summary>
        /// Display the request statistic of affiliate.
        /// </summary>
        /// <param name="criteria">The request statistic criteria.</param>
        /// <returns>Represents a JSON-formatted content of request statistic.</returns>
        public JsonResult GetPartnerRequestStatistic(RequestStatisticCriteria criteria)
        {
            if (string.IsNullOrEmpty(criteria.PartnerName) || criteria.PartnerName.ToLower() == ViewResources.Affiliate_Name.ToLower())
            {
                criteria.PartnerName = string.Empty;
            }

            if (string.IsNullOrEmpty(criteria.DomainName) || criteria.DomainName.ToLower() == ViewResources.Domain_Name.ToLower())
            {
                criteria.DomainName = string.Empty;
            }

            criteria.SystemId = CurrentSession.SystemId;
            criteria.PageIndex++;

            var dr = _affiliateService.GetPartnerRequestStatisticReturnDataCollection(criteria);
            var result = FanexGridHelper.DataForFanexGrid(dr);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Display xml URLs of affiliate.
        /// </summary>
        /// <param name="id">The affiliate's id.</param>
        /// <param name="name">The affiliate's name.</param>
        /// <returns>The xml URLs page.</returns>
        [HttpGet]
        public ActionResult ViewXMLURLs_Edit(int id, string name)
        {
            if (id == 0)
            {
                return RedirectToAction("List", "Affiliate");
            }

            ViewBag.CurrentUser = " for \"" + name + "\"";
            ViewBag.AffiliateName = name;
            ViewBag.AssignedDomain = _affiliateService.GetDomainByAffId(id);
            return View();
        }

        /// <summary>
        /// Gets the XML odds feed link.
        /// </summary>
        /// <param name="domainURL">The domain URL.</param>
        /// <param name="affiliateName">Name of the affiliate.</param>
        /// <returns>The XML odds feed link. </returns>
        public string GetXMLOddsFeedLink(string domainURL, string affiliateName)
        {
            return string.IsNullOrEmpty(domainURL) ?
                string.Empty :
                "http://" + domainURL.Replace("http://", string.Empty) + "/" + GetVirtualPath() + affiliateName + "/odds.xml";
        }

        /// <summary>
        /// Gets the XML odds feed_ help link.
        /// </summary>
        /// <param name="domainURL">The domain URL.</param>
        /// <param name="affiliateName">Name of the affiliate.</param>
        /// <returns>The XML odds feed link. </returns>
        public string GetXMLOddsFeed_HelpLink(string domainURL, string affiliateName)
        {
            return string.IsNullOrEmpty(domainURL) ?
                string.Empty :
                "http://" + domainURL.Replace("http://", string.Empty) + "/" + GetVirtualPath() + affiliateName + "/help.htm";
        }

        /// <summary>
        /// Gets the XML odds feed link.
        /// </summary>
        /// <param name="domainURL">The domain URL.</param>
        /// <param name="affiliateName">Name of the affiliate.</param>
        /// <returns>The XML odds feed link. </returns>
        public string GetBetRadarLink(string domainURL, string affiliateName)
        {
            return string.IsNullOrEmpty(domainURL) ?
                string.Empty :
                "http://" + domainURL.Replace("http://", string.Empty) + "/" + GetVirtualPath() + affiliateName + "/oddsfeed.xml";
        }

        #endregion

        #region Delete

        /// <summary>
        /// Delete the affiliate.
        /// </summary>
        /// <param name="affId">The affiliate's id.</param>
        /// <returns>Return "Successfully!" if success else the error message.</returns>
        public string Delete(int affId)
        {
            var criteria = new DeleteAffiliateCriteria
            {
                Creator = CurrentSession.UserId,
                PartnerID = affId
            };

            var result = _affiliateService.Delete(criteria);

            return result != null && result.IsSuccessful
                ? "Successfully!"
                : result.Message;
        }

        #endregion        

        #region Common

        /// <summary>
        /// Customize data before inserting an affiliate.
        /// </summary>
        /// <param name="criteria">The affiliate's criteria.</param>
        private void CustomizeDataBeforeAddNew(ref InsertAffiliateCriteria criteria)
        {
            var fromIPs = string.Empty;
            var toIPs = string.Empty;

            if (!string.IsNullOrEmpty(criteria.IpsFrom))
            {
                var arrIps = criteria.IpsFrom.Split(',');
                for (int i = 0; i < arrIps.Length; i += 2)
                {
                    fromIPs += arrIps[i] + ",";
                    toIPs += arrIps[i + 1] + ",";
                }
            }

            criteria.SystemID = CurrentSession.SystemId;
            criteria.Creator = CurrentSession.UserId;
            criteria.IpsFrom = fromIPs;
            criteria.IpsTo = toIPs;
            criteria.Ip = CurrentContext.IP;
            criteria.Content = criteria.Content ?? string.Empty;
            criteria.Note = criteria.Note ?? string.Empty;
            criteria.Domains = criteria.Domains ?? string.Empty;
            criteria.BetTypeIDList = criteria.BetTypeIDList ?? string.Empty;
            criteria.LeagueIDList = criteria.LeagueIDList ?? string.Empty;
            criteria.OutRightIDList = criteria.OutRightIDList ?? string.Empty;
            criteria.LeagueKeyword = criteria.LeagueKeyword ?? string.Empty;
            criteria.LeagueKeyword_O = criteria.LeagueKeyword_O ?? string.Empty;

            criteria.SportIDList = UpdateSportIDListForCricket(criteria.SportIDList);
            CustomizeInsertProfile(ref criteria);
        }

        /// <summary>
        /// Customize data before editing affiliate's profile.
        /// </summary>
        /// <param name="criteria">The affiliate's profile criteria.</param>
        private void CustomizeDataBeforeEditProfile(ref UpdateProfileCriteria criteria)
        {
            criteria.OddsType = criteria.OddsType ?? string.Empty;
            criteria.OddsRegion = criteria.OddsRegion ?? string.Empty;
            criteria.LanguageList = criteria.LanguageList ?? string.Empty;
            criteria.SportIDList = criteria.SportIDList ?? string.Empty;
            criteria.CountryCodeList = criteria.CountryCodeList ?? string.Empty;
            criteria.BetTypeIDList = criteria.BetTypeIDList ?? string.Empty;

            criteria.Creator = CurrentSession.UserId;

            criteria.SportIDList = UpdateSportIDListForCricket(criteria.SportIDList);
            CustomizeUpdateProfile(ref criteria);
        }

        private string UpdateSportIDListForCricket(string sportIDList)
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
            else if (!sportList.Contains("27") && sportList.Contains("50"))
            {
                sportList.Remove("50");
            }

            return String.Join(",", sportList.ToArray());
        }


        /// <summary>
        /// Customize for inserting the affiliate's profile.
        /// </summary>
        /// <param name="criteria">The affiliate's profile criteria.</param>
        private void CustomizeInsertProfile(ref InsertAffiliateCriteria criteria)
        {
            if (!string.IsNullOrEmpty(criteria.LeagueIDList))
            {
                var leagues =
                    _affiliateService.GetSelectList(new GetLeagueCriteria { SportIDList = criteria.SportIDList, CountryCodeList = criteria.CountryCodeList });
                if (Utilities.IsExistInSelectListItem(criteria.LeagueIDList, leagues))
                {
                    criteria.LeagueIDList = "all";
                }
            }

            if (!string.IsNullOrEmpty(criteria.OutRightIDList))
            {
                var outrights = _affiliateService.GetSelectList(new GetOutRightCriteria
                {
                    SportIDList = criteria.SportIDList,
                    CountryCodeList = criteria.CountryCodeList
                });
                if (Utilities.IsExistInSelectListItem(criteria.OutRightIDList, outrights))
                {
                    criteria.OutRightIDList = "all";
                }
            }

            if (!string.IsNullOrEmpty(criteria.SportIDList))
            {
                var sports = _affiliateService.GetSelectList(new GetSupportedSportCriteria());
                if (Utilities.IsExistInSelectListItem(criteria.SportIDList, sports))
                {
                    criteria.SportIDList = "all";
                }
            }

            if (!string.IsNullOrEmpty(criteria.CountryCodeList))
            {
                var countries = _affiliateService.GetSelectList(new GetAllCountryCriteria());
                if (Utilities.IsExistInSelectListItem(criteria.CountryCodeList, countries))
                {
                    criteria.CountryCodeList = "all";
                }
            }

            if (!string.IsNullOrEmpty(criteria.BetTypeIDList))
            {
                var bettypes = _affiliateService.GetSelectList(new GetSupportedBettypeCriteria());
                if (Utilities.IsExistInSelectListItem(criteria.BetTypeIDList, bettypes))
                {
                    criteria.BetTypeIDList = "all";
                }
            }
        }

        /// <summary>
        /// Customize for editing the affiliate's profile.
        /// </summary>
        /// <param name="criteria">The affiliate's profile criteria.</param>
        private void CustomizeUpdateProfile(ref UpdateProfileCriteria criteria)
        {
            if (!string.IsNullOrEmpty(criteria.LeagueIDList))
            {
                var leagues =
                    _affiliateService.GetSelectList(new GetLeagueCriteria { SportIDList = criteria.SportIDList, CountryCodeList = criteria.CountryCodeList });
                if (Utilities.IsExistInSelectListItem(criteria.LeagueIDList, leagues))
                {
                    criteria.LeagueIDList = "all";
                }
            }

            if (!string.IsNullOrEmpty(criteria.OutRightIDList))
            {
                var outrights = _affiliateService.GetSelectList(new GetOutRightCriteria
                {
                    SportIDList = criteria.SportIDList,
                    CountryCodeList = criteria.CountryCodeList
                });
                if (Utilities.IsExistInSelectListItem(criteria.OutRightIDList, outrights))
                {
                    criteria.OutRightIDList = "all";
                }
            }

            if (!string.IsNullOrEmpty(criteria.SportIDList))
            {
                var sports = _affiliateService.GetSelectList(new GetSupportedSportCriteria());
                if (Utilities.IsExistInSelectListItem(criteria.SportIDList, sports))
                {
                    criteria.SportIDList = "all";
                }
            }

            if (!string.IsNullOrEmpty(criteria.CountryCodeList))
            {
                var countries = _affiliateService.GetSelectList(new GetAllCountryCriteria());
                if (Utilities.IsExistInSelectListItem(criteria.CountryCodeList, countries))
                {
                    criteria.CountryCodeList = "all";
                }
            }

            if (!string.IsNullOrEmpty(criteria.BetTypeIDList))
            {
                var bettypes = _affiliateService.GetSelectList(new GetSupportedBettypeCriteria());
                if (Utilities.IsExistInSelectListItem(criteria.BetTypeIDList, bettypes))
                {
                    criteria.BetTypeIDList = "all";
                }
            }
        }

        /// <summary>
        /// Views the bag for flags.
        /// </summary>
        /// <param name="oddsTypeFlag">If set to <c>true</c> [odds type flag].</param>
        /// <param name="oddsStyleFlag">If set to <c>true</c> [odds style flag].</param>
        /// <param name="languagesFlag">If set to <c>true</c> [languages flag].</param>
        /// <param name="sportsFlag">If set to <c>true</c> [sports flag].</param>
        /// <param name="countriesFlag">If set to <c>true</c> [countries flag].</param>
        /// <param name="marketsFlag">If set to <c>true</c> [markets flag].</param>
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

        /// <summary>
        /// The sub method for adding an affiliate with account manager role.
        /// </summary>
        /// <param name="licenseeProfile">The profile of the licensee which new affiliate belongs to.</param>
        /// <param name="criteria">The affiliate's criteria.</param>
        private void AddAffiliateWithAccountManagerRole(DataSet licenseeProfile, ref InsertAffiliateCriteria criteria)
        {
            if (licenseeProfile != null && licenseeProfile.Tables.Count > 0 && licenseeProfile.Tables[0].Rows.Count > 0)
            {
                var row = licenseeProfile.Tables[0].Rows[0];

                criteria.OddsType = row["OddsType"].ToString();
                criteria.LanguageList = row["LanguageList"].ToString();
                criteria.SportIDList = row["SportIDList"].ToString();
                criteria.CountryCodeList = row["CountryCodeList"].ToString();
                criteria.LeagueIDList = row["LeagueIDList"].ToString();
                criteria.OutRightIDList = row["OutRightIDList"].ToString();
                criteria.BetTypeIDList = row["BetTypeIDList"].ToString();
                criteria.LeagueKeyword = row["LeagueKeyword"].ToString();
                criteria.LeagueKeyword_O = row["LeagueKeyword_O"].ToString();

                ViewBag.NumberOfIPs = int.Parse(row["NumberOfIPs"].ToString());

                ViewBagForFlags(true, true, true, true, true, true);

                ProfileForAddAffiliateWithAccountManagerRole(ref criteria);
            }
        }

        /// <summary>
        /// Profile for adding new affiliate with account manager role.
        /// </summary>
        /// <param name="criteria">The affiliate's criteria.</param>
        private void ProfileForAddAffiliateWithAccountManagerRole(ref InsertAffiliateCriteria criteria)
        {
            ViewBag.OddsTypeList = _affiliateService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "OddsType" }, false)
                                                                  .Where(item => item.Text != "Live" || (item.Text == "Live" && IsAccountManagerRole()));
            ViewBag.OddsStyleList = _affiliateService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "OddsRegion" }, false);
            ViewBag.LanguageIDList = _affiliateService.GetSelectList(new GetAllLanguageCriteria());
            ViewBag.DelayTimeList = GenerateDelayTime(_affiliateService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "Config" }, false));
            ////Sport
            var sports = _affiliateService.GetSelectList(new GetSupportedSportCriteria());
            ViewBag.SportList = UpdateSportListForCricket(sports);
            if (criteria.SportIDList.Equals("all"))
            {
                criteria.SportIDList = string.Join(",", sports.Select(item => item.Value));
            }
            ////Market
            var bettypes = _affiliateService.GetSelectList(new GetSupportedBettypeCriteria());
            ViewBag.BettypesList = bettypes;
            if (criteria.BetTypeIDList.Equals("all"))
            {
                criteria.BetTypeIDList = string.Join(",", bettypes.Select(item => item.Value));
            }
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

        /// <summary>
        /// Get affiliate's profile with account manager role.
        /// </summary>
        /// <param name="affiliateProfile">The affiliate's profile.</param>
        /// <param name="criteria">The affiliate's criteria.</param>
        private void GetAffiliateProfileWithAccountManageRole(DataSet affiliateProfile, ref UpdateProfileCriteria criteria)
        {
            if (affiliateProfile != null && affiliateProfile.Tables.Count > 0 && affiliateProfile.Tables[0].Rows.Count > 0)
            {
                var row = affiliateProfile.Tables[0].Rows[0];
                ////Delay Times
                var delayTimes = row["DelayTimes"].ToString().Split(',');
                criteria.DeadballDelayTimes = delayTimes.Length > 0 ? delayTimes[0] : "0";
                criteria.LiveDelayTimes = delayTimes.Length > 1 ? delayTimes[1] : "0";
                criteria.OutrightDelayTimes = delayTimes.Length > 2 ? delayTimes[2] : "0";

                criteria.NumberOfIPs = row["NumberOfIPs"].GetType() != typeof(DBNull) ? (short)row["NumberOfIPs"] : (short)0;
                criteria.LeagueKeyword = row["LeagueKeyword"].ToString();
                criteria.LeagueKeyword_O = row["LeagueKeyword_O"].ToString();
                criteria.LeagueIDList = row["LeagueIDList"].ToString();
                criteria.OutRightIDList = row["OutrightIDList"].ToString();
                ViewBag.ActualNumberOfIps = int.Parse(row["ActualNumberOfIps"].ToString());
            }

            var licenseeProfile = _affiliateService.GetDataSet(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = CurrentSession.SystemId });
            if (licenseeProfile != null && licenseeProfile.Tables.Count > 0 && licenseeProfile.Tables[0].Rows.Count > 0)
            {
                var row = licenseeProfile.Tables[0].Rows[0];
                ViewBag.NumberOfIPs = int.Parse(row["NumberOfIPs"].ToString());
                ViewBagForFlags(true, true, true, true, true, true);

                ProfileForEditAffiliateWithAccountManagerRole(ref criteria);
            }
        }

        /// <summary>
        /// Profile for editing an affiliate with account manager role.
        /// </summary>
        /// <param name="criteria">The affiliate's profile.</param>
        private void ProfileForEditAffiliateWithAccountManagerRole(ref UpdateProfileCriteria criteria)
        {
            ViewBag.OddsTypeList = _affiliateService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "OddsType" }, false);
            ViewBag.OddsStyleList = _affiliateService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "OddsRegion" }, false);
            ViewBag.LanguageIDList = _affiliateService.GetSelectList(new GetAllLanguageCriteria());
            var config = _affiliateService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "Config" }, false);
            ViewBag.DelayTimeList = GenerateDelayTime(config);
            ////Sport
            var sports = _affiliateService.GetSelectList(new GetSupportedSportCriteria());
            ViewBag.SportList = UpdateSportListForCricket(sports);
            if (criteria.SportIDList.Equals("all"))
            {
                criteria.SportIDList = string.Join(",", sports.Select(item => item.Value));
            }
            ////Market
            var bettypes = _affiliateService.GetSelectList(new GetSupportedBettypeCriteria());
            ViewBag.BettypesList = bettypes;
            if (criteria.BetTypeIDList.Equals("all"))
            {
                criteria.BetTypeIDList = string.Join(",", bettypes.Select(item => item.Value));
            }
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

        #endregion

        #region LicenseeAdmin/Sub-Acc

        /// <summary>
        /// The sub method for adding an affiliate with licensee admin role.
        /// </summary>
        /// <param name="licenseeProfile">The profile of the licensee which new affiliate belongs to.</param>
        private void AddAffiliateWithLicenseeAdminRole(DataSet licenseeProfile)
        {
            if (licenseeProfile != null && licenseeProfile.Tables.Count > 0 && licenseeProfile.Tables[0].Rows.Count > 0)
            {
                var row = licenseeProfile.Tables[0].Rows[0];
                var oddStyles = row["OddsType"].ToString();
                ViewBag.NumberOfIPs = int.Parse(row["NumberOfIPs"].ToString());
                ViewBagForFlags(
                                        bool.Parse(row["OddsTypeFlag"].ToString()),
                                        bool.Parse(row["OddsRegionFlag"].ToString()),
                                        bool.Parse(row["LanguageListFlag"].ToString()),
                                        bool.Parse(row["SportIDListFlag"].ToString()),
                                        bool.Parse(row["CountryCodeListFlag"].ToString()),
                                        bool.Parse(row["BettypeIDListFlag"].ToString()) && (oddStyles != "8" && oddStyles != string.Empty));

                ProfileForAddAffiliateWithLicenseeAdminAndSubAccountRole();
            }
        }

        /// <summary>
        /// Get affiliate's profile with licensee admin role.
        /// </summary>
        /// <param name="criteria">The affiliate's profile.</param>
        private void GetAffiliateProfileWithLicenseeAdminRole(ref UpdateProfileCriteria criteria)
        {
            var licenseeProfile = _affiliateService.GetDataSet(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = CurrentSession.SystemId });
            if (licenseeProfile != null && licenseeProfile.Tables.Count > 0 && licenseeProfile.Tables[0].Rows.Count > 0)
            {
                var row = licenseeProfile.Tables[0].Rows[0];
                var oddStyles = row["OddsType"].ToString();
                ViewBag.NumberOfIPs = int.Parse(row["NumberOfIPs"].ToString());
                ViewBag.OddsTypeFlag = bool.Parse(row["OddsTypeFlag"].ToString());
                ViewBag.OddsStyleFlag = bool.Parse(row["OddsRegionFlag"].ToString());
                ViewBag.LanguagesFlag = bool.Parse(row["LanguageListFlag"].ToString());
                ViewBag.SportsFlag = bool.Parse(row["SportIDListFlag"].ToString());
                ViewBag.CountriesFlag = bool.Parse(row["CountryCodeListFlag"].ToString());
                ViewBag.MarketsFlag = bool.Parse(row["BettypeIDListFlag"].ToString()) && (oddStyles != "8" && oddStyles != string.Empty);

                ProfileForEditAffiliateWithLicenseeAdminAndSubAccountRole(ref criteria);
            }
        }

        /// <summary>
        /// The sub method for adding an affiliate with sub account role.
        /// </summary>
        private void AddAffiliateWithSubAccountRole()
        {
            var userProfile = _affiliateService.GetDataSet(new UserGetPermissionCriteria
            {
                SystemID = CurrentSession.SystemId,
                UserID = CurrentSession.UserId
            });

            if (userProfile != null && userProfile.Tables.Count > 0 && userProfile.Tables[0].Rows.Count > 0)
            {
                var row = userProfile.Tables[0].Rows[0];
                ViewBag.NumberOfIPs = int.Parse(row["NumberOfIPs"].ToString());
                ViewBagForFlags(
                                       bool.Parse(row["OddsTypeFlag"].ToString()),
                                       bool.Parse(row["OddsRegionFlag"].ToString()),
                                       bool.Parse(row["LanguageListFlag"].ToString()),
                                       bool.Parse(row["SportIDListFlag"].ToString()),
                                       bool.Parse(row["CountryCodeListFlag"].ToString()),
                                       bool.Parse(row["BettypeIDListFlag"].ToString()));

                ProfileForAddAffiliateWithLicenseeAdminAndSubAccountRole();
            }
        }

        /// <summary>
        /// Get affiliate's profile with sub account role.
        /// </summary>
        /// <param name="criteria">The affiliate's profile.</param>
        private void GetAffiliateProfileWithSubAccountRole(ref UpdateProfileCriteria criteria)
        {
            var userProfile = _affiliateService.GetDataSet(new UserGetPermissionCriteria
            {
                SystemID = CurrentSession.SystemId,
                UserID = CurrentSession.UserId
            });

            if (userProfile != null && userProfile.Tables.Count > 0 && userProfile.Tables[0].Rows.Count > 0)
            {
                var row = userProfile.Tables[0].Rows[0];
                ViewBag.NumberOfIPs = int.Parse(row["NumberOfIPs"].ToString());
                ViewBag.OddsTypeFlag = bool.Parse(row["OddsTypeFlag"].ToString());
                ViewBag.OddsStyleFlag = bool.Parse(row["OddsRegionFlag"].ToString());
                ViewBag.LanguagesFlag = bool.Parse(row["LanguageListFlag"].ToString());
                ViewBag.SportsFlag = bool.Parse(row["SportIDListFlag"].ToString());
                ViewBag.CountriesFlag = bool.Parse(row["CountryCodeListFlag"].ToString());
                ViewBag.MarketsFlag = bool.Parse(row["BettypeIDListFlag"].ToString());

                ProfileForEditAffiliateWithLicenseeAdminAndSubAccountRole(ref criteria);
            }
        }

        /// <summary>
        /// Profile for adding an affiliate with licensee admin and sub account role.
        /// </summary>
        private void ProfileForAddAffiliateWithLicenseeAdminAndSubAccountRole()
        {
            ////OddsType
            ViewBag.OddsTypeList = _affiliateService.GetSelectList(
                                                    new GetTypeSettingCriteria { SystemID = CurrentSession.SystemId, Type = "OddsType" },
                                                    false)
                                            .Where(item => item.Text != "Live" || (item.Text == "Live" && IsAccountManagerRole()));

            ////OddsStyle                    
            ViewBag.OddsStyleList = _affiliateService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "OddsRegion" }, false);

            ////Language
            ViewBag.LanguageIDList = _affiliateService.GetLanguageBySystemId();

            ////Market                    
            ViewBag.BettypesList = _affiliateService.GetBettypeBySystemId();

            ////Country                    
            ViewBag.CountriesList = _affiliateService.GetCountryBySystemId();

            ////Sport                    
            ViewBag.SportList = UpdateSportListForCricket(_affiliateService.GetSportBySystemId());
        }

        /// <summary>
        /// Profile for editing an affiliate with licensee admin and sub account role.
        /// </summary>
        /// <param name="criteria">The affiliate's profile.</param>
        private void ProfileForEditAffiliateWithLicenseeAdminAndSubAccountRole(ref UpdateProfileCriteria criteria)
        {
            ////OddsType
            ViewBag.OddsTypeList = _affiliateService
                .GetSelectList(new GetTypeSettingCriteria { SystemID = CurrentSession.SystemId, Type = "OddsType" }, false)
                .Where(item => item.Text != "Live" || (item.Text == "Live" && IsAccountManagerRole()));

            ////OddsStyle                    
            ViewBag.OddsStyleList = _affiliateService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "OddsRegion" }, false);

            ////Language
            ViewBag.LanguageIDList = _affiliateService.GetLanguageBySystemId();

            ////Sport
            var sports = _affiliateService.GetSportBySystemId();
            ViewBag.SportList = UpdateSportListForCricket(sports);
            if (criteria.SportIDList.Equals("all"))
            {
                criteria.SportIDList = string.Join(",", sports.Select(item => item.Value));
            }

            ////Market
            var bettypes = _affiliateService.GetBettypeBySystemId();
            ViewBag.BettypesList = bettypes;
            if (criteria.BetTypeIDList.Equals("all"))
            {
                criteria.BetTypeIDList = string.Join(",", bettypes.Select(item => item.Value));
            }

            ////Country
            var countries = _affiliateService.GetCountryBySystemId();
            ViewBag.CountriesList = countries;
            if (criteria.CountryCodeList.Equals("all"))
            {
                criteria.CountryCodeList = string.Join(",", countries.Select(item => item.Value));
            }
        }

        private IEnumerable<SelectListItem> UpdateSportListForCricket(IEnumerable<SelectListItem> sports)
        {
            return sports.Where(s => s.Value != "50");
        }

        #endregion

        #endregion
    }
}