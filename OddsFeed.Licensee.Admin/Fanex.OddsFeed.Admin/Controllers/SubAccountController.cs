namespace Fanex.OddsFeed.Admin.Controllers
{
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
    using static Fanex.OddsFeed.Core.Business.Constant;

    [IsAuthorize]
    public class SubAccountController : BaseController
    { 
        private readonly ISubAccService _subAccService;

        public SubAccountController()
            : this(null)
        {
        }

        public SubAccountController(ISubAccService subAccService)
        {
            _subAccService = subAccService ?? new SubAccService();
        }

        #region Add

        [HttpGet]
        public ActionResult Add()
        {
            if (!HasEditPermission(currentSession.UserRole))
            {
                return this.NoAuthorization();
            }

            ViewBag.SystemList = baseService.GetSelectList(new GetSystemByUserIDCriteria { UserID = currentSession.UserId, IgnoreIsActive = IsAccountManagerRole() });
            ViewBagFlags();
            return View();
        }

        [HttpPost]
        public JsonResult Add(InsertSubAccCriteria criteria)
        {
            var actionResult = new OddsFeedActionResult();
            criteria.SystemID = currentSession.SystemId;
            criteria.Creator = currentSession.UserId;
            criteria.RoleID = (byte)UserRole.LicenseeSubAccount;
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
                if (result != null)
                {
                    actionResult.IsSuccess = false;
                    actionResult.ResultMessage = result.Message;
                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.ResultMessage = ViewResources.ActionResult_EmptyResultError;
                }
            }

            ViewBag.SystemList = baseService.GetSelectList(new GetSystemCriteria());
            return Json(actionResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckExistSubAccount(InsertSubAccCriteria criteria)
        {
            var checkResult = new OddsFeedActionResult();
            var res = this._subAccService.GetSubAccountListReturnDataCollection(new CheckExistSubAccountCriteria
            {
                Username = string.IsNullOrEmpty(criteria.UserName) ? string.Empty : criteria.UserName,
                PageIndex = 1,
                PageSize = 1,
                RoleId = (byte)UserRole.LicenseeSubAccount
            });

            if (res.Data.Tables.Count > 0 &&
                res.Data.Tables[0].Rows.Count > 0)
            {
                checkResult.IsSuccess = false;
            }

            return Json(checkResult, JsonRequestBehavior.AllowGet);
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
            criteria.RoleId = (byte)UserRole.LicenseeSubAccount;
            criteria.PageIndex++;
            var dr = _subAccService.GetSubAccountListReturnDataCollection(criteria);
            var result = FanexGridHelper.DataForFanexGrid_PreventContainQuote(dr);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string ListExistedSubAccount(string username)
        {
            var criteria = new GetTop10SubAccCriteria
            {
                SystemId = currentSession.SystemId,
                RoleId = (byte)UserRole.LicenseeSubAccount,
                Username = username
            };

            var top10AffList = _subAccService.GetTop10SubAccountReturnDataSet(criteria).Tables[0];
            return DataForAutoComplete(top10AffList, "username");
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

            var rawData = _subAccService.GetBySystemId(currentSession.SystemId);
            var rawInfo = rawData.ToList().FirstOrDefault(user => user.Id == id);
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

        [HttpPost]
        public JsonResult EditSubAccInfo(UpdateSubAccCriteria criteria)
        {
            var actionResult = new OddsFeedActionResult();
            if (ModelState.IsValid)
            {
                criteria.FirstName = criteria.FirstName ?? string.Empty;
                criteria.LastName = criteria.LastName ?? string.Empty;

                var result = baseService.ExecuteNonQuery(criteria);
                if (result == null || !result.IsSuccessful)
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

        public string EditActive(int userId, bool isActive)
        {
            var resultMessage = ViewResources.ActionResult_EmptyResultError;

            var criteria = new UpdateActiveSubAccCriteria
            {
                SubAccountID = userId,
                IsActive = isActive
            };

            var result = baseService.ExecuteNonQuery(criteria);

            if (result != null)
            {
                resultMessage = result.IsSuccessful ? ViewResources.SuccessfulMessage : result.Message;
            }

            return resultMessage;
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
                    var result = baseService.ExecuteNonQuery(criteria);

                    if(result != null)
                    {
                        actionResult.IsSuccess = result.IsSuccessful;
                        actionResult.ResultMessage = result.IsSuccessful 
                            ? ViewResources.ActionResult_UpdatePasswordSuccess 
                            : result.Message;
                    }
                    else
                    {
                        actionResult.IsSuccess = false;
                        actionResult.ResultMessage = ViewResources.ActionResult_EmptyResultError;
                    }
                }
            }

            return Json(actionResult, JsonRequestBehavior.AllowGet);
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

            var result = _subAccService.GetUserPermission(id, systemId);
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

            ////System
            ViewBag.SystemListID = baseService.GetSelectList(new GetOptionalSystemBySubAccountIDCriteria { UserID = currentSession.UserId, SubAccountID = id });

            var criteria = new AssignLicensee_SubAccCriteria
            {
                UserID = id,
                SystemList = baseService.GetSelectList(new GetSystemByUserIDCriteria { UserID = id, IgnoreIsActive = false }).Select(item => item.Value).ToArray()
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

            ////System
            ViewBag.SystemListID = baseService.GetSelectList(new GetOptionalSystemBySubAccountIDCriteria
            {
                UserID = currentSession.UserId,
                SubAccountID = criteria.UserID
            });

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
            return userRole != (int)Constant.UserRole.LicenseeSubAccount;
        }

        #endregion AssignLicensee

        #region Private methods

        private void ViewBagFlags()
        {
            var flags = baseService.GetDataSet(new GetLicenseeProfileByLicenseeIDCriteria { SystemID = currentSession.SystemId });
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

        #endregion Private methods
    }
}