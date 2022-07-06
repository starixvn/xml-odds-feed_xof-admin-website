namespace Fanex.OddsFeed.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.OddsFeed.Core.Business;
    using Fanex.OddsFeed.Core.Criteria.Administrator;
    using Fanex.OddsFeed.Core.Criteria.Licensee;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Service.Impl;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Core.Web.Attribute;
    using Fanex.OddsFeed.Framework.Data;

    [IsAuthorize]
    public class AdministratorController : BaseController
    {
        private readonly IUserService userService;

        public AdministratorController() : this(null)
        {
        }

        public AdministratorController(IUserService userService)
        {
            this.userService = userService ?? new UserService();
        }

        #region Add New Administrator

        [HttpGet]
        public ActionResult AdministratorAdd()
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            ////Sport Flag
            var flags = baseService.GetDataSet(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = currentSession.SystemId });
            if (flags != null && flags.Tables.Count > 0 && flags.Tables[0].Rows.Count > 0)
            {
                ViewBag.SportsFlag = bool.Parse(flags.Tables[0].Rows[0]["SportIDListFlag"].ToString());
            }

            ////System
            ViewBag.SystemList = baseService.GetSelectList(new GetAllSystemsCriteria());
            return View();
        }

        [HttpPost]
        public JsonResult AddNew(InsertAdminCriteria criteria)
        {
            string addResult;

            criteria.FirstName = criteria.FirstName ?? string.Empty;
            criteria.LastName = criteria.LastName ?? string.Empty;
            criteria.Password = criteria.Password.Trim();
            //// hash password incase of add new
            if (!criteria.IsImport)
            {
                criteria.Password = Cryptography.CfsEnCode(HttpUtility.HtmlEncode(criteria.Password));
            }

            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            ICommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result != null && result.IsSuccessful)
            {
                ViewBag.Success = true;
                addResult = ViewResources.SuccessfulMessage;
            }
            else
            {
                addResult = result != null ? result.Message : ViewResources.InternalErrorMessage;
            }

            return Json(addResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckExistAdministrator(InsertAdminCriteria criteria)
        {
            var checkResult = new OddsFeedActionResult();
            var res = baseService.GetData(new CheckExistAdministratorCriteria
            {
                Username = criteria.UserName,
                PageIndex = 1,
                PageSize = 1,
                UserID = 0,
                SystemID = 0
            });
            if (res.Data.Tables.Count > 0 && res.Data.Tables[0].Rows.Count > 0)
            {
                checkResult.IsSuccess = false;
            }

            return Json(checkResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Administrator List

        public ActionResult AdministratorList()
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            ////System
            var systemList = (List<SelectListItem>)baseService.GetSelectList("Odds_System_SelAll");
            systemList.Insert(0, new SelectListItem { Selected = true, Text = "All", Value = "0" });
            ViewBag.SystemList = systemList;

            ////Role
            var roleList = (List<SelectListItem>)baseService.GetSelectList("Odds_Role_Sel");
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
            var dr = baseService.GetData(criteria);
            var result = FanexGridHelper.DataForFanexGrid_PreventContainQuote(dr);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update Active status

        [HttpPost]
        public string UpdateActive(int adminid, bool isactive)
        {
            var criteria = new DeleteAdminCriteria
            {
                AdminID = adminid,
                Isactive = isactive
            };
            ICommandResult result = baseService.ExecuteNonQuery(criteria);

            var responseMessage = (result != null && result.IsSuccessful)
                ? ViewResources.SuccessfulMessage
                : ViewResources.InternalErrorMessage;

            return responseMessage;
        }

        #endregion

        #region Update administrator's profile information

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

            User userInfo = userService.GetUserInfo(id);
            ViewBag.CurrentUser = " of \"" + username + "\"";
            ViewBag.CurrentUserId = currentSession.UserId;
            var criteria = new UpdateAdminInfoCriteria { UserID = id, Username = username };

            if (userInfo != null)
            {
                criteria.FirstName = userInfo.FirstName;
                criteria.LastName = userInfo.LastName;
                criteria.Email = userInfo.Email;
                criteria.RoleID = userInfo.RoleID;

                ViewBag.HasEditPermission = this.HasEditPermission(currentSession.UserRole, criteria.UserID);
                if (!ViewBag.HasEditPermission)
                {
                    ViewBag.Error = ViewResources.You_have_no_authorization;
                }
            }

            return View(criteria);
        }

        [HttpPost]
        public JsonResult AdministratorEditProfile(UpdateAdminInfoCriteria criteria)
        {
            var actionResult = new OddsFeedActionResult();
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            criteria.FirstName = criteria.FirstName ?? string.Empty;
            criteria.LastName = criteria.LastName ?? string.Empty;

            if (!this.HasEditPermission(currentSession.UserRole, criteria.UserID))
            {
                actionResult.IsSuccess = false;
                actionResult.ResultMessage = ViewResources.You_can_not_update_this_administrator;
                return Json(actionResult, JsonRequestBehavior.AllowGet);
            }

            ICommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result == null || !result.IsSuccessful)
            {
                actionResult.IsSuccess = false;
                if (result != null)
                {
                    actionResult.ResultMessage = result.Message;
                }
            }

            if (currentSession.UserId == criteria.UserID)
            {
                currentSession.UserRole = (byte)criteria.RoleID;
            }

            return Json(actionResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update administrator's system info

        [HttpGet]
        public ActionResult AdministratorEditSystem(int id, string username, int role = 0)
        {
            if (id == 0)
            {
                return RedirectToAction("AdministratorList", "Administrator");
            }

            if (!IsAccountManagerRole())
            {
                return NoAuthorization();
            }

            ViewBag.CurrentUser = " \"" + username + "\"";
            ////System
            var systemList = role == (int)Constant.UserRole.Licensee
                ? baseService.GetSelectList(new GetSystemByUserIDCriteria { UserID = currentSession.UserId, IgnoreIsActive = true })
                : baseService.GetSelectList(new GetOptionalSystemBySubAccountIDCriteria { UserID = currentSession.UserId, SubAccountID = id });

            ViewBag.SystemListID = systemList;

            var criteria = new UpdateAdminSystemCriteria
            {
                UserID = id,
                UserName = username,
                SystemList = baseService.GetSelectList(new GetSystemByUserIDCriteria { UserID = id, IgnoreIsActive = false }).Select(item => item.Value).ToArray()
            };

            ViewBag.HasEditPermission = this.HasEditPermission(currentSession.UserRole, criteria.UserID);
            if (!ViewBag.HasEditPermission)
            {
                ViewBag.Error = ViewResources.You_have_no_authorization;
            }

            return View(criteria);
        }

        [HttpPost]
        public ActionResult AdministratorEditSystem(UpdateAdminSystemCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;

            if (!this.HasEditPermission(currentSession.UserRole, criteria.UserID))
            {
                ViewBag.Success = false;
                ViewBag.Error = ViewResources.You_can_not_update_this_administrator;
                return View(criteria);
            }

            ////System
            var systemList = IsLicenseeAdminRole()
                ? baseService.GetSelectList(new GetSystemByUserIDCriteria { UserID = currentSession.UserId, IgnoreIsActive = true })
                : baseService.GetSelectList(new GetOptionalSystemBySubAccountIDCriteria { UserID = currentSession.UserId, SubAccountID = criteria.UserID });

            if (string.IsNullOrEmpty(criteria.ListSystemId))
            {
                ViewBag.SystemListID = systemList;
                ViewBag.Success = false;
                ViewBag.Error = ViewResources.Validate_Licensee_Require;
                return View(criteria);
            }

            ICommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result != null && result.IsSuccessful)
            {
                ViewBag.Success = true;
            }

            return CustomizeAfterEditSystem(criteria, systemList);
        }
        #endregion

        #region import existed admin

        [HttpPost]
        public string ListExistedAdmin(string username)
        {
            var criteria = new GetTop10AdminCriteria
            {
                Username = username
            };

            var top10AdminDc = baseService.GetDataSet(criteria).Tables[0];
            return DataForAutoComplete(top10AdminDc, "username");
        }

        [HttpPost]
        public string GetExistedAdmin(string username)
        {
            var licenseeProfile = baseService.GetDataSet(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = currentSession.SystemId });
            if (licenseeProfile != null && licenseeProfile.Tables.Count > 0 && licenseeProfile.Tables[0].Rows.Count > 0)
            {
                ViewBag.SportsFlag = bool.Parse(licenseeProfile.Tables[0].Rows[0]["SportIDListFlag"].ToString());
            }

            var criteria = new GetExistedAdminCriteria
            {
                Username = username
            };
            var result = baseService.GetDataSet(criteria);
            if (result.Tables.Count != 0)
            {
                var resultStr = result.Tables[0].Rows[0]["username"].ToString();
                resultStr += "," + result.Tables[0].Rows[0]["firstname"];
                resultStr += "," + result.Tables[0].Rows[0]["lastname"];
                resultStr += "," + result.Tables[0].Rows[0]["email"];
                resultStr += "," + result.Tables[0].Rows[0]["password"];
                resultStr += "," + result.Tables[0].Rows[0]["roleid"];

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

            ViewBag.HasEditPermission = this.HasEditPermission(currentSession.UserRole, criteria.UserID);
            if (!ViewBag.HasEditPermission)
            {
                ViewBag.Error = ViewResources.You_have_no_authorization;
            }

            return View(criteria);
        }

        [HttpPost]
        public JsonResult AdministratorResetPwd(UpdateAdminPasswordCriteria criteria)
        {
            var actionResult = new OddsFeedActionResult();
            if (!this.HasEditPermission(currentSession.UserRole, criteria.UserID))
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
                criteria.Creator = currentSession.UserId;
                criteria.IP = currentContext.IP;
                ICommandResult result = baseService.ExecuteNonQuery(criteria);
                if (result != null && result.IsSuccessful)
                {
                    actionResult.IsSuccess = true;
                    actionResult.ResultMessage = ViewResources.ActionResult_UpdatePasswordSuccess;
                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.ResultMessage = result != null ? result.Message : ViewResources.InternalErrorMessage;
                }
            }

            return Json(actionResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public string ListExistedAdministrator(AdminList_GetTop10Criteria criteria)
        {
            criteria.SystemId = currentSession.SystemId;
            var users = baseService.GetData<User>(criteria);
            var builder = new StringBuilder();
            foreach (var user in users)
            {
                builder.AppendFormat("{0},", user.Username);
            }

            return builder.ToString();
        }

        #region Private methods

        private bool HasEditPermission(int userRole, int editedUserId)
        {
            User userInfo = userService.GetUserInfo(editedUserId);
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

        private ActionResult CustomizeAfterEditSystem(UpdateAdminSystemCriteria criteria, IEnumerable<SelectListItem> systemList)
        {
            if (criteria.UserID == currentSession.UserId && systemList != null)
            {
                if (criteria.SystemList == null)
                {
                    new LoginController().DoSignOut();
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    var updateList = new List<SelectListItem>();
                    var strSystemList = new StringBuilder();

                    foreach (var system in systemList)
                    {
                        if (criteria.SystemList.Contains(system.Value))
                        {
                            strSystemList.Append(system.Value + "," + system.Text + ";");
                            updateList.Add(system);
                        }
                    }

                    ViewBag.SystemListID = updateList;
                    currentSession.SystemList = strSystemList.ToString();
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