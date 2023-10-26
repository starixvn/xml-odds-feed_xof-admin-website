using System.Drawing;
using System.Drawing.Imaging;
using Fanex.OddsFeed.Admin.Utility;
using Fanex.OddsFeed.Core.Service.Interface;
using Fanex.OddsFeed.Core.Web.Context;
using Fanex.OddsFeed.Core.Web.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Fanex.OddsFeed.Admin.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IActionContextAccessor _actionContext;
        public LoginController(ICurrentContext currentContext, 
            ICurrentSession currentSession,
            IActionContextAccessor actionContext,
            IBaseService baseService) : base(currentContext, currentSession, baseService)
        {
            _actionContext = actionContext;
        }

        public IActionResult ShowCaptchaImage()
        {
            var bmp = new Bitmap(70, 25);
            var g = Graphics.FromImage(bmp);
            g.Clear(Color.CadetBlue);
            var randomString = LoginCaptcha.GetCaptchaString(4);
            _actionContext.ActionContext.HttpContext.Session.SetString("captchastring", randomString);
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

        public ActionResult LogOut()
        {
            DoSignOut();
            return RedirectToAction("Index");
        }

        public void DoSignOut()
        {
            //baseService.ExecuteNonQuery(new LogOutUserCriteria
            //{
            //    OnUserId = currentSession.OnUserId
            //});
            currentSession.ClearAll();
        }
    }
}
