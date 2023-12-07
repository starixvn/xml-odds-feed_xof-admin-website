namespace Fanex.OddsFeed.Core
{
    using System.Globalization;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
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

        public static string ZeroPrefix(int value)
        {
            return value < 10 ? "0" + value : value.ToString();
        }

        public static string StringArrayToString(string[] array)
        {
            if (array == null || array.Length == 0)
            {
                return string.Empty;
            }

            return string.Join(",", array);
        }

        #region IP

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

        public static int GetIso8601WeekOfYear(DateTime time)
        {
            var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day is >= DayOfWeek.Monday and <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

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
