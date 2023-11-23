namespace Fanex.OddsFeed.Controllers
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using Fanex.Data;
    using Fanex.OddsFeed.Admin.Utility;
    using Fanex.OddsFeed.Core.Context;
    using Fanex.OddsFeed.Core.Session;
    using Fanex.OddsFeed.Criteria.User;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Resources;
    using Fanex.OddsFeed.Services.Interface;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class LoginController : BaseController
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserService userService;
        public LoginController(ICurrentContext currentContext, 
            ICurrentSession currentSession,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IBaseService baseService,
            AppSettings appSettings) : base(currentContext, currentSession, baseService, appSettings)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userService = userService;
        }

        public IActionResult ShowCaptchaImage()
        {
            #pragma warning disable CA1416 // Validate platform compatibility
            var bmp = new Bitmap(70, 25);
            var g = Graphics.FromImage(bmp);
            g.Clear(Color.CadetBlue);
            var randomString = LoginCaptcha.GetCaptchaString(4);
            currentSession.Captcha = randomString;
            g.DrawString(randomString, new Font("Courier New", 16), new SolidBrush(Color.WhiteSmoke), 2, 2);
            using (var stream = new MemoryStream())
            {
                bmp.Save(stream, ImageFormat.Jpeg);
                var imageData = stream.ToArray();
                return File(imageData, "image/jpeg");
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserLoginUserCriteria criteria)
        {
            CommandResult result = new CommandResult();

            var username = criteria.UserName;
            var password = criteria.UserPwd;
            var validate = criteria.Validate;

            if (currentSession.Captcha != null && validate != currentSession.Captcha)
            {
                result.Message = ViewResources.Error_InvalCaptcha;
                return Json(result);
            }

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                CustomizeDataBeforeLogin(ref criteria, password);
                User user = await userService.UserLogin(criteria);
                if (user != null)
                {
                    await CustomizeDataAfterLogin(criteria, user, result);
                }
            }
            else
            {
                result.Message = ViewResources.Error_InvalidInput;
            }

            return Json(result);
        }

        public async Task<ActionResult> LogOut()
        {
            await DoSignOut();

            return RedirectToAction("Index");
        }

        public async Task DoSignOut()
        {
            var criteria = new LogOutUserCriteria 
            {
                OnUserId = currentSession.OnUserId
            };

            await baseService.GetDataEntityAsync<CommandResult>(criteria);
            currentSession.ClearAll();
        }

        [HttpPost]
        public async Task<IActionResult> ReCheckLogin()
        {
            var criteria = new UserReCheckInUserCriteria
            {
                OnUserId = currentSession.OnUserId
            };
            await userService.ReUserCheckIn(criteria);

            return Json(criteria.IsKickedOut ? "1" : "0");
        }

        [HttpGet]
        public IActionResult UpdateUserPassword()
        {
            ModelState.Clear();
            var criteria = new UpdatePasswordUserCriteria
            {
                UserID = currentSession.UserId
            };

            var referrer = httpContextAccessor?.HttpContext?.Request.Headers["Referer"].ToString();

            if (!string.IsNullOrEmpty(referrer))
            {
                var segments = new Uri(referrer).Segments;
                if (segments.Length >= 4)
                {
                    ViewBag.RefController = segments[^2];
                    ViewBag.RefAction = segments[^1];
                }
                else
                {
                    ViewBag.RefController = string.Empty;
                    ViewBag.RefAction = string.Empty;
                }
            }

            return View(criteria);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserPassword(UpdatePasswordUserCriteria criteria)
        {
            var result = new CommandResult();
            if (ModelState.IsValid)
            {
                criteria.OldPassword = Cryptography.EncryptPassword(criteria.OldPassword.Trim());
                criteria.Password = Cryptography.EncryptPassword(criteria.Password.Trim());
                criteria.ConfirmPassword = Cryptography.EncryptPassword(criteria.ConfirmPassword.Trim());
                result = await baseService.ExecuteNonQueryAsync(criteria);
                result.Message = result.IsSuccessful ? ViewResources.ActionResult_UpdatePasswordSuccess : GetMessage(result.Code);
            }

            return Json(result);
        }

        private void CustomizeDataBeforeLogin(ref UserLoginUserCriteria criteria, string password)
        {
            password = Cryptography.EncryptPassword(password);
            criteria.UserPwd = password;
            criteria.LoginDomain = currentContext.DomainName;
            criteria.LoginIP = currentContext.IP;
            criteria.NetSessionID = HttpContext.Session.Id;
            criteria.LoginDate = DateTime.Now;
            criteria.BrowserName = currentContext.BrowserName;
            criteria.CountryCode = currentContext.CountryCode;
            criteria.Domain = currentContext.DomainName;
            criteria.Platform = currentContext.Platform;
        }

        private async Task CustomizeDataAfterLogin(UserLoginUserCriteria criteria, User user, CommandResult result)
        {
            const byte InvalidUsernamePassword = 101;
            const byte Inactive = 102;
            const byte NoPermission = 103;
            const byte NotAuthorized = 104;
            switch (criteria.ErrCode)
            {
                case InvalidUsernamePassword:
                    result.Message = ViewResources.Invalid_Username_or_Password;
                    break;
                case Inactive:
                    result.Message = ViewResources.This_user_is_not_active;
                    break;
                case NoPermission:
                    result.Message = ViewResources.This_user_has_no_permission;
                    break;
                case NotAuthorized:
                    result.Message = ViewResources.This_user_are_not_authorized_to_use_this_system;
                    break;
                default:
                    result.Code = 0;
                    await SaveUserSession(criteria, user);
                    break;
            }
        }

        private async Task SaveUserSession(UserLoginUserCriteria criteria, User user)
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
                NetSessionID = HttpContext.Session.Id
            };

            await userService.UserGetProfile(criteriaProfile);

            // Save user session
            currentSession.UserId = user.Id;
            currentSession.UserRole = user.RoleID;
            currentSession.Username = user.Username;
            currentSession.OnUserId = criteriaProfile.OnUserId;
            currentSession.SystemId = systemId;
            currentSession.SystemName = systemName;
            currentSession.SystemList = criteria.SystemIdNameList;

            currentSession.SessionId = HttpContext.Session.Id;
        }
    }
}
