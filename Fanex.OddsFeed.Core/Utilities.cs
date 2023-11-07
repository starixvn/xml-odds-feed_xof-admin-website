

namespace Fanex.OddsFeed.Core
{
    using System.Globalization;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Caching.Distributed;

    public static class Utilities
    {
        //private static readonly IMaintenanceService _maintenanceService = new MaintenanceService();
        private static IDistributedCache cache;

        public static void Initialize(IDistributedCache dc)
        {
            cache = dc;
        }

        #region Property

        public static IList<SelectListItem> DropListStatus
        {
            get
            {
                return new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Open", Value = "opened" },
                        new SelectListItem { Text = "Close", Value = "closed" }
                    };
            }
        }

        #endregion

        public static DateTime AddTimeToDate(DateTime dateTime, string time)
        {
            var times = time.Split(new[] { ':' });
            var ts = new TimeSpan(int.Parse(times[0]), int.Parse(times[1]), 0);
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

        public static DateTime FirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo = null)
        {
            cultureInfo = cultureInfo ?? CultureInfo.CurrentCulture;

            var firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            var firstDayInWeek = dayInWeek.Date;
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

        public static IList<SelectListItem> MinuteSteps(int step)
        {
            var list = new List<SelectListItem>();
            var index = 60;
            while (index > 0)
            {
                list.Add(new SelectListItem
                {
                    Text = index + "'",
                    Value = index.ToString()
                });
                index -= step;
            }

            return list;
        }

        public static IList<SelectListItem> TimeSteps(int minuteStep)
        {
            var list = new List<SelectListItem>();
            var hour = 0;
            var minute = 0;

            while (hour < 24)
            {
                var value = ZeroPrefix(hour) + ":" + ZeroPrefix(minute);
                list.Add(new SelectListItem
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
            var hour = (int)value;
            var minute = (int)Math.Round((value - hour) * 60);
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

            var hour = value / 60;
            var minute = value - (hour * 60);
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
                var bytes = BitConverter.GetBytes(unit);
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
                var bytes = IPAddress.Parse(ip).GetAddressBytes();
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
            var arrayStr1 = str1.Split(',');
            var arrayStr2 = str2.Split(',');

            var arrayDiff = arrayStr1.Except(arrayStr2).Where(item => !string.IsNullOrEmpty(item));
            if (arrayDiff.Any())
            {
                return string.Join(",", arrayDiff);
            }

            return string.Empty;
        }

        public static bool IsExistInSelectListItem(string ids, IEnumerable<SelectListItem> list)
        {
            var result = false;
            var arrayStr1 = ids.Split(',');

            var strBuilder = new StringBuilder();
            foreach (SelectListItem item in list)
            {
                strBuilder.Append(item.Value + ",");
            }

            var str = strBuilder.ToString();

            if (!string.IsNullOrEmpty(str))
            {
                str = str.Substring(0, str.Length - 1);

                var arrayStr2 = str.Split(',');
                var arrayDiff = arrayStr2.Except(arrayStr1);

                result = !arrayDiff.Any();
            }

            return result;
        }
        #endregion

        /*
        public static bool GetDownTime(out string startTime, out string endTime)
        {
            startTime = endTime = null;
            const int timeCacheUM = 10; // seconds
            var isUM = false;

            string isUMCache = cache.GetString("IsUM");
            if (string.IsNullOrEmpty(isUMCache))
            {
                try
                {
                    var umTime = _maintenanceService.GetUnderMaintenanceTime();
                    startTime = umTime.From;
                    endTime = umTime.To;
                    isUM = umTime.IsUnderMaintenanceTime;

                    // Cache Information which is gotten from Service
                    var cacheEntryOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(timeCacheUM)
                    };
                    cache.SetString("IsUM", isUM.ToString(), cacheEntryOptions);
                    cache.SetString("StartTime", startTime, cacheEntryOptions);
                    cache.SetString("EndTime", endTime, cacheEntryOptions);
                }
                catch (Exception ex)
                {
                    //LogService.Error(ex.Message, ex);
                }
            }
            else
            {
                isUM = Convert.ToBoolean(isUMCache);
                startTime = cache.GetString("StartTime");
                endTime = cache.GetString("EndTime");
            }

            return isUM;
        }
        */
    }
}
