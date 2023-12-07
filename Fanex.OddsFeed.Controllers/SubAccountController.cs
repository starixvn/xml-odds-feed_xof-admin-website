namespace Fanex.OddsFeed.Controllers
{
    using System.Web;
    using Fanex.OddsFeed.Core;
    using Fanex.OddsFeed.Core.Attribute;
    using Fanex.OddsFeed.Core.Context;
    using Fanex.OddsFeed.Core.Session;
    using Fanex.OddsFeed.Criteria.Administrator;
    using Fanex.OddsFeed.Criteria.Licensee;
    using Fanex.OddsFeed.Criteria.SubAccount;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Resources;
    using Fanex.OddsFeed.Services.Interface;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    [IsAuthorize]
    public class SubAccountController : BaseController
    {
        private readonly ISubAccService subAccService;
        public SubAccountController(ICurrentContext currentContext, 
            ICurrentSession currentSession, 
            IBaseService baseService,
            ISubAccService subAccService,
            AppSettings appSettings) : base(currentContext, currentSession, baseService, appSettings)
        {
            this.subAccService = subAccService;
        }

        #region Add

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if (!HasEditPermission(currentSession.UserRole))
            {
                return this.NoAuthorization();
            }

            var systems = await baseService.GetDataAsync<SystemDto>(new GetSystemByUserIDCriteria { UserID = currentSession.UserId, IgnoreIsActive = IsAccountManagerRole() });
            ViewBag.SystemList = systems.Select(x => new SelectListItem { Value = x.SystemId.ToString(), Text = x.SystemName }).ToList();
            await ViewBagFlags();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] InsertSubAccCriteria criteria)
        {
            criteria.SystemID = currentSession.SystemId;
            criteria.Creator = currentSession.UserId;
            criteria.RoleID = (byte)Core.Enums.UserRole.LicenseeSubAccount;
            criteria.Password = Cryptography.CfsEnCode(HttpUtility.HtmlEncode(criteria.Password.Trim()));
            criteria.FirstName = criteria.FirstName ?? string.Empty;
            criteria.LastName = criteria.LastName ?? string.Empty;

            CommandResult result = await subAccService.InsertAsync(criteria);
            if (result.IsSuccessful)
            {
                result.Message = ViewResources.ActionResult_AddSubAccountSuccess;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> CheckExistSubAccount([FromBody] InsertSubAccCriteria criteria)
        {
            var result = new CommandResult { Code = 0 };
            var users = await baseService.GetDataAsync<User>(new CheckExistSubAccountCriteria
            {
                Username = string.IsNullOrEmpty(criteria.UserName) ? string.Empty : criteria.UserName,
                PageIndex = 1,
                PageSize = 1,
                RoleId = (byte)Core.Enums.UserRole.LicenseeSubAccount
            });

            if (users != null && users.Any())
            {
                result.Code = 1;
            }

            return Json(result);
        }

        #endregion Add

        #region Get

        [HttpGet]
        public IActionResult List()
        {
            if (!HasEditPermission(currentSession.UserRole))
            {
                return this.NoAuthorization();
            }

            return View("SubAccList");
        }

        public async Task<IActionResult> GetSubAccountList(GetSubAccByNameCriteria criteria)
        {
            criteria.SystemId = currentSession.SystemId;
            criteria.Username = (criteria.Username == null || criteria.Username == "Username") ? string.Empty : criteria.Username.Trim();
            criteria.RoleId = (byte)Core.Enums.UserRole.LicenseeSubAccount;
            criteria.PageIndex++;
            var result = await baseService.GetDataGridAsync<User>(criteria);
            return new CustomJsonResult(result);
        }

        public async Task<string> ListExistedSubAccount(string username)
        {
            var criteria = new GetTop10SubAccCriteria
            {
                SystemId = currentSession.SystemId,
                RoleId = (byte)Core.Enums.UserRole.LicenseeSubAccount,
                Username = username
            };
            var users = await baseService.GetDataAsync<User>(criteria);
            return string.Join(",", users.Select(x => x.Username));
        }

        #endregion Get

        #region Edit

        public async Task<IActionResult> EditSubAccInfo(int id, string userName)
        {
            if (!HasEditPermission(currentSession.UserRole))
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

            var users = await subAccService.GetBySystemIdAsync(currentSession.SystemId);
            var user = users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                ViewBag.CurrentUser = " of \"" + userName + "\"";
                criteria.FirstName = user.FirstName;
                criteria.LastName = user.LastName;
                criteria.Email = user.Email;
                criteria.IsActive = user.IsActive;
            }

            return View(criteria);
        }

        [HttpPost]
        public async Task<IActionResult> EditSubAccInfo([FromBody] UpdateSubAccCriteria criteria)
        {
            var result = new CommandResult();
            if (ModelState.IsValid)
            {
                criteria.FirstName = criteria.FirstName ?? string.Empty;
                criteria.LastName = criteria.LastName ?? string.Empty;

                result = await baseService.ExecuteNonQueryAsync(criteria);
            }

            return Json(result);
        }

        public async Task<string> EditActive(int userId, bool isActive)
        {
            var criteria = new UpdateActiveSubAccCriteria
            {
                SubAccountID = userId,
                IsActive = isActive
            };
            CommandResult result = await baseService.ExecuteNonQueryAsync(criteria);

            return result.IsSuccessful ? ViewResources.SuccessfulMessage : result.Message;
        }

        [HttpGet]
        public IActionResult ResetPassword(int id, string userName)
        {
            if (!HasEditPermission(currentSession.UserRole))
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

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassSubAccCriteria criteria)
        {
            var result = new CommandResult();
            if (ModelState.IsValid)
            {
                if (criteria.Password != criteria.ConfirmPassword)
                {
                    result.Message = ViewResources.Error_PasswordNotMatch;
                }
                else
                {
                    criteria.Password = Cryptography.CfsEnCode(HttpUtility.HtmlEncode(criteria.Password.Trim()));
                    result = await baseService.ExecuteNonQueryAsync(criteria);

                    if (result.IsSuccessful)
                    {
                        result.Message = ViewResources.ActionResult_UpdatePasswordSuccess;
                    }
                }
            }

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> EditPermission(int id, string userName)
        {
            if (!HasEditPermission(currentSession.UserRole))
            {
                return NoAuthorization();
            }

            if (id == 0)
            {
                return RedirectToAction("List", "SubAccount");
            }

            ViewBag.CurrentUser = " of \"" + userName + "\"";
            var systemId = currentSession.SystemId;

            PermissionFlag result = await subAccService.GetUserPermissionAsync(id, systemId);
            if (result == null)
            {
                return View();
            }

            var criteria = new EditPermissionSubAccCriteria
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

            await ViewBagFlags();

            return View(criteria);
        }

        [HttpPost]
        public async Task<IActionResult> EditPermission(EditPermissionSubAccCriteria criteria)
        {
            if (ModelState.IsValid)
            {
                criteria.UpdatedBy = currentSession.UserId;
                var result = await baseService.ExecuteNonQueryAsync(criteria);
                if (result != null && result.IsSuccessful)
                {
                    ViewBag.Success = true;
                }
                else
                {
                    ViewBag.Error = result != null ? result.Message : ViewResources.ActionResult_EmptyResultError;
                }
            }

            await ViewBagFlags();

            ViewBag.CurrentUser = " of \"" + criteria.SubAccountName + "\"";
            return View(criteria);
        }

        #endregion Edit

        #region Del

        public async Task<string> Delete(int userId)
        {
            var resultMessage = ViewResources.ActionResult_EmptyResultError;

            var criteria = new DeleteSubAccCriteria
            {
                SubAccountID = userId
            };

            var result = await baseService.ExecuteNonQueryAsync(criteria);

            if (result != null)
            {
                resultMessage = result.IsSuccessful ? ViewResources.SuccessfulMessage : result.Message;
            }

            return resultMessage;
        }

        #endregion Del

        #region AssignLicensee

        [HttpGet]
        public async Task<IActionResult> AssignLicensee(int id, string username)
        {
            if (id == 0)
            {
                return RedirectToAction("List", "SubAccount");
            }

            if (!HasEditPermission(currentSession.UserRole))
            {
                return this.NoAuthorization();
            }

            ViewBag.CurrentUser = " \"" + username + "\"";

            // System
            var systems = await baseService.GetDataAsync<SystemDto>(new GetOptionalSystemBySubAccountIDCriteria { UserID = currentSession.UserId, SubAccountID = id });
            ViewBag.SystemListID = systems.Select(x => new SelectListItem { Value = x.SystemId.ToString(), Text = x.SystemName }).ToList();

            var criteria = new AssignLicensee_SubAccCriteria
            {
                UserID = id,
                SystemList = (await baseService.GetDataAsync<SystemDto>(new GetSystemByUserIDCriteria { UserID = id, IgnoreIsActive = false }))
                    .Select(item => item.SystemId.ToString()).ToArray()
            };

            if (!HasEditPermission(currentSession.UserRole))
            {
                ViewBag.HasEditPermission = false;
                ViewBag.Error = ViewResources.You_have_no_authorization;
            }

            return View(criteria);
        }

        [HttpPost]
        public async Task<IActionResult> AssignLicensee(AssignLicensee_SubAccCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;

            if (!HasEditPermission(currentSession.UserRole))
            {
                ViewBag.Success = false;
                ViewBag.Error = ViewResources.You_have_no_authorization;
                return View(criteria);
            }

            // System
            var systems = await baseService.GetDataAsync<SystemDto>(new GetOptionalSystemBySubAccountIDCriteria
            {
                UserID = currentSession.UserId,
                SubAccountID = criteria.UserID
            });
            ViewBag.SystemListID = systems.Select(x => new SelectListItem { Value = x.SystemId.ToString(), Text = x.SystemName }).ToList();

            if (string.IsNullOrEmpty(criteria.ListSystemId))
            {
                ViewBag.Success = false;
                ViewBag.Error = ViewResources.Validate_Licensee_Require;
                return View(criteria);
            }

            var result = await baseService.ExecuteNonQueryAsync(criteria);
            if (result != null && result.IsSuccessful)
            {
                ViewBag.Success = true;
            }

            ViewBag.CurrentUser = " \"" + criteria.UserName + "\"";
            return View(criteria);
        }

        private static bool HasEditPermission(int userRole)
        {
            return userRole != (byte)Core.Enums.UserRole.LicenseeSubAccount;
        }

        #endregion AssignLicensee

        #region Private methods

        private async Task ViewBagFlags()
        {
            var licensee = await baseService.GetDataEntityAsync<LicenseeProfile>(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = currentSession.SystemId });
            if (licensee != null)
            {
                var oddStyles = licensee.OddsType;

                ViewBag.OddsTypeFlag = licensee.OddsTypeFlag;
                ViewBag.OddsStyleFlag = licensee.OddsRegionFlag;
                ViewBag.LanguagesFlag = licensee.LanguageListFlag;
                ViewBag.SportsFlag = licensee.SportIdListFlag;
                ViewBag.CountriesFlag = licensee.CountryCodeListFlag;
                ViewBag.MarketsFlag = licensee.BetTypeIdListFlag && (!string.IsNullOrWhiteSpace(oddStyles) && oddStyles != "8");
            }
        }

        #endregion Private methods
    }
}
