namespace Fanex.OddsFeed.Admin.Controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.OddsFeed.Core.Business;
    using Fanex.OddsFeed.Core.Criteria.Administrator;
    using Fanex.OddsFeed.Core.Criteria.Licensee;
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Service.Impl;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Core.Web.Attribute;
    using Fanex.OddsFeed.Framework.Mvc;

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

            // Sport Flag
            var licenseeProfile = baseService.GetDataEntity<LicenseeProfile>(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = currentSession.SystemId });
            if (licenseeProfile != null)
            {
                ViewBag.SportsFlag = licenseeProfile.SportIdListFlag;
            }

            // System
            var systems = baseService.GetData<SystemDto>(new GetAllSystemsCriteria());
            ViewBag.SystemList = systems.Select(x => new SelectListItem { Value = x.SystemId.ToString(), Text = x.SystemName });
            return View();
        }

        [HttpPost]
        public JsonResult AddNew(InsertAdminCriteria criteria)
        {
            string addResult;

            criteria.FirstName = criteria.FirstName ?? string.Empty;
            criteria.LastName = criteria.LastName ?? string.Empty;
            criteria.Password = criteria.Password.Trim();
            // hash password incase of add new
            if (!criteria.IsImport)
            {
                criteria.Password = Cryptography.CfsEnCode(HttpUtility.HtmlEncode(criteria.Password));
            }

            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            CommandResult result = baseService.ExecuteNonQuery(criteria);
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
            var result = new CommandResult { Code = 0 };
            var users = baseService.GetData<User>(new CheckExistAdministratorCriteria
            {
                Username = criteria.UserName,
                PageIndex = 1,
                PageSize = 1,
                UserID = 0,
                SystemID = 0
            });
            if (users != null && users.Any())
            {
                result.Code = 1;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Administrator List

        public ActionResult AdministratorList()
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            // System
            var systems = baseService.GetData<SystemDto>(new GetAllSystemsCriteria());
            var systemList = systems.Select(x => new SelectListItem { Value = x.SystemId.ToString(), Text = x.SystemName }).ToList();
            systemList.Insert(0, new SelectListItem { Selected = true, Text = "All", Value = "0" });
            ViewBag.SystemList = systemList;

            // Role
            var roles = baseService.GetData<UserRole>(new GetUserRoleCriteria());
            var roleList = roles.Select(x => new SelectListItem { Value = x.RoleId.ToString(), Text = x.RoleName }).ToList();
            roleList.Insert(0, new SelectListItem { Selected = true, Text = "All", Value = "0" });
            ViewBag.RoleList = roleList;
            return View("~/Views/Administrator/AdministratorList.cshtml");
        }

        [HttpPost]
        public JsonResult GetListAdminBySystemAndNameAndRole(GetAdminCriteria criteria)
        {
            criteria.PageIndex++;
            criteria.Username = (criteria.Username == null || criteria.Username == "Username")
                ? string.Empty : criteria.Username.Trim();
            var result = baseService.GetDataGrid<User>(criteria);
            return new CustomJsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
            CommandResult result = baseService.ExecuteNonQuery(criteria);

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
            var result = new CommandResult();
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            criteria.FirstName = criteria.FirstName ?? string.Empty;
            criteria.LastName = criteria.LastName ?? string.Empty;

            if (!this.HasEditPermission(currentSession.UserRole, criteria.UserID))
            {
                result.Code = 1;
                result.Message = ViewResources.You_can_not_update_this_administrator;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            result = baseService.ExecuteNonQuery(criteria);
            if (currentSession.UserId == criteria.UserID)
            {
                currentSession.UserRole = (byte)criteria.RoleID;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
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
            // System
            var systems = role == (int)Constant.UserRole.Licensee
                ? baseService.GetData<SystemDto>(new GetSystemByUserIDCriteria { UserID = currentSession.UserId, IgnoreIsActive = true })
                : baseService.GetData<SystemDto>(new GetOptionalSystemBySubAccountIDCriteria { UserID = currentSession.UserId, SubAccountID = id });
            ViewBag.SystemListID = systems.Select(x => new SelectListItem { Value = x.SystemId.ToString(), Text = x.SystemName }).ToList();

            var criteria = new UpdateAdminSystemCriteria
            {
                UserID = id,
                UserName = username,
                SystemList = baseService.GetData<SystemDto>(new GetSystemByUserIDCriteria { UserID = id, IgnoreIsActive = false })
                        .Select(item => item.SystemId.ToString()).ToArray()
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

            // System
            var systems = IsLicenseeAdminRole()
                ? baseService.GetData<SystemDto>(new GetSystemByUserIDCriteria { UserID = currentSession.UserId, IgnoreIsActive = true })
                : baseService.GetData<SystemDto>(new GetOptionalSystemBySubAccountIDCriteria { UserID = currentSession.UserId, SubAccountID = criteria.UserID });
            var systemList = systems.Select(x => new SelectListItem { Value = x.SystemId.ToString(), Text = x.SystemName }).ToList();

            if (string.IsNullOrEmpty(criteria.ListSystemId))
            {
                ViewBag.SystemListID = systemList;
                ViewBag.Success = false;
                ViewBag.Error = ViewResources.Validate_Licensee_Require;
                return View(criteria);
            }

            CommandResult result = baseService.ExecuteNonQuery(criteria);
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
            var users = baseService.GetData<User>(criteria);
            return string.Join(",", users.Select(x => x.Username));
        }

        [HttpPost]
        public string GetExistedAdmin(string username)
        {
            var licenseeProfile = baseService.GetDataEntity<LicenseeProfile>(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = currentSession.SystemId });
            if (licenseeProfile != null)
            {
                ViewBag.SportsFlag = licenseeProfile.SportIdListFlag;
            }

            var criteria = new GetExistedAdminCriteria
            {
                Username = username
            };
            var user = baseService.GetDataEntity<User>(criteria);
            if (user != null)
            {
                var resultStr = string.Format("{0},{1},{2},{3},{4},{5}",
                    user.Username,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Password,
                    user.RoleID);

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
            var result = new CommandResult();
            if (!this.HasEditPermission(currentSession.UserRole, criteria.UserID))
            {
                result.Code = 1;
                result.Message = ViewResources.You_can_not_update_this_administrator;
            }

            if (criteria.Password != criteria.ConfirmPassword)
            {
                result.Code = 1;
                result.Message = ViewResources.Error_PasswordNotMatch;
            }
            else
            {
                criteria.Password = Cryptography.CfsEnCode(HttpUtility.HtmlEncode(criteria.Password.Trim()));
                criteria.Creator = currentSession.UserId;
                criteria.IP = currentContext.IP;
                result = baseService.ExecuteNonQuery(criteria);
                if (result.IsSuccessful)
                {
                    result.Message = ViewResources.ActionResult_UpdatePasswordSuccess;
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
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