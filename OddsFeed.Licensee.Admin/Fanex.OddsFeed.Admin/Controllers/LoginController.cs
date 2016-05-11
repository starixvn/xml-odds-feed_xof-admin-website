// --------------------------------------------------------------------------
// <copyright file="LoginController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2012.8.28">Create</change>
// </history>
namespace Fanex.OddsFeed.Admin.Controllers
{
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.OddsFeed.Admin.Utility;
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Service.Impl;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Core.Web.Session;
    using ONEbook.UM.Filters;
    using System;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Class LoginController.
    /// </summary>
    public class LoginController : BaseController
    {
        #region members

        /// <summary>
        /// The _user service.
        /// </summary>
        private IUserService _userService;

        #endregion

        #region ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginController"/> class.
        /// </summary>
        public LoginController()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="currentSession">The current session.</param>
        public LoginController(
            IUserService userService,
            ICurrentSession currentSession)
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
        [UMBackdoorFilter]
        public ActionResult Index()
        {
            ViewBag.FullDomainName = CurrentContext.FullDomainName;
            ViewBag.ValidationCodePath = CurrentContext.FullDomainName + "/login/ShowCaptchaImage";
            return View();
        }

        /// <summary>
        /// User login.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Represents a JSON-formatted content of logging.</returns>
        [HttpPost]
        [CheckUMFilter]
        public JsonResult Index(UserLoginUserCriteria criteria)
        {
            var actionResult = new OddsFeedActionResult();

            string username = criteria.UserName;
            string password = criteria.UserPwd;
            string validate = criteria.Validate;

            ViewBag.FullDomainName = CurrentContext.FullDomainName;
            ViewBag.ValidationCodePath = CurrentContext.FullDomainName + "/login/ShowCaptchaImage";

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

        [CheckUMFilter]
        public ActionResult LogOut()
        {
            DoSignOut();

            return RedirectToAction("Index");
        }

        [CheckUMFilter]
        public void DoSignOut()
        {
            _userService.UserLogOut(new LogOutUserCriteria
            {
                OnUserId = CurrentSession.OnUserId
            });
            ////FormsAuthentication.SignOut();
            ////Response.Cookies.Clear();            
            CurrentSession.ClearAll();
        }

        #endregion

        #region re check in

        [HttpPost]
        [CheckUMFilter]
        public ActionResult ReCheckLogin()
        {
            UserReCheckInUserCriteria criteria = new UserReCheckInUserCriteria();
            criteria.OnUserId = Convert.ToInt32(CurrentSession.OnUserId);
            _userService.ReUserCheckIn(ref criteria);

            return Json(criteria.IsKickedOut ? "1" : "0");
        }

        #endregion

        #region Update Password
        /// <summary>
        /// Display the page for updating user password.
        /// </summary>
        /// <returns>The user password updating page.</returns>
        [HttpGet]
        [CheckUMFilter]
        public ActionResult UpdateUserPassword()
        {
            ModelState.Clear();
            var criteria = new UpdatePasswordUserCriteria();
            criteria.UserID = CurrentSession.UserId;

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

        /// <summary>
        /// Updates the user password post.
        /// </summary>
        /// <param name="criteria">The user's criteria.</param>
        /// <returns>Represents a JSON-formatted content of updating.</returns>
        [HttpPost]
        [CheckUMFilter]
        public JsonResult UpdateUserPasswordPost(UpdatePasswordUserCriteria criteria)
        {
            var actionResult = new OddsFeedActionResult();
            if (ModelState.IsValid)
            {
                criteria.OldPassword = Cryptography.CfsEnCode(HttpUtility.HtmlEncode(criteria.OldPassword.Trim()));
                criteria.Password = Cryptography.CfsEnCode(HttpUtility.HtmlEncode(criteria.Password.Trim()));
                criteria.ConfirmPassword = Cryptography.CfsEnCode(HttpUtility.HtmlEncode(criteria.ConfirmPassword.Trim()));
                var result = _userService.UpdateData(criteria);
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

        /// <summary>
        /// Customizes the data before login.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="password">The password.</param>
        private void CustomizeDataBeforeLogin(ref UserLoginUserCriteria criteria, string password)
        {
            ////var domain = new Uri(HttpContext.Request.Url.AbsoluteUri).GetLeftPart(UriPartial.Authority).Replace("www", "");
            ////string loginIp = (Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)
            ////?? new IPAddress(new byte[] { 127, 0, 0, 1 })).ToString();

            password = Cryptography.CfsEnCode(HttpUtility.HtmlEncode(password));
            criteria.UserPwd = password;
            criteria.LoginDomain = CurrentContext.DomainName;
            criteria.LoginIP = CurrentContext.IP;
            criteria.NetSessionID = Session.SessionID;
            criteria.LoginDate = DateTime.Now;
            criteria.BrowserName = CurrentContext.BrowserName;
            criteria.CountryCode = CurrentContext.CountryCode;
            criteria.Domain = CurrentContext.DomainName;
            criteria.Platform = CurrentContext.Platform;
        }

        /// <summary>
        /// Customizes the data after login.
        /// </summary>
        /// <param name="criteria">The user's criteria.</param>
        /// <param name="user">The login user.</param>
        /// <param name="actionResult">The action result.</param>
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

        /// <summary>
        /// Saves the user session.
        /// </summary>
        /// <param name="criteria">The user criteria.</param>
        /// <param name="user">The login user.</param>
        private void SaveUserSession(UserLoginUserCriteria criteria, User user)
        {
            ////get systemId
            string[] systemIdNameList = criteria.SystemIdNameList.Split(';');
            string[] firstSystem = systemIdNameList[0] != null ? systemIdNameList[0].Split(',') : null;
            byte systemId = firstSystem != null & !string.IsNullOrEmpty(firstSystem[0]) ? Convert.ToByte(firstSystem[0]) : Convert.ToByte(0);
            string systemName = firstSystem != null & !string.IsNullOrEmpty(firstSystem[1]) ? Convert.ToString(firstSystem[1]) : string.Empty;

            var criteriaProfile = new UserGetProfileCriteria
            {
                UserId = user.Id,
                SystemId = systemId,
                IP = CurrentContext.IP,
                NetSessionID = Session.SessionID
            };

            var flags = _userService.UserGetProfile(ref criteriaProfile);

            ////Save user session
            CurrentSession.UserId = user.Id;
            CurrentSession.UserRole = user.RoleID;
            CurrentSession.Username = user.Username;
            CurrentSession.OnUserId = criteriaProfile.OnUserId;
            CurrentSession.SystemId = systemId;
            CurrentSession.SystemName = systemName;
            CurrentSession.SystemList = criteria.SystemIdNameList;

            CurrentSession.SessionId = Session.SessionID.ToString();

            ////CreateAuthInfo(user);            
        }
        #endregion
    }
}