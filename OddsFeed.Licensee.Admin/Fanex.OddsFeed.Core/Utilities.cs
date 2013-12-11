// --------------------------------------------------------------------------
// <copyright file="Utilities.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.06.26">Created</change>
// </history>

namespace Fanex.OddsFeed.Core
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Logging;
    using Sunplus.UMControllerLibrary;

    /// <summary>
    /// Class Utilities.
    /// </summary>
    public static class Utilities
    {
        #region Property

        public static IList<SelectListItem> DropListStatus
        {
            get
            {
                return new List<SelectListItem>
                    {
                        new SelectListItem() { Text = "Open", Value = "opened" },
                        new SelectListItem() { Text = "Close", Value = "closed" }
                    };
            }
        }

        #endregion

        public static DateTime AddTimeToDate(DateTime dateTime, string time)
        {
            var times = time.Split(new[] { ':' });
            TimeSpan ts = new TimeSpan(int.Parse(times[0]), int.Parse(times[1]), 0);
            return dateTime.Date + ts;
        }

        public static string GetTime(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return string.Empty;
            }

            var value = (DateTime)dateTime;
            return ZeroPrefix(value.Hour) + ":" + ZeroPrefix(value.Minute);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="s">The object s.</param>
        /// <param name="t">The object t.</param>
        public static void CopyTo(this object s, object t)
        {
            foreach (var pS in s.GetType().GetProperties())
            {
                foreach (var pT in t.GetType().GetProperties())
                {
                    if (pT.Name != pS.Name)
                    {
                        continue;
                    }

                    pT.GetSetMethod().Invoke(t, new object[] { pS.GetGetMethod().Invoke(s, null) });
                }
            }
        }

        /// <summary>
        /// Reads the text file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>Object System.String.</returns>
        public static string ReadTextFile(string filePath)
        {
            try
            {
                using (var sr = new StreamReader(filePath))
                {
                    return sr.ReadToEnd();
                }
            }
            catch
            {
                ////LogService.Error(ex.Message, ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Randomize the string.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns>Object System.String.</returns>
        public static string RandomString(int length)
        {
            // returns random string of length specified with characters including: 0-9, a-z, A-Z
            var ca = new char[length];
            var random = new byte[length];

            // RNGCryptoServiceProvider is an implementation of a random number generator.
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(random); // The array is now filled with cryptographically strong random bytes.

            for (var i = 0; i < length; i++)
            {
                var found = false;
                var rand = (int)random[i];
                while (!found)
                {
                    if (((rand >= 48) && (rand <= 57)) || ((rand >= 65) && (rand <= 90)) || ((rand >= 97) && (rand <= 122)))
                    {
                        found = true;
                    }
                    else
                    {
                        // get a new random int. 
                        var single = new byte[1];
                        rng.GetBytes(single);
                        rand = single[0];
                    }
                }

                var ci = (char)rand;
                ca[i] = ci;
            }

            return new string(ca).ToLower();
        }

        /// <summary>
        /// Firsts the day of week.
        /// </summary>
        /// <param name="dayInWeek">The day in week.</param>
        /// <param name="cultureInfo">The culture information.</param>
        /// <returns>Object DateTime.</returns>
        public static DateTime FirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo = null)
        {
            if (cultureInfo == null)
            {
                cultureInfo = CultureInfo.CurrentCulture;
            }

            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
            {
                firstDayInWeek = firstDayInWeek.AddDays(-1);
            }

            return firstDayInWeek;
        }

        public static DateTime LastDayOfWeek(DateTime dayInWeek)
        {
            return DateTime.Now.AddDays(6 - Convert.ToDouble(dayInWeek.DayOfWeek));
        }

        /// <summary>
        /// Minutes the steps.
        /// </summary>
        /// <param name="step">The step for.</param>
        /// <returns>Object IList{SelectListItem}.</returns>
        public static IList<SelectListItem> MinuteSteps(int step)
        {
            var list = new List<SelectListItem>();
            var index = 60;
            while (index > 0)
            {
                list.Add(new SelectListItem()
                {
                    Text = index + "'",
                    Value = index.ToString()
                });
                index -= step;
            }

            return list;
        }

        /// <summary>
        /// Times the steps.
        /// </summary>
        /// <param name="minuteStep">The minute step.</param>
        /// <returns>Object IList{SelectListItem}.</returns>
        public static IList<SelectListItem> TimeSteps(int minuteStep)
        {
            var list = new List<SelectListItem>();
            var hour = 0;
            var minute = 0;

            while (hour < 24)
            {
                var value = ZeroPrefix(hour) + ":" + ZeroPrefix(minute);
                list.Add(new SelectListItem()
                {
                    Text = value,
                    Value = value
                });
                minute += minuteStep;
                if (minute >= 60)
                {
                    minute = 0;
                    hour++;
                }
            }

            return list;
        }

        public static string ZeroPrefix(int value)
        {
            return value < 10 ? "0" + value : value.ToString();
        }

        public static string ZeroPrefix(string value)
        {
            return int.Parse(value) < 10 ? "0" + value : value;
        }

        public static string StringArrayToString(string[] array)
        {
            if (array == null || array.Length == 0)
            {
                return string.Empty;
            }

            return string.Join(",", array);          
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            return regex.IsMatch(email);
        }

        public static string TotalHoursToTime(float value)
        {
            int hour = (int)value;
            int minute = (int)Math.Round((value - hour) * 60);
            return hour > 0
                ? ZeroPrefix(hour) + "h" + ZeroPrefix(minute) + "'"
                : ZeroPrefix(minute) + "'";
        }

        public static string MinuteToTime(int value)
        {
            if (value == 0)
            {
                return string.Empty;
            }

            int hour = value / 60;
            int minute = value - (hour * 60);
            return hour > 0 ? ZeroPrefix(hour) + "h" + ZeroPrefix(minute) + "'" : ZeroPrefix(minute) + "'";
        }      

        #region IP
        /// <summary>
        /// Convert a UNIT to IP String.
        /// </summary>
        /// <param name="unit">The unit covert to IP. </param>
        /// <returns>Return the IP.</returns>
        public static string ConvertUintToIP(uint unit)
        {
            try
            {
                byte[] bytes = BitConverter.GetBytes(unit);
                Array.Reverse(bytes);
                return new IPAddress(bytes).ToString();
            }
            catch
            {
                return "0.0.0.0";
            }
        }

        /// <summary>
        /// Convert IP String to UNIT.
        /// </summary>
        /// <param name="ip">The IP convert to Unit.</param>
        /// <returns>Return the Unit.</returns>
        public static uint ConvertIPToUint(string ip)
        {
            try
            {
                byte[] bytes = IPAddress.Parse(ip).GetAddressBytes();
                Array.Reverse(bytes);
                return BitConverter.ToUInt32(bytes, 0);
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region Strings
        /// <summary>
        /// Gets all the elements from the first array except for the elements from the second array.
        /// </summary>
        /// <param name="str1">Example STR1 is "1,2,3,5".</param>
        /// <param name="str2">Example STR2 is "2,3,4".</param>
        /// <returns>The result is "1,5".</returns>
        public static string ProducesDifferenceTwoString(string str1, string str2)
        {            
            string[] arrayStr1 = str1.Split(',');
            string[] arrayStr2 = str2.Split(',');

            IEnumerable<string> arrayDiff = arrayStr1.Except(arrayStr2).Where(item => !string.IsNullOrEmpty(item));
            if (arrayDiff.Any())
            {                
                return string.Join(",", arrayDiff);          
            }

            return string.Empty;
        }

        /// <summary>
        /// Check items in Ids are exist in list.
        /// </summary>
        /// <param name="ids">Example Ids = "1,323,12".</param>
        /// <param name="list">Example list = "1,4".</param>
        /// <returns>Result is False "because Ids miss 4".</returns>
        public static bool IsExistInSelectListItem(string ids, IEnumerable<SelectListItem> list)
        {
            string[] arrayStr1 = ids.Split(',');

            string str = string.Empty;
            foreach (SelectListItem item in list)
            {
                str += item.Value + ",";
            }

            if (!string.IsNullOrEmpty(str))
            {
                str = str.Substring(0, str.Length - 1);
            }

            string[] arrayStr2 = str.Split(',');

            IEnumerable<string> arrayDiff = arrayStr2.Except(arrayStr1);

            return arrayDiff.Count() == 0;
        }
        #endregion

        /// <summary>
        /// Gets down time.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns><c>True</c> if down time, <c>false</c> otherwise.</returns>
         public static bool GetDownTime(out string startTime, out string endTime)
        {
            startTime = endTime = null;
            DateTime now = DateTime.Now;

            var underMaintainStartTime = Convert.ToDateTime(ConfigurationManager.AppSettings["UM_StartTime"]);
            var underMaintainEndTime = Convert.ToDateTime(ConfigurationManager.AppSettings["UM_EndTime"]);
            int timeCacheUM = 10; ////second

            string urlDownTime = Convert.ToString(ConfigurationManager.AppSettings["LBUrl"]);
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
                try
                {
                    var underMaintainCtrl = new UMBusiness();
                    isUM = underMaintainCtrl.isUnderMaintainence(Sites.OddsFeed, urlDownTime, ref startTime, ref endTime);
                 
                    ////Cache Information which is gotten from Service
                    HttpContext.Current.Cache.Insert("IsUM", isUM, null, DateTime.Now.AddSeconds(timeCacheUM), TimeSpan.Zero);
                    HttpContext.Current.Cache.Insert("StartTime", startTime, null, DateTime.Now.AddSeconds(timeCacheUM), TimeSpan.Zero);
                    HttpContext.Current.Cache.Insert("EndTime", endTime, null, DateTime.Now.AddSeconds(timeCacheUM), TimeSpan.Zero);
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message, ex);                    
                    ////DNAide.Web.DNAideLog.Write("LoadBalancerLibrary Error : " + ex.Message);
                }
            }
            else
            {
                isUM = Convert.ToBoolean(HttpContext.Current.Cache["IsUM"]);
                startTime = Convert.ToString(HttpContext.Current.Cache["StartTime"]);
                endTime = Convert.ToString(HttpContext.Current.Cache["EndTime"]);
            }

            return isUM;
        }
    }
}
