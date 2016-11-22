// --------------------------------------------------------------------------
// <copyright file="LoginCaptcha.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2012.12.12">Create</change>
// </history>

namespace Fanex.OddsFeed.Admin.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;    
    using System.Drawing.Imaging;    
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Class LoginCaptcha.
    /// </summary>
    public class LoginCaptcha : ActionResult
    {
        /// <summary>
        /// Gets the captcha string.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns>The captcha string.</returns>
        public string GetCaptchaString(int length)
        {
            int intZero = '0';
            int intNine = '9';
            ////int intA = 'A';
            ////int intZ = 'Z';
            var intCount = 0;
            var intRandomNumber = 0;
            var strCaptchaString = string.Empty;

            var random = new Random(System.DateTime.Now.Millisecond);

            while (intCount < length)
            {
                intRandomNumber = random.Next(intZero, intNine);
                /* || (intRandomNumber >= intA) && (intRandomNumber <= intZ)))*/
                if ((intRandomNumber >= intZero) && (intRandomNumber <= intNine))                    
                {
                    strCaptchaString = strCaptchaString + (char)intRandomNumber;
                    intCount = intCount + 1;
                }
            }

            return strCaptchaString;
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult" /> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed.
        /// The context information includes the controller, HTTP content, request context, and route data.</param>
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