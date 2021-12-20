namespace Fanex.OddsFeed.Admin.Utility
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Web.Mvc;

    public class LoginCaptcha : ActionResult
    {
        public string GetCaptchaString(int length)
        {
            const int intZero = '0';
            const int intNine = '9';
            var intCount = 0;
            var strCaptchaString = string.Empty;

            var random = new Random(DateTime.Now.Millisecond);

            while (intCount < length)
            {
                var intRandomNumber = random.Next(intZero, intNine);
                if ((intRandomNumber >= intZero) && (intRandomNumber <= intNine))
                {
                    strCaptchaString = strCaptchaString + (char)intRandomNumber;
                    intCount = intCount + 1;
                }
            }

            return strCaptchaString;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var bmp = new Bitmap(70, 25);
            var g = Graphics.FromImage(bmp);
            g.Clear(Color.CadetBlue);
            var randomString = this.GetCaptchaString(4);
            context.HttpContext.Session["captchastring"] = randomString;
            g.DrawString(randomString, new Font("Courier", 16), new SolidBrush(Color.WhiteSmoke), 2, 2);
            var response = context.HttpContext.Response;
            response.ContentType = "image/jpeg";
            bmp.Save(response.OutputStream, ImageFormat.Jpeg);
            bmp.Dispose();
        }
    }
}