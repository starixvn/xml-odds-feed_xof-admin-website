namespace Fanex.OddsFeed.Controllers
{
    using System.Text;
    using System.Web;
    using Fanex.OddsFeed.Core;
    using Fanex.OddsFeed.Core.Attribute;
    using Fanex.OddsFeed.Core.Context;
    using Fanex.OddsFeed.Core.Session;
    using Fanex.OddsFeed.Criteria.Administrator;
    using Fanex.OddsFeed.Criteria.Licensee;
    using Fanex.OddsFeed.Criteria.User;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Resources;
    using Fanex.OddsFeed.Services.Interface;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    [IsAuthorize]
    public class AdministratorController : BaseController
    {
        private readonly IUserService userService;

        public AdministratorController(IUserService userService,
            ICurrentContext currentContext,
            ICurrentSession currentSession,
            IBaseService baseService,
            AppSettings appSettings) : base(currentContext, currentSession, baseService, appSettings)
        {
            this.userService = userService;
        }

        #region Add New Administrator

        [HttpGet]
        public async Task<IActionResult> AdministratorAdd()
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            // Sport Flag
            var licenseeProfile = await baseService.GetDataEntityAsync<LicenseeProfile>(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = currentSession.SystemId });
            if (licenseeProfile != null)
            {
                ViewBag.SportsFlag = licenseeProfile.SportIdListFlag;
            }

            // System
            var systems =await baseService.GetDataAsync<SystemDto>(new GetAllSystemsCriteria());
            ViewBag.SystemList = systems.Select(x => new SelectListItem { Value = x.SystemId.ToString(), Text = x.SystemName });
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNew([FromBody] InsertAdminCriteria criteria)
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
            CommandResult result = await baseService.ExecuteNonQueryAsync(criteria);
            if (result != null && result.IsSuccessful)
            {
                ViewBag.Success = true;
                addResult = ViewResources.SuccessfulMessage;
            }
            else
            {
                addResult = result != null ? result.Message : ViewResources.InternalErrorMessage;
            }

            return Json(addResult);
        }

        [HttpPost]
        public async Task<IActionResult> CheckExistAdministrator([FromBody] InsertAdminCriteria criteria)
        {
            var result = new CommandResult { Code = 0 };
            var users =await baseService.GetDataAsync<User>(new CheckExistAdministratorCriteria
            {
                Username = criteria.UserName,
                PageIndex = 1,
                PageSize = 1,
                UserID = 0,
                SystemID = 0
            });
            if (users.Any())
            {
                result.Code = 1;
            }

            return Json(result);
        }

        #endregion

        #region Administrator List

        public async Task<IActionResult> AdministratorList()
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            // System
            var systems =await baseService.GetDataAsync<SystemDto>(new GetAllSystemsCriteria());
            var systemList = systems.Select(x => new SelectListItem { Value = x.SystemId.ToString(), Text = x.SystemName }).ToList();
            systemList.Insert(0, new SelectListItem { Selected = true, Text = "All", Value = "0" });
            ViewBag.SystemList = systemList;

            // Role
            var roles = await baseService.GetDataAsync<UserRole>(new GetUserRoleCriteria());
            var roleList = roles.Select(x => new SelectListItem { Value = x.RoleId.ToString(), Text = x.RoleName }).ToList();
            roleList.Insert(0, new SelectListItem { Selected = true, Text = "All", Value = "0" });
            ViewBag.RoleList = roleList;
            return View("~/Views/Administrator/AdministratorList.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> GetListAdminBySystemAndNameAndRole([FromBody] GetAdminCriteria criteria)
        {
            criteria.PageIndex++;
            criteria.Username = criteria.Username.Equals("Username") ? string.Empty : criteria.Username.Trim();
            var result = await baseService.GetDataAsync<User>(criteria);
            return new JsonResult(new {data = result, totalItems = criteria.RowCount});
        }

        #endregion

        #region Update Active status

        [HttpPost]
        public async Task<string> UpdateActive(int adminid, bool isactive)
        {
            var criteria = new DeleteAdminCriteria
            {
                AdminID = adminid,
                Isactive = isactive
            };
            CommandResult result = await baseService.ExecuteNonQueryAsync(criteria);

            var responseMessage = (result != null && result.IsSuccessful)
                ? ViewResources.SuccessfulMessage
                : ViewResources.InternalErrorMessage;

            return responseMessage;
        }

        #endregion

        #region Update administrator's profile information

        [HttpGet]
        public async Task<IActionResult> AdministratorEditProfile(int id, string username)
        {
            if (id == 0)
            {
                return RedirectToAction("AdministratorList", "Administrator");
            }

            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            User userInfo = await userService.GetUserInfoAsync(id);
            ViewBag.CurrentUser = " of \"" + username + "\"";
            ViewBag.CurrentUserId = currentSession.UserId;
            var criteria = new UpdateAdminInfoCriteria { UserID = id, Username = username };
            if (userInfo != null)
            {
                criteria.FirstName = userInfo.FirstName;
                criteria.LastName = userInfo.LastName;      
                criteria.Email = userInfo.Email;
                criteria.RoleID = userInfo.RoleID;

                ViewBag.HasEditPermission = await this.HasEditPermission(currentSession.UserRole, criteria.UserID);
                if (!ViewBag.HasEditPermission)
                {
                    ViewBag.Error = ViewResources.You_have_no_authorization;
                }
            }

            return View(criteria);
        }

        [HttpPost]
        public async Task<IActionResult> AdministratorEditProfile([FromBody] UpdateAdminInfoCriteria criteria)
        {
            var result = new CommandResult();
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            criteria.FirstName = criteria.FirstName ?? string.Empty;
            criteria.LastName = criteria.LastName ?? string.Empty;

            if (!await this.HasEditPermission(currentSession.UserRole, criteria.UserID))
            {
                result.Code = 1;
                result.Message = ViewResources.You_can_not_update_this_administrator;
                return Json(result);
            }

            result =await baseService.ExecuteNonQueryAsync(criteria);
            if (currentSession.UserId == criteria.UserID)
            {
                currentSession.UserRole = (byte)criteria.RoleID;
            }

            return Json(result);
        }

        #endregion

        #region Update administrator's system info

        [HttpGet]
        public async Task<IActionResult> AdministratorEditSystem(int id, string username, int role = 0)
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
            var systems = role == (byte)Core.Enums.UserRole.Licensee
                ? await baseService.GetDataAsync<SystemDto>(new GetSystemByUserIDCriteria { UserID = currentSession.UserId, IgnoreIsActive = true })
                : await baseService.GetDataAsync<SystemDto>(new GetOptionalSystemBySubAccountIDCriteria { UserID = currentSession.UserId, SubAccountID = id });
            ViewBag.SystemListID = systems.Select(x => new SelectListItem { Value = x.SystemId.ToString(), Text = x.SystemName }).ToList();

            var systemList = await baseService.GetDataAsync<SystemDto>(new GetSystemByUserIDCriteria
                { UserID = id, IgnoreIsActive = false });

            var criteria = new UpdateAdminSystemCriteria
            {
                UserID = id,
                UserName = username,
                SystemList = systemList
                        .Select(item => item.SystemId.ToString()).ToArray()
            };

            ViewBag.HasEditPermission = await this.HasEditPermission(currentSession.UserRole, criteria.UserID);
            if (!ViewBag.HasEditPermission)
            {
                ViewBag.Error = ViewResources.You_have_no_authorization;
            }

            return View(criteria);
        }

        [HttpPost]
        public async Task<IActionResult> AdministratorEditSystem([FromBody] UpdateAdminSystemCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;

            if (!await this.HasEditPermission(currentSession.UserRole, criteria.UserID))
            {
                ViewBag.Success = false;
                ViewBag.Error = ViewResources.You_can_not_update_this_administrator;
                return View(criteria);
            }

            // System
            var systems = IsLicenseeAdminRole()
                ? await baseService.GetDataAsync<SystemDto>(new GetSystemByUserIDCriteria { UserID = currentSession.UserId, IgnoreIsActive = true })
                : await baseService.GetDataAsync<SystemDto>(new GetOptionalSystemBySubAccountIDCriteria { UserID = currentSession.UserId, SubAccountID = criteria.UserID });
            var systemList = systems.Select(x => new SelectListItem { Value = x.SystemId.ToString(), Text = x.SystemName }).ToList();

            if (string.IsNullOrEmpty(criteria.ListSystemId))
            {
                ViewBag.SystemListID = systemList;
                ViewBag.Success = false;
                ViewBag.Error = ViewResources.Validate_Licensee_Require;
                return View(criteria);
            }

            CommandResult result =await baseService.ExecuteNonQueryAsync(criteria);
            if (result != null && result.IsSuccessful)
            {
                ViewBag.Success = true;
            }

            return await CustomizeAfterEditSystem(criteria, systemList);
        }
        #endregion

        #region import existed admin

        [HttpPost]
        public async Task<string> ListExistedAdmin(string username)
        {
            var criteria = new GetTop10AdminCriteria
            {
                Username = username
            };
            var users = await baseService.GetDataAsync<User>(criteria);
            return string.Join(",", users.Select(x => x.Username));
        }

        [HttpPost]
        public async Task<string> GetExistedAdmin(string username)
        {
            var licenseeProfile =await baseService.GetDataEntityAsync<LicenseeProfile>(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = currentSession.SystemId });
            if (licenseeProfile != null)
            {
                ViewBag.SportsFlag = licenseeProfile.SportIdListFlag;
            }

            var criteria = new GetExistedAdminCriteria
            {
                Username = username
            };
            var user =await baseService.GetDataEntityAsync<User>(criteria);
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
        public async Task<IActionResult> AdministratorResetPwd(int id, string username)
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

            ViewBag.HasEditPermission = await this.HasEditPermission(currentSession.UserRole, criteria.UserID);
            if (!ViewBag.HasEditPermission)
            {
                ViewBag.Error = ViewResources.You_have_no_authorization;
            }

            return View(criteria);
        }

        [HttpPost]
        public async Task<IActionResult> AdministratorResetPwd([FromBody] UpdateAdminPasswordCriteria criteria)
        {
            var result = new CommandResult();
            if (!await this.HasEditPermission(currentSession.UserRole, criteria.UserID))
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
                result = await baseService.ExecuteNonQueryAsync(criteria);
                if (result.IsSuccessful)
                {
                    result.Message = ViewResources.ActionResult_UpdatePasswordSuccess;
                }
            }

            return Json(result);
        }

        #endregion

        public async Task<string> ListExistedAdministrator([FromBody] AdminList_GetTop10Criteria criteria)
        {
            criteria.SystemId = currentSession.SystemId;
            var users =await baseService.GetDataAsync<User>(criteria);
            var builder = new StringBuilder();
            foreach (var user in users)
            {
                builder.AppendFormat("{0},", user.Username);
            }

            return builder.ToString();
        }

        #region Private methods

        private async Task<bool> HasEditPermission(int userRole, int editedUserId)
        {
            User userInfo = await userService.GetUserInfoAsync(editedUserId);
            if (userInfo != null)
            {
                var editedUserRole = userInfo.RoleID;
                switch (userRole)
                {
                    case (byte)Core.Enums.UserRole.ITAdministrator:
                        return true;
                    case (byte)Core.Enums.UserRole.Licensee:
                        return editedUserRole == (byte)Core.Enums.UserRole.LicenseeSubAccount;
                    default:
                        return false;
                }
            }

            return false;
        }

        private async Task<IActionResult> CustomizeAfterEditSystem(UpdateAdminSystemCriteria criteria, IEnumerable<SelectListItem> systemList)
        {
            if (criteria.UserID == currentSession.UserId && systemList != null)
            {
                if (criteria.SystemList == null)
                {
                    var logOutUserCriteria = new LogOutUserCriteria
                    {
                        OnUserId = currentSession.OnUserId
                    };

                    await baseService.GetDataEntityAsync<CommandResult>(logOutUserCriteria);
                    currentSession.ClearAll();
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
