// --------------------------------------------------------------------------
// <copyright file="SubAccountController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.02.05">Create</change>
// </history>
namespace Fanex.OddsFeed.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Fanex.Common;
    using Fanex.OddsFeed.Core.Business;
    using Fanex.OddsFeed.Core.Criteria.Administrator;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Criteria.Licensee;
    using Fanex.OddsFeed.Core.Criteria.SubAccount;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Service.Impl;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Core.Web.Attribute;

    /// <summary>
    /// Class SubAccountController.
    /// </summary>
    [IsAuthorize]
    [IsUnderMaintainance]    
    public class SubAccountController : BaseController
    {
        #region Members

        /// <summary>
        /// The _sub account service.
        /// </summary>
        private readonly ISubAccService _subAccService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SubAccountController"/> class.
        /// </summary>
        public SubAccountController()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubAccountController"/> class.
        /// </summary>
        /// <param name="subAccService">The sub account service.</param>
        public SubAccountController(ISubAccService subAccService = null)
        {
            _subAccService = subAccService ?? new SubAccService { ServiceContext = ServiceContext };
        }

        #endregion

        #region Add

        [HttpGet]
        public ActionResult Add()
        {
            if (!this.HasEditPermission(CurrentSession.UserRole))
            {
                return this.NoAuthorization();
            }

            ViewBag.SystemList = _subAccService.GetSelectList(new GetSystemByUserIDCriteria() { UserID = CurrentSession.UserId, IgnoreIsActive = IsAccountManagerRole() });
            ViewBagFlags();
            return View();
        }

        /// <summary>
        /// Adds a sub account.
        /// </summary>
        /// <param name="criteria">The sub account's criteria.</param>
        /// <returns>Represents a JSON-formatted content for adding.</returns>
        [HttpPost]
        public JsonResult Add(InsertSubAccCriteria criteria)
        {
            var actionResult = new OddsFeedActionResult();
            criteria.SystemID = CurrentSession.SystemId;
            criteria.Creator = CurrentSession.UserId;
            criteria.RoleID = 3;
            criteria.Password = Cryptography.CfsEnCode(HttpUtility.HtmlEncode(criteria.Password.Trim()));
            criteria.FirstName = criteria.FirstName ?? string.Empty;
            criteria.LastName = criteria.LastName ?? string.Empty;

            var result = _subAccService.Insert(criteria);
            if (result != null && result.IsSuccessful)
            {
                actionResult.IsSuccess = true;
                actionResult.ResultMessage = ViewResources.ActionResult_AddSubAccountSuccess;
            }
            else
            {
                actionResult.IsSuccess = false;
                actionResult.ResultMessage = result.Message;
            }

            ViewBag.SystemList = _subAccService.GetSelectList(new GetSystemCriteria());
            return Json(actionResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Checks the exist sub account.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Represents a JSON-formatted content for checking.</returns>
        [HttpPost]
        public JsonResult CheckExistSubAccount(InsertSubAccCriteria criteria)
        {
            var checkResult = new OddsFeedActionResult();
            var res = this._subAccService.GetSubAccountListReturnDataCollection(new CheckExistSubAccountCriteria
            {
                Username = string.IsNullOrEmpty(criteria.UserName) ? string.Empty : criteria.UserName,
                PageIndex = 1,
                PageSize = 1,                
                RoleId = 3
            });

            if (res.Data.Tables.Count > 0 &&
                res.Data.Tables[0].Rows.Count > 0)
            {
                checkResult.IsSuccess = false;
            }

            return Json(checkResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get

        [HttpGet]
        public ActionResult List()
        {
            if (!this.HasEditPermission(CurrentSession.UserRole))
            {
                return this.NoAuthorization();
            }

            return View("SubAccList");
        }

        /// <summary>
        /// Gets the sub account list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Represents a JSON-formatted content.</returns>
        public JsonResult GetSubAccountList(GetSubAccByNameCriteria criteria)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            criteria.SystemId = CurrentSession.SystemId;
            criteria.Username = (criteria.Username == null || criteria.Username == "Username") ? string.Empty : criteria.Username.Trim();
            criteria.RoleId = 3;
            criteria.PageIndex++;
            var dr = _subAccService.GetSubAccountListReturnDataCollection(criteria);
            var result = FanexGridHelper.DataForFanexGrid_PreventContainQuote(dr);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// List existed sub account.
        /// </summary>
        /// <param name="username">The user name for searching.</param>
        /// <returns>The sub account list.</returns>
        public string ListExistedSubAccount(string username)
        {
            var criteria = new GetTop10SubAccCriteria
            {
                SystemId = CurrentSession.SystemId,
                RoleId = 3,
                Username = username
            };

            var top10AffList = _subAccService.GetTop10SubAccountReturnDataSet(criteria).Tables[0];
            return DataForAutoComplete(top10AffList, "username");
        }

        #endregion

        #region Edit

        /// <summary>
        /// Edits the sub account information.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns>Action Result.</returns>
        [HttpGet]
        public ActionResult EditSubAccInfo(int id, string userName)
        {
            if (!this.HasEditPermission(CurrentSession.UserRole))
            {
                return this.NoAuthorization();
            }

            if (id == 0)
            {
                return RedirectToAction("List", "SubAccount");
            }

            var criteria = new UpdateSubAccCriteria
            {
                SubAccountID = id,
                UserName = userName
            };

            var rawData = _subAccService.GetBySystemId(CurrentSession.SystemId);
            var rawInfo = rawData.ToList().Where(user => user.Id == id).FirstOrDefault();
            if (rawInfo != null)
            {
                ViewBag.CurrentUser = " of \"" + userName + "\"";
                criteria.FirstName = rawInfo.FirstName;
                criteria.LastName = rawInfo.LastName;
                criteria.Email = rawInfo.Email;
                criteria.IsActive = rawInfo.IsActive;
            }

            return View(criteria);
        }

        /// <summary>
        /// Edits the sub account information.
        /// </summary>
        /// <param name="criteria">The sub account's criteria.</param>
        /// <returns>Represents a JSON-formatted content of editing.</returns>
        [HttpPost]
        public JsonResult EditSubAccInfo(UpdateSubAccCriteria criteria)
        {
            var actionResult = new OddsFeedActionResult();
            if (ModelState.IsValid)
            {
                criteria.FirstName = criteria.FirstName ?? string.Empty;
                criteria.LastName = criteria.LastName ?? string.Empty;

                var result = _subAccService.Update(criteria);
                if (result == null || result.IsSuccessful == false)
                {
                    actionResult.IsSuccess = false;
                    if (result != null)
                    {
                        actionResult.ResultMessage = result.Message;
                    }
                }        
            }

            return Json(actionResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Edits the active.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="isActive">The user's active.</param>
        /// <returns>Return "Successfully!" if success else return error message. </returns>
        public string EditActive(int userId, bool isActive)
        {
            var criteria = new UpdateActiveSubAccCriteria
            {
                SubAccountID = userId,
                IsActive = isActive
            };

            var result = _subAccService.Update(criteria);

            if (result != null && result.IsSuccessful)
            {
                return "Successfully!";
            }
            else
            {
                return result.Message;
            }
        }

        /// <summary>
        /// Display the password reset page.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns>The password reset page.</returns>
        [HttpGet]
        public ActionResult ResetPassword(int id, string userName)
        {
            if (!this.HasEditPermission(CurrentSession.UserRole))
            {
                return this.NoAuthorization();
            }

            if (id == 0)
            {
                return RedirectToAction("List", "SubAccount");
            }

            ViewBag.CurrentUser = " of \"" + userName + "\"";
            var criteria = new ResetPassSubAccCriteria
            {
                SubAccountID = id
            };

            return View(criteria);
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="criteria">The user's criteria.</param>
        /// <returns>Represents a JSON-formatted content of updating.</returns>
        [HttpPost]
        public JsonResult ResetPassword(ResetPassSubAccCriteria criteria)
        {
            var actionResult = new OddsFeedActionResult();
            if (ModelState.IsValid)
            {
                if (criteria.Password != criteria.ConfirmPassword)
                {
                    actionResult.IsSuccess = false;
                    actionResult.ResultMessage = ViewResources.Error_PasswordNotMatch;
                }
                else
                {
                    criteria.Password = Cryptography.CfsEnCode(HttpUtility.HtmlEncode(criteria.Password.Trim()));
                    var result = _subAccService.Update(criteria);

                    if (result != null && result.IsSuccessful)
                    {
                        actionResult.IsSuccess = true;
                        actionResult.ResultMessage = ViewResources.ActionResult_UpdatePasswordSuccess;
                    }
                    else
                    {
                        actionResult.IsSuccess = false;
                        actionResult.ResultMessage = result.Message;
                    }
                }
            }

            return Json(actionResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Edits the permission.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns>The page for editing the permission.</returns>
        [HttpGet]
        public ActionResult EditPermission(int id, string userName)
        {
            if (!this.HasEditPermission(CurrentSession.UserRole))
            {
                return this.NoAuthorization();
            }

            if (id == 0)
            {
                return RedirectToAction("List", "SubAccount");
            }

            ViewBag.CurrentUser = " of \"" + userName + "\"";
            byte systemId = CurrentSession.SystemId;
            
            var result = _subAccService.GetUserPermission(id, systemId) as PermissionFlag;
            if (result == null)
            {
                return View();
            }

            var criteria = new EditPermissionSubAccCriteria()
            {
                SubAccountID = id,
                SubAccountName = userName,
                OddsTypeFlag = result.OddsTypeFlag,
                OddsStyleFlag = result.OddsRegionFlag,
                LanguageListFlag = result.LanguageListFlag,
                SportIDListFlag = result.SportIDListFlag,
                CountryCodeListFlag = result.CountryCodeListFlag,
                BetTypeIDListFlag = result.BetTypeIDListFlag
            };

            ViewBagFlags();

            return View(criteria);
        }

        /// <summary>
        /// Edits the permission.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult EditPermission(EditPermissionSubAccCriteria criteria)
        {
            if (ModelState.IsValid)
            {
                criteria.UpdatedBy = CurrentSession.UserId;
                var result = _subAccService.Update(criteria);
                if (result != null && result.IsSuccessful)
                {
                    ViewBag.Success = true;
                }
                else
                {
                    ViewBag.Error = result.Message;
                }
            }

            ViewBagFlags();

            ViewBag.CurrentUser = " of \"" + criteria.SubAccountName + "\"";
            return View(criteria);
        }
        #endregion

        #region Del

        /// <summary>
        /// Deletes the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Return "Successfully!" if success else return error message.</returns>
        public string Delete(int userId)
        {
            var criteria = new DeleteSubAccCriteria
            {
                SubAccountID = userId
            };

            var result = _subAccService.Delete(criteria);
            if (result != null && result.IsSuccessful)
            {
                return "Successfully!";
            }
            else
            {
                return result.Message;
            }
        }

        #endregion

        #region AssignLicensee

        /// <summary>
        /// Assigns the licensee.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="username">The username.</param>
        /// <returns>Action Result.</returns>
        [HttpGet]
        public ActionResult AssignLicensee(int id, string username)
        {
            if (id == 0)
            {
                return RedirectToAction("List", "SubAccount");
            }

            if (!this.HasEditPermission(CurrentSession.UserRole))
            {
                return this.NoAuthorization();
            }

            ViewBag.CurrentUser = " \"" + username + "\"";

            ////System
            IEnumerable<SelectListItem> systemList = new List<SelectListItem>();
            systemList = _subAccService.GetSelectList(new GetOptionalSystemBySubAccountIDCriteria() { UserID = CurrentSession.UserId, SubAccountID = id });
            ViewBag.SystemListID = systemList;

            var criteria = new AssignLicensee_SubAccCriteria
            {
                UserID = id,
                SystemList = _subAccService.GetSelectList(new GetSystemByUserIDCriteria() { UserID = id, IgnoreIsActive = false }).Select(item => item.Value).ToArray()
            };

            if (!this.HasEditPermission(CurrentSession.UserRole))
            {
                ViewBag.HasEditPermission = false;
                ViewBag.Error = ViewResources.You_have_no_authorization;
            }

            return View(criteria);
        }

        /// <summary>
        /// Assigns the licensee.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult AssignLicensee(AssignLicensee_SubAccCriteria criteria)
        {
            criteria.Creator = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;

            if (!this.HasEditPermission(CurrentSession.UserRole))
            {
                ViewBag.Success = false;
                ViewBag.Error = ViewResources.You_have_no_authorization;
                return View(criteria);
            }

            ////System
            IEnumerable<SelectListItem> systemList = new List<SelectListItem>();            
            systemList = _subAccService.GetSelectList(new GetOptionalSystemBySubAccountIDCriteria() { UserID = CurrentSession.UserId, SubAccountID = criteria.UserID });
            ViewBag.SystemListID = systemList;

            if (string.IsNullOrEmpty(criteria.ListSystemId))
            {
                ViewBag.Success = false;
                ViewBag.Error = ViewResources.Validate_Licensee_Require;
                return View(criteria);
            }

            var result = this._subAccService.AssignLicensee(criteria);
            if (result != null && result.IsSuccessful)
            {
                ViewBag.Success = true;
            }

            ViewBag.CurrentUser = " \"" + criteria.UserName + "\"";
            return View(criteria);
        }
        
        private bool HasEditPermission(int userRole)
        {
            return !(userRole == (int)Constant.UserRole.LicenseeSubAccount);
        }

        #endregion AssignLicensee

        #region Private methods

        /// <summary>
        /// Views the bag flags.
        /// </summary>
        private void ViewBagFlags()
        {
            var flags = _subAccService.GetDataSet(new GetLicenseeProfileByLicenseeIDCriteria() { SystemID = CurrentSession.SystemId });
            if (flags != null && flags.Tables.Count > 0 && flags.Tables[0].Rows.Count > 0)
            {
                var oddStyles = flags.Tables[0].Rows[0]["OddsType"].ToString();

                ViewBag.OddsTypeFlag = bool.Parse(flags.Tables[0].Rows[0]["OddsTypeFlag"].ToString());
                ViewBag.OddsStyleFlag = bool.Parse(flags.Tables[0].Rows[0]["OddsRegionFlag"].ToString());
                ViewBag.LanguagesFlag = bool.Parse(flags.Tables[0].Rows[0]["LanguageListFlag"].ToString());
                ViewBag.SportsFlag = bool.Parse(flags.Tables[0].Rows[0]["SportIDListFlag"].ToString());
                ViewBag.CountriesFlag = bool.Parse(flags.Tables[0].Rows[0]["CountryCodeListFlag"].ToString());
                ViewBag.MarketsFlag = bool.Parse(flags.Tables[0].Rows[0]["BettypeIDListFlag"].ToString())
                                                                && (oddStyles != "8" && oddStyles != string.Empty);
            }
        }

        #endregion
    }
}
