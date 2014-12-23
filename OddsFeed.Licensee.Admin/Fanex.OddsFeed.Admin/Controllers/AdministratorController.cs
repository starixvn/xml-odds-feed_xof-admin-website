// --------------------------------------------------------------------------
// <copyright file="AdministratorController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013.03.05">Created</change>
// </history>

namespace Fanex.OddsFeed.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.Logging;
    using Fanex.OddsFeed.Core.Business;
    using Fanex.OddsFeed.Core.Criteria.Administrator;
    using Fanex.OddsFeed.Core.Criteria.Licensee;
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Service.Impl;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Core.Web.Attribute;

    /// <summary>
    /// Administrator controller class.
    /// </summary>
    [IsAuthorize]
    [IsUnderMaintainance]
    public class AdministratorController : BaseController
    {
        #region Members

        private readonly IAdministratorService _adminService;

        private readonly IUserService _userService;

        #endregion

        #region Constructors

        public AdministratorController()
            : this(null, null)
        {
        }

        public AdministratorController(IAdministratorService adminService = null, IUserService userService = null)
        {
            _adminService = adminService ?? new AdministratorService { ServiceContext = ServiceContext };
            _userService = userService ?? new UserService { ServiceContext = ServiceContext };
        }

        #endregion

        #region Add New Administrator

        //// GET: /Administrator/

        /// <summary>
        /// Display the adding administrator page.
        /// </summary>
        /// <returns>The new administrator page.</returns>
        [HttpGet]
        public ActionResult AdministratorAdd()
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            ////Sport Flag
            var flags = _adminService.GetDataSet(new GetLicenseeProfileByLicenseeIDCriteria() { SystemID = CurrentSession.SystemId });
            if (flags != null && flags.Tables.Count > 0 && flags.Tables[0].Rows.Count > 0)
            {
                ViewBag.SportsFlag = bool.Parse(flags.Tables[0].Rows[0]["SportIDListFlag"].ToString());
            }

            ////System
            ViewBag.SystemList = _adminService.GetSelectList(new GetAllSystemsCriteria());
            return View();
        }

        /// <summary>
        /// Adding new administrator.
        /// </summary>
        /// <param name="criteria">The criteria for adding.</param>
        /// <returns>Return "Successfully" if success else return an error message.</returns>
        [HttpPost]
        public JsonResult AddNew(InsertAdminCriteria criteria)
        {
            var addResult = string.Empty;

            criteria.FirstName = criteria.FirstName ?? string.Empty;
            criteria.LastName = criteria.LastName ?? string.Empty;
            criteria.Password = criteria.Password.Trim();
            //// hash password incase of add new
            if (!criteria.IsImport)
            {
                criteria.Password = Cryptography.CfsEnCode(HttpUtility.HtmlEncode(criteria.Password));
            }

            criteria.Creator = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;
            var result = _adminService.InsertAdmin(criteria);
            if (result != null && result.IsSuccessful)
            {
                ViewBag.Success = true;
                addResult = "Successfully!";
            }
            else
            {
                addResult = result.Message;
            }

            return Json(addResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Check exist an administrator.
        /// </summary>
        /// <param name="criteria">The criteria for checking.</param>
        /// <returns>Represents a JSON-formatted content for checking.</returns>
        [HttpPost]
        public JsonResult CheckExistAdministrator(InsertAdminCriteria criteria)
        {
            var checkResult = new OddsFeedActionResult();
            var res = this._adminService.GetAdminListReturnDataCollection(new CheckExistAdministratorCriteria
            {
                Username = criteria.UserName,
                PageIndex = 1,
                PageSize = 1,
                UserID = 0,
                SystemID = 0
            });

            if (res.Data.Tables.Count > 0 &&
                res.Data.Tables[0].Rows.Count > 0)
            {
                checkResult.IsSuccess = false;
            }

            return Json(checkResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Administrator List

        /// <summary>
        /// Show administrator list.
        /// </summary>
        /// <returns>The administrator list page.</returns>
        public ActionResult AdministratorList()
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            ////System
            var systemList = (List<SelectListItem>)_adminService.GetSelectList("Odds_System_SelAll");
            systemList.Insert(0, new SelectListItem { Selected = true, Text = "All", Value = "0" });
            ViewBag.SystemList = systemList;

            ////Role
            var roleList = (List<SelectListItem>)_adminService.GetSelectList("Odds_Role_Sel");
            roleList.Insert(0, new SelectListItem { Selected = true, Text = "All", Value = "0" });
            ViewBag.RoleList = roleList;
            return View("~/Views/Administrator/AdministratorList.cshtml");
        }

        [HttpPost]
        public JsonResult GetListAdminBySystemAndNameAndRole(GetAdminCriteria criteria)
        {
            criteria.PageIndex++;
            criteria.Username = (criteria.Username == null || criteria.Username == "Username") ?
                string.Empty :
                criteria.Username.Trim();
            var dr = _adminService.GetAdminListReturnDataCollection(criteria);
            var result = FanexGridHelper.DataForFanexGrid_PreventContainQuote(dr);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update Active status
        /// <summary>
        /// Update active status of an administrator.
        /// </summary>
        /// <param name="adminid">The administrator's id.</param>
        /// <param name="isactive">The administrator's active status.</param>
        /// <returns>Return "Successfully" if success else the error message.</returns>
        [HttpPost]
        public string UpdateActive(int adminid, bool isactive)
        {
            var criteria = new DeleteAdminCriteria
            {
                AdminID = adminid,
                Isactive = isactive
            };

            var result = _adminService.UpdateAdmin(criteria);

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

        #region Update administrator's profile information

        /// <summary>
        /// Edit the administrator's profile.
        /// </summary>
        /// <param name="id">The administrator's id.</param>
        /// <param name="username">The administrator's name.</param>
        /// <returns>Return the administrator list page if id = 0 else return the administrator's profile page.</returns>
        [HttpGet]
        public ActionResult AdministratorEditProfile(int id, string username)
        {
            if (id == 0)
            {
                return RedirectToAction("AdministratorList", "Administrator");
            }

            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            var userInfo = _userService.GetUserInfo(new GetUserInfoCriteria { UserId = id }).FirstOrDefault();

            ViewBag.CurrentUser = " of \"" + username + "\"";
            ViewBag.CurrentUserId = CurrentSession.UserId;
            var criteria = new UpdateAdminInfoCriteria { UserID = id, Username = username };

            if (userInfo != null)
            {
                criteria.FirstName = userInfo.FirstName;
                criteria.LastName = userInfo.LastName;
                criteria.Email = userInfo.Email;
                criteria.RoleID = userInfo.RoleID;

                ViewBag.HasEditPermission = this.HasEditPermission(CurrentSession.UserRole, criteria.UserID);
                if (!ViewBag.HasEditPermission)
                {
                    ViewBag.Error = ViewResources.You_have_no_authorization;
                }
            }

            return View(criteria);
        }

        /// <summary>
        /// Edit the administrator's profile.
        /// </summary>
        /// <param name="criteria">The administrator's profile.</param>
        /// <returns>Return a java script object notation for editing.</returns>
        [HttpPost]
        public JsonResult AdministratorEditProfile(UpdateAdminInfoCriteria criteria)
        {
            var actionResult = new OddsFeedActionResult();
            criteria.Creator = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;
            criteria.FirstName = criteria.FirstName ?? string.Empty;
            criteria.LastName = criteria.LastName ?? string.Empty;

            if (!this.HasEditPermission(CurrentSession.UserRole, criteria.UserID))
            {
                actionResult.IsSuccess = false;
                actionResult.ResultMessage = ViewResources.You_can_not_update_this_administrator;
                return Json(actionResult, JsonRequestBehavior.AllowGet);
            }

            var result = _adminService.UpdateAdminInfo(criteria);
            if (result == null || result.IsSuccessful == false)
            {
                actionResult.IsSuccess = false;
                if (result != null)
                {
                    actionResult.ResultMessage = result.Message;
                }
            }

            if (CurrentSession.UserId == criteria.UserID)
            {
                CurrentSession.UserRole = (byte)criteria.RoleID;
            }

            return Json(actionResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update administrator's system info
        /// <summary>
        /// Edit administrator's system.
        /// </summary>
        /// <param name="id">The admin's id.</param>
        /// <param name="username">The admin's username.</param>
        /// <param name="role">The admin's role.</param>
        /// <returns>Return the administrator list page if id = 0 else return the administrator's system page.</returns>
        [HttpGet]
        public ActionResult AdministratorEditSystem(int id, string username, int role = 0)
        {
            if (id == 0)
            {
                return RedirectToAction("AdministratorList", "Administrator");
            }

            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            ViewBag.CurrentUser = " \"" + username + "\"";
            ////System
            IEnumerable<SelectListItem> systemList = new List<SelectListItem>();
            if (role == (int)Constant.UserRole.Licensee)
            {
                systemList = _adminService.GetSelectList(new GetSystemByUserIDCriteria() { UserID = CurrentSession.UserId, IgnoreIsActive = true });
            }
            else
            {
                systemList = _adminService.GetSelectList(new GetOptionalSystemBySubAccountIDCriteria() { UserID = CurrentSession.UserId, SubAccountID = id });
            }

            ViewBag.SystemListID = systemList;

            var criteria = new UpdateAdminSystemCriteria
            {
                UserID = id,
                UserName = username,
                SystemList = _adminService.GetSelectList(new GetSystemByUserIDCriteria() { UserID = id, IgnoreIsActive = false }).Select(item => item.Value).ToArray()
            };

            ViewBag.HasEditPermission = this.HasEditPermission(CurrentSession.UserRole, criteria.UserID);
            if (!ViewBag.HasEditPermission)
            {
                ViewBag.Error = ViewResources.You_have_no_authorization;
            }

            return View(criteria);
        }

        /// <summary>
        /// Edit administrator's system.
        /// </summary>
        /// <param name="criteria">The admin's system.</param>
        /// <returns>Return a java script object notation for editing.</returns>
        [HttpPost]
        public ActionResult AdministratorEditSystem(UpdateAdminSystemCriteria criteria)
        {
            criteria.Creator = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;

            if (!this.HasEditPermission(CurrentSession.UserRole, criteria.UserID))
            {
                ViewBag.Success = false;
                ViewBag.Error = ViewResources.You_can_not_update_this_administrator;
                return View(criteria);
            }

            ////System
            IEnumerable<SelectListItem> systemList = new List<SelectListItem>();
            if (IsLicenseeAdminRole())
            {
                systemList = _adminService.GetSelectList(new GetSystemByUserIDCriteria() { UserID = CurrentSession.UserId, IgnoreIsActive = true });
            }
            else
            {
                systemList = _adminService.GetSelectList(new GetOptionalSystemBySubAccountIDCriteria() { UserID = CurrentSession.UserId, SubAccountID = criteria.UserID });
            }

            if (string.IsNullOrEmpty(criteria.ListSystemId))
            {
                ViewBag.SystemListID = systemList;
                ViewBag.Success = false;
                ViewBag.Error = ViewResources.Validate_Licensee_Require;
                return View(criteria);
            }

            var result = _adminService.UpdateAdminSystem(criteria);

            if (result != null && result.IsSuccessful)
            {
                ViewBag.Success = true;
            }

            return CustomizeAfterEditSystem(criteria, systemList);
        }
        #endregion

        #region import existed admin

        /// <summary>
        /// List existed admins.
        /// </summary>
        /// <param name="username">The admin's name to search.</param>
        /// <returns>List of admins match with the username.</returns>
        [HttpPost]
        public string ListExistedAdmin(string username)
        {
            var criteria = new GetTop10AdminCriteria
            {
                Username = username
            };

            var top10AdminDc = _adminService.GetTop10AdminReturnDataSet(criteria).Tables[0];
            return DataForAutoComplete(top10AdminDc, "username");
        }

        /*
        public string DataForAutoComplete(DataSet dc)
        {
            StringBuilder builder = new StringBuilder();
            //builder.Append("[");

            foreach (DataRow row in dc.Tables[0].Rows)
            {
                //builder.Append("\"");
                builder.Append(row["username"].ToString());
                //builder.Append("\"");
                builder.Append(",");
            }
            builder.Remove(builder.Length - 1, 1);
            //builder.Append("]");

            return builder.ToString();
        }
        */

        /// <summary>
        /// Get existed admin.
        /// </summary>
        /// <param name="username">The admin's name to search.</param>
        /// <returns>The admins match with the username.</returns>
        [HttpPost]
        public string GetExistedAdmin(string username)
        {
            var licenseeProfile = _adminService.GetDataSet(new GetLicenseeProfileByLicenseeIDCriteria() { SystemID = CurrentSession.SystemId });
            if (licenseeProfile != null && licenseeProfile.Tables.Count > 0 && licenseeProfile.Tables[0].Rows.Count > 0)
            {
                ViewBag.SportsFlag = bool.Parse(licenseeProfile.Tables[0].Rows[0]["SportIDListFlag"].ToString());
            }

            var criteria = new GetExistedAdminCriteria
            {
                Username = username
            };
            var result = _adminService.GetExistedAdminDataSet(criteria);

            if (result.Tables.Count != 0)
            {
                string resultStr = result.Tables[0].Rows[0]["username"].ToString();
                resultStr += "," + result.Tables[0].Rows[0]["firstname"].ToString();
                resultStr += "," + result.Tables[0].Rows[0]["lastname"].ToString();
                resultStr += "," + result.Tables[0].Rows[0]["email"].ToString();
                resultStr += "," + result.Tables[0].Rows[0]["password"].ToString();
                resultStr += "," + result.Tables[0].Rows[0]["roleid"].ToString();

                ViewBag.Success = true;
                return resultStr;
            }
            else
            {
                ViewBag.Success = false;
                return ViewResources.User_is_not_existed;
            }
        }
        #endregion

        #region reset administrator's password
        /// <summary>
        /// Reset admin's password.
        /// </summary>
        /// <param name="id">The admin's id.</param>
        /// <param name="username">The admin's username.</param>
        /// <returns>Return the administrator list page if id = 0 else return the administrator's reset password page.</returns>
        [HttpGet]
        public ActionResult AdministratorResetPwd(int id, string username)
        {
            if (id == 0)
            {
                return RedirectToAction("AdministratorList", "Administrator");
            }

            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            ViewBag.CurrentUser = " of \"" + username + "\"";

            var criteria = new UpdateAdminPasswordCriteria
            {
                UserID = id
            };

            ViewBag.HasEditPermission = this.HasEditPermission(CurrentSession.UserRole, criteria.UserID);
            if (!ViewBag.HasEditPermission)
            {
                ViewBag.Error = ViewResources.You_have_no_authorization;
            }

            return View(criteria);
        }

        /// <summary>
        /// Reset admin's password.
        /// </summary>
        /// <param name="criteria">The admin's criteria.</param>
        /// <returns>Return a java script object notation for resetting.</returns>
        [HttpPost]
        public JsonResult AdministratorResetPwd(UpdateAdminPasswordCriteria criteria)
        {
            var actionResult = new OddsFeedActionResult();
            if (!this.HasEditPermission(CurrentSession.UserRole, criteria.UserID))
            {
                actionResult.IsSuccess = false;
                actionResult.ResultMessage = ViewResources.You_can_not_update_this_administrator;
            }

            if (criteria.Password != criteria.ConfirmPassword)
            {
                actionResult.IsSuccess = false;
                actionResult.ResultMessage = ViewResources.Error_PasswordNotMatch;
            }
            else
            {
                criteria.Password = Cryptography.CfsEnCode(HttpUtility.HtmlEncode(criteria.Password.Trim()));
                criteria.Creator = CurrentSession.UserId;
                criteria.IP = CurrentContext.IP;
                var result = _adminService.UpdateAdminPassword(criteria);
                if (result != null && result.IsSuccessful)
                {
                    actionResult.IsSuccess = true;
                    actionResult.ResultMessage = ViewResources.ActionResult_UpdatePasswordSuccess;
                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.ResultMessage = result != null ? result.Message : string.Empty;
                }
            }

            return Json(actionResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public string ListExistedAdministrator(AdminList_GetTop10Criteria criteria)
        {
            criteria.SystemId = CurrentSession.SystemId;
            var top10AdminList = this._adminService.AdminList_GetTop10(criteria).Tables[0];
            return DataForAutoComplete(top10AdminList, "UserName");
        }

        #region Private methods

        /// <summary>
        /// Check the current user has permission to edit a user.
        /// </summary>
        /// <param name="userRole">Role of the current user.</param>
        /// <param name="editedUserId">Id of the edited user.</param>
        /// <returns>True if has permission else False.</returns>
        private bool HasEditPermission(int userRole, int editedUserId)
        {
            var userInfo = this._userService.GetUserInfo(new GetUserInfoCriteria
            {
                UserId = editedUserId
            }).ToList().FirstOrDefault();

            if (userInfo != null)
            {
                var editedUserRole = userInfo.RoleID;
                switch (userRole)
                {
                    case (int)Constant.UserRole.ITAdministrator:
                        return true;
                    case (int)Constant.UserRole.Licensee:
                        return editedUserRole == (int)Constant.UserRole.LicenseeSubAccount;
                    default:
                        return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Customize after editing the system of user has licensee role.
        /// </summary>
        /// <param name="criteria">The user's criteria.</param>
        /// <param name="systemList">The system list.</param>
        /// <returns>Action result page.</returns>
        private ActionResult CustomizeAfterEditSystem(UpdateAdminSystemCriteria criteria, IEnumerable<SelectListItem> systemList)
        {
            if (criteria.UserID == CurrentSession.UserId && systemList != null)
            {
                if (criteria.SystemList == null)
                {
                    new LoginController().DoSignOut();
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    var updateList = new List<SelectListItem>();
                    var strSystemList = string.Empty;
                    foreach (var system in systemList)
                    {
                        if (criteria.SystemList.Contains(system.Value))
                        {
                            strSystemList += system.Value + "," + system.Text + ";";
                            updateList.Add(system);
                        }
                    }

                    ViewBag.SystemListID = updateList;
                    CurrentSession.SystemList = strSystemList;
                }
            }
            else
            {
                ViewBag.SystemListID = systemList;
            }

            ViewBag.CurrentUser = " \"" + criteria.UserName + "\"";
            return View(criteria);
        }

        #endregion
    }
}
