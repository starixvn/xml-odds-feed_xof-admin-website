// --------------------------------------------------------------------------
// <copyright file="Utilities.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>
namespace Nexcel.OddsFeed.Library
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Web;
    using Fanex.Common;
    using Sunplus.UMControllerLibrary;

    /// <summary>
    /// Class Utilities.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Reads the text file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>System String.</returns>
        public static string ReadTextFile(string filePath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    return sr.ReadToEnd();
                }
            }
            catch
            {
                return string.Empty;
            }
        }
       
        /// <summary>
        /// Get IP address.
        /// </summary>
        /// <returns>The IP address.</returns>
        public static string GetIpAddressOfClient()
        {
            string clientIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(clientIp))
            {
                string[] forwardedIps = clientIp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                clientIp = forwardedIps[forwardedIps.Length - 1];
            }
            else
            {
                clientIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (clientIp == "::1")
            {
                clientIp = "127.0.0.1";
            }

            return clientIp;

            ////var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
            ////            ?? HttpContext.Current.Request.UserHostAddress;
            ////if (ip == "::1")
            ////{
            ////    ip = "127.0.0.1";
            ////}

            ////return ip;
        }
     
        public static IList<int> GetIntList(string stringArray)
        {
            string[] separator = { "," };
            var items = stringArray.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            return items.Select(item => int.Parse(item)).ToList();
        }
     
        public static IList<string> GetStringIList(string stringArray, string sep)
        {
            string[] separator = { sep };
            return stringArray.Split(separator, StringSplitOptions.None).ToList();
        }
    
        public static IList<string> GetStringIList(string stringArray, bool trim = false)
        {
            string[] separator = { "," };
            var items = stringArray.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            return trim == true ? items.Select(item => item.Trim()).ToList() : items.ToList();          
        }
    
        public static decimal ToHkOdds(decimal malayOdds)
        {
            return malayOdds >= 0
                       ? malayOdds
                       : Utilities.Floor(1 / Math.Abs(malayOdds));
        }
           
        public static decimal ToDecimalOddsFromHkOdds(decimal hongKongOdds)
        {
            return hongKongOdds + 1;
        }

        public static decimal ToDecimalOddsFromMalayOdds(decimal malayOdds)
        {
            ////as malayOdd always <= 1
            if (malayOdds <= 1) 
            {
                return ToDecimalOddsFromHkOdds(ToHkOdds(malayOdds));
            }

            return malayOdds;
        }
     
        public static float ToIndoOdds(int malayOdds, float spread)
        {
            return malayOdds > 0 ? -1 / Math.Abs(malayOdds) : 1 / Math.Abs(malayOdds);
        }

        /// <summary>
        /// Converts the odds.
        /// </summary>
        /// <param name="oddsRegion">The odds region.</param>
        /// <param name="decimalOdds">The malay odds.</param>
        /// <returns>System Decimal.</returns>
        public static decimal ConvertOdds(int oddsRegion, decimal decimalOdds)
        {
            switch (oddsRegion)
            {
                case 1:
                    return DecimalToMalay(decimalOdds);  //// MalayOdds
                case 2:
                    return DecimalToHK(decimalOdds); //// HongKongOdds
                case 3:
                    return Utilities.Floor(decimalOdds); ////DecimalOdds
                case 4:
                    return DecimalToIndo(decimalOdds); ////IndoOdds
                case 5:
                    return DecimalToUS(decimalOdds); ////USOdds
                default:
                    return Utilities.Floor(decimalOdds);
            }
        }
   
        public static string RealSp(string sp)
        {
            return bool.Parse(ConfigurationManager.AppSettings["SP.Live"])
                       ? sp
                       : sp + ConfigurationManager.AppSettings["SP.Suffix"];
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="from">From address.</param>
        /// <param name="to">To address.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The content body.</param>
        /// <returns><c>true</c> If XXXX, <c>false</c> otherwise.</returns>
        public static bool SendEmail(string from, string to, string subject, string body)
        {
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["SmtpClient.EnableSsl"]);
            client.Host = ConfigurationManager.AppSettings["SmtpClient.Host"];
            client.Port = int.Parse(ConfigurationManager.AppSettings["SmtpClient.Port"]);

            //// setup Smtp authentication
            NetworkCredential credentials = new System.Net.NetworkCredential(
                                                                ConfigurationManager.AppSettings["SmtpClient.Account"],
                                                                ConfigurationManager.AppSettings["SmtpClient.Password"]);

            client.UseDefaultCredentials = false;
            client.Credentials = credentials;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailAddress _from = new MailAddress(from, from, System.Text.Encoding.UTF8);
            MailAddress _to = new MailAddress(to, to, System.Text.Encoding.UTF8);
            MailMessage message = new MailMessage(_from, _to);
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.Subject = subject;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.Body = body;
            try
            {
                client.SendAsync(message, Guid.NewGuid());
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets down time.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool GetDownTime(out string startTime, out string endTime)
        {
            startTime = endTime = null;
            DateTime now = DateTime.Now;

            var underMaintainStartTime = Converter.ToDateTime(ConfigurationManager.AppSettings["UM_StartTime"]);
            var underMaintainEndTime = Converter.ToDateTime(ConfigurationManager.AppSettings["UM_EndTime"]);
            int timeCacheUM = 10; ////second

            string urlDownTime = Converter.ToString2(ConfigurationManager.AppSettings["LBUrl"]);
            if (now >= underMaintainStartTime && now <= underMaintainEndTime)
            {
                startTime = underMaintainStartTime.ToString("hh:mm tt");
                endTime = underMaintainEndTime.ToString("hh:mm tt");
                return true;
            }

            if (string.IsNullOrEmpty(urlDownTime))
            {
                return false;
            }

            bool isUM = false;
            if (HttpContext.Current.Cache["IsUM"] == null)
            {
                var underMaitainCtrl = new UMBusiness();
                isUM = underMaitainCtrl.isUnderMaintainence(Sites.OddsFeed, urlDownTime, ref startTime, ref endTime);

                ////Cache Information which is gotten from Service
                HttpContext.Current.Cache.Insert("IsUM", isUM, null, DateTime.Now.AddSeconds(timeCacheUM), TimeSpan.Zero);
                HttpContext.Current.Cache.Insert("StartTime", startTime, null, DateTime.Now.AddSeconds(timeCacheUM), TimeSpan.Zero);
                HttpContext.Current.Cache.Insert("EndTime", endTime, null, DateTime.Now.AddSeconds(timeCacheUM), TimeSpan.Zero);
            }
            else
            {
                isUM = Converter.ToBool(HttpContext.Current.Cache["IsUM"]);
                startTime = Converter.ToString2(HttpContext.Current.Cache["StartTime"]);
                endTime = Converter.ToString2(HttpContext.Current.Cache["EndTime"]);
            }

            return isUM;
        }

        #region Convert Odds From Decimal

        /// <summary>
        /// Decimals to us.
        /// </summary>
        /// <param name="dec">The decimal.</param>
        /// <returns>System Decimal.</returns>
        public static decimal DecimalToUS(decimal dec)
        {
            decimal usa;            
            if (dec <= 1)
            {
                usa = 0;
            }
            else if (dec < 2)
            {
                usa = -100 / (dec - 1);
            }
            else
            {
                usa = (dec - 1) * 100;
            }
            
            return Utilities.Floor(usa);
        }

        /// <summary>
        /// Decimals to Hong Kong.
        /// </summary>
        /// <param name="dec">The decimal.</param>
        /// <returns>System Decimal.</returns>
        public static decimal DecimalToHK(decimal dec)
        {
            decimal hongKong;            
            if (dec <= 1)
            {
                hongKong = 0;
            }
            else
            {
                hongKong = dec - 1;
            }
            
            return Utilities.Floor(hongKong);            
        }

        /// <summary>
        /// Decimals to indo.
        /// </summary>
        /// <param name="dec">The decimal.</param>
        /// <returns>System Decimal.</returns>
        public static decimal DecimalToIndo(decimal dec)
        {
            decimal indo;            
            if (dec <= 1)
            {
                indo = 0;
            }
            else if (dec >= 2)
            {
                indo = dec - 1;
            }
            else
            {
                indo = 1 / (1 - dec);
            }
            
            return Utilities.Floor(indo);
        }

        /// <summary>
        /// Decimals to malay.
        /// </summary>
        /// <param name="dec">The decimal.</param>
        /// <returns>System Decimal.</returns>
        public static decimal DecimalToMalay(decimal dec)
        {
            decimal malay;            
            if (dec <= 1)
            {
                malay = 0;
            }
            else if (dec <= 2)
            {
                malay = dec - 1;
            }
            else
            {
                malay = 1 / (1 - dec);
            }
            
            return Utilities.Floor(malay);
        }

        #endregion
   
        public static decimal Floor(decimal value, int decimalPlaces = 2)
        {
            decimal adjustment = (decimal)Math.Pow(10, decimalPlaces);
            return Math.Floor(value * adjustment) / adjustment;
        }
    }
}
