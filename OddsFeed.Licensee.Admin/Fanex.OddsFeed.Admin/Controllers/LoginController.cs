namespace Fanex.OddsFeed.Admin.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.OddsFeed.Admin.Utility;
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Service.Impl;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web;

    public class LoginController : BaseController
    {
        #region members

        private readonly IUserService _userService;

        #endregion

        #region ctors

        public LoginController() : this(null)
        {
        }

        public LoginController(IUserService userService)
        {
            _userService = userService ?? new UserService();
        }

        #endregion

        #region sign in

        public LoginCaptcha ShowCaptchaImage()
        {
            return new LoginCaptcha();
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.FullDomainName = currentContext.FullDomainName;
            ViewBag.ValidationCodePath = currentContext.FullDomainName + "/login/ShowCaptchaImage";
            return View();
        }

        [HttpPost]
        public JsonResult Index(UserLoginUserCriteria criteria)
        {
            var actionResult = new OddsFeedActionResult();

            var username = criteria.UserName;
            var password = criteria.UserPwd;
            var validate = criteria.Validate;

            ViewBag.FullDomainName = currentContext.FullDomainName;
            ViewBag.ValidationCodePath = currentContext.FullDomainName + "/login/ShowCaptchaImage";

            if (HttpContext.Session["captchastring"] != null && validate != HttpContext.Session["captchastring"].ToString())
            {
                actionResult.IsSuccess = false;
                actionResult.ResultMessage = ViewResources.Error_InvalCaptcha;
                return Json(actionResult, JsonRequestBehavior.AllowGet);
            }

            if (!username.IsNullOrTrimEmpty() && !password.IsNullOrTrimEmpty())
            {
                CustomizeDataBeforeLogin(ref criteria, password);

                var user = _userService.UserLogin(ref criteria) as User;
                if (user != null)
                {
                    actionResult.IsSuccess = false;
                    CustomizeDataAfterLogin(criteria, user, ref actionResult);
                }
            }
            else
            {
                actionResult.IsSuccess = false;
                actionResult.ResultMessage = ViewResources.Error_InvalidInput;
            }

            return Json(actionResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region sign out

        public ActionResult LogOut()
        {
            DoSignOut();

            return RedirectToAction("Index");
        }

        public void DoSignOut()
        {
            baseService.ExecuteNonQuery(new LogOutUserCriteria
            {
                OnUserId = currentSession.OnUserId
            });
            currentSession.ClearAll();
        }

        #endregion

        #region re check in

        [HttpPost]
        public ActionResult ReCheckLogin()
        {
            var criteria = new UserReCheckInUserCriteria
            {
                OnUserId = Convert.ToInt32(currentSession.OnUserId)
            };
            _userService.ReUserCheckIn(ref criteria);

            return Json(criteria.IsKickedOut ? "1" : "0");
        }

        #endregion

        #region Update Password

        [HttpGet]
        public ActionResult UpdateUserPassword()
        {
            ModelState.Clear();
            var criteria = new UpdatePasswordUserCriteria
            {
                UserID = currentSession.UserId
            };

            if (Request.UrlReferrer != null && Request.UrlReferrer.Segments.Length >= 4)
            {
                var refUrlLength = Request.UrlReferrer.Segments.Length;
                ViewBag.RefController = Request.UrlReferrer.Segments[refUrlLength - 2];
                ViewBag.RefAction = Request.UrlReferrer.Segments[refUrlLength - 1];
            }
            else
            {
                ViewBag.RefController = string.Empty;
                ViewBag.RefAction = string.Empty;
            }

            return View(criteria);
        }

        [HttpPost]
        public JsonResult UpdateUserPasswordPost(UpdatePasswordUserCriteria criteria)
        {
            var actionResult = new OddsFeedActionResult();
            if (ModelState.IsValid)
            {
                criteria.OldPassword = Cryptography.CfsEnCode(HttpUtility.HtmlEncode(criteria.OldPassword.Trim()));
                criteria.Password = Cryptography.CfsEnCode(HttpUtility.HtmlEncode(criteria.Password.Trim()));
                criteria.ConfirmPassword = Cryptography.CfsEnCode(HttpUtility.HtmlEncode(criteria.ConfirmPassword.Trim()));
                var result = baseService.ExecuteNonQuery(criteria);
                if (result.Code == 0)
                {
                    actionResult.IsSuccess = true;
                    actionResult.ResultMessage = ViewResources.ActionResult_UpdatePasswordSuccess;
                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.ResultMessage = GetMessage(result.Code);
                }
            }

            return Json(actionResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private methods

        private void CustomizeDataBeforeLogin(ref UserLoginUserCriteria criteria, string password)
        {
            password = Cryptography.CfsEnCode(HttpUtility.HtmlEncode(password));
            criteria.UserPwd = password;
            criteria.LoginDomain = currentContext.DomainName;
            criteria.LoginIP = currentContext.IP;
            criteria.NetSessionID = Session.SessionID;
            criteria.LoginDate = DateTime.Now;
            criteria.BrowserName = currentContext.BrowserName;
            criteria.CountryCode = currentContext.CountryCode;
            criteria.Domain = currentContext.DomainName;
            criteria.Platform = currentContext.Platform;
        }

        private void CustomizeDataAfterLogin(UserLoginUserCriteria criteria, User user, ref OddsFeedActionResult actionResult)
        {
            switch (criteria.ErrCode)
            {
                case 100:
                    actionResult.ResultMessage = criteria.ErrMsg; //// ViewResources.Invalid_Username_or_Password;
                    break;
                case 101:
                    actionResult.ResultMessage = ViewResources.Invalid_Username_or_Password;
                    break;
                case 102:
                    actionResult.ResultMessage = ViewResources.This_user_is_not_active;
                    break;
                case 103:
                    actionResult.ResultMessage = ViewResources.This_user_has_no_permission;
                    break;
                case 104:
                    actionResult.ResultMessage = ViewResources.This_user_are_not_authorized_to_use_this_system;
                    break;
                default: ////login successfully
                    actionResult.IsSuccess = true;
                    SaveUserSession(criteria, user);
                    break;
            }
        }

        private void SaveUserSession(UserLoginUserCriteria criteria, User user)
        {
            ////get systemId
            var systemIdNameList = criteria.SystemIdNameList.Split(';');
            var firstSystem = systemIdNameList[0]?.Split(',');
            var systemId = firstSystem != null && !string.IsNullOrEmpty(firstSystem[0]) ? Convert.ToByte(firstSystem[0]) : Convert.ToByte(0);
            var systemName = firstSystem != null && !string.IsNullOrEmpty(firstSystem[1]) ? Convert.ToString(firstSystem[1]) : string.Empty;

            var criteriaProfile = new UserGetProfileCriteria
            {
                UserId = user.Id,
                SystemId = systemId,
                IP = currentContext.IP,
                NetSessionID = Session.SessionID
            };

            _userService.UserGetProfile(ref criteriaProfile);

            ////Save user session
            currentSession.UserId = user.Id;
            currentSession.UserRole = user.RoleID;
            currentSession.Username = user.Username;
            currentSession.OnUserId = criteriaProfile.OnUserId;
            currentSession.SystemId = systemId;
            currentSession.SystemName = systemName;
            currentSession.SystemList = criteria.SystemIdNameList;

            currentSession.SessionId = Session.SessionID;
        }

        #endregion
    }
}