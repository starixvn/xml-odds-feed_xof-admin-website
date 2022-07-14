namespace Fanex.OddsFeed.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
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

    [IsAuthorize]
    public class SubAccountController : BaseController
    { 
        private readonly ISubAccService subAccService;

        public SubAccountController() : this(null)
        {
        }

        public SubAccountController(ISubAccService subAccService)
        {
            this.subAccService = subAccService ?? new SubAccService();
        }

        #region Add

        [HttpGet]
        public ActionResult Add()
        {
            if (!HasEditPermission(currentSession.UserRole))
            {
                return this.NoAuthorization();
            }

            var systems = baseService.GetData<SystemDto>(new GetSystemByUserIDCriteria { UserID = currentSession.UserId, IgnoreIsActive = IsAccountManagerRole() });
            ViewBag.SystemList = systems.Select(x => new SelectListItem { Value = x.SystemId.ToString(), Text = x.SystemName }).ToList();
            ViewBagFlags();
            return View();
        }

        [HttpPost]
        public JsonResult Add(InsertSubAccCriteria criteria)
        {
            criteria.SystemID = currentSession.SystemId;
            criteria.Creator = currentSession.UserId;
            criteria.RoleID = (byte)Constant.UserRole.LicenseeSubAccount;
            criteria.Password = Cryptography.CfsEnCode(HttpUtility.HtmlEncode(criteria.Password.Trim()));
            criteria.FirstName = criteria.FirstName ?? string.Empty;
            criteria.LastName = criteria.LastName ?? string.Empty;

            CommandResult result = subAccService.Insert(criteria);
            if (result.IsSuccessful)
            {
                result.Message = ViewResources.ActionResult_AddSubAccountSuccess;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckExistSubAccount(InsertSubAccCriteria criteria)
        {
            var result = new CommandResult { Code = 0 };
            var users = baseService.GetData<User>(new CheckExistSubAccountCriteria
            {
                Username = string.IsNullOrEmpty(criteria.UserName) ? string.Empty : criteria.UserName,
                PageIndex = 1,
                PageSize = 1,
                RoleId = (byte)Constant.UserRole.LicenseeSubAccount
            });

            if (users != null && users.Any())
            {
                result.Code = 1;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Add

        #region Get

        [HttpGet]
        public ActionResult List()
        {
            if (!HasEditPermission(currentSession.UserRole))
            {
                return this.NoAuthorization();
            }

            return View("SubAccList");
        }

        public JsonResult GetSubAccountList(GetSubAccByNameCriteria criteria)
        {
            criteria.SystemId = currentSession.SystemId;
            criteria.Username = (criteria.Username == null || criteria.Username == "Username") ? string.Empty : criteria.Username.Trim();
            criteria.RoleId = (byte)Constant.UserRole.LicenseeSubAccount;
            criteria.PageIndex++;
            var dr = baseService.GetData(criteria);
            var result = FanexGridHelper.DataForFanexGrid_PreventContainQuote(dr);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string ListExistedSubAccount(string username)
        {
            var criteria = new GetTop10SubAccCriteria
            {
                SystemId = currentSession.SystemId,
                RoleId = (byte)Constant.UserRole.LicenseeSubAccount,
                Username = username
            };
            var users = baseService.GetData<User>(criteria);
            return string.Join(",", users.Select(x => x.Username));
        }

        #endregion Get

        #region Edit

        public ActionResult EditSubAccInfo(int id, string userName)
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

            IList<User> users = subAccService.GetBySystemId(currentSession.SystemId);
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
        public JsonResult EditSubAccInfo(UpdateSubAccCriteria criteria)
        {
            var result = new CommandResult();
            if (ModelState.IsValid)
            {
                criteria.FirstName = criteria.FirstName ?? string.Empty;
                criteria.LastName = criteria.LastName ?? string.Empty;

                result = baseService.ExecuteNonQuery(criteria);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string EditActive(int userId, bool isActive)
        {
            var criteria = new UpdateActiveSubAccCriteria
            {
                SubAccountID = userId,
                IsActive = isActive
            };
            CommandResult result = baseService.ExecuteNonQuery(criteria);

            return result.IsSuccessful ? ViewResources.SuccessfulMessage : result.Message;
        }

        [HttpGet]
        public ActionResult ResetPassword(int id, string userName)
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
        public JsonResult ResetPassword(ResetPassSubAccCriteria criteria)
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
                    result = baseService.ExecuteNonQuery(criteria);

                    if(result.IsSuccessful)
                    {
                        result.Message = ViewResources.ActionResult_UpdatePasswordSuccess;
                    }
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditPermission(int id, string userName)
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

            PermissionFlag result = subAccService.GetUserPermission(id, systemId);
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

            ViewBagFlags();

            return View(criteria);
        }

        [HttpPost]
        public ActionResult EditPermission(EditPermissionSubAccCriteria criteria)
        {
            if (ModelState.IsValid)
            {
                criteria.UpdatedBy = currentSession.UserId;
                var result = baseService.ExecuteNonQuery(criteria);
                if (result != null && result.IsSuccessful)
                {
                    ViewBag.Success = true;
                }
                else
                {
                    ViewBag.Error = result != null ? result.Message : ViewResources.ActionResult_EmptyResultError;
                }
            }

            ViewBagFlags();

            ViewBag.CurrentUser = " of \"" + criteria.SubAccountName + "\"";
            return View(criteria);
        }

        #endregion Edit

        #region Del

        public string Delete(int userId)
        {
            var resultMessage = ViewResources.ActionResult_EmptyResultError;

            var criteria = new DeleteSubAccCriteria
            {
                SubAccountID = userId
            };

            var result = baseService.ExecuteNonQuery(criteria);

            if(result != null)
            {
                resultMessage = result.IsSuccessful ? ViewResources.SuccessfulMessage : result.Message;
            }

            return resultMessage;
        }

        #endregion Del

        #region AssignLicensee

        [HttpGet]
        public ActionResult AssignLicensee(int id, string username)
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
            var systems = baseService.GetData<SystemDto>(new GetOptionalSystemBySubAccountIDCriteria { UserID = currentSession.UserId, SubAccountID = id });
            ViewBag.SystemListID = systems.Select(x => new SelectListItem { Value = x.SystemId.ToString(), Text = x.SystemName }).ToList();

            var criteria = new AssignLicensee_SubAccCriteria
            {
                UserID = id,
                SystemList = baseService.GetData<SystemDto>(new GetSystemByUserIDCriteria { UserID = id, IgnoreIsActive = false })
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
        public ActionResult AssignLicensee(AssignLicensee_SubAccCriteria criteria)
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
            var systems = baseService.GetData<SystemDto>(new GetOptionalSystemBySubAccountIDCriteria
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

            var result = baseService.ExecuteNonQuery(criteria);
            if (result != null && result.IsSuccessful)
            {
                ViewBag.Success = true;
            }

            ViewBag.CurrentUser = " \"" + criteria.UserName + "\"";
            return View(criteria);
        }

        private static bool HasEditPermission(int userRole)
        {
            return userRole != (byte)Constant.UserRole.LicenseeSubAccount;
        }

        #endregion AssignLicensee

        #region Private methods

        private void ViewBagFlags()
        {
            var licensee = baseService.GetDataEntity<LicenseeProfile>(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = currentSession.SystemId });
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