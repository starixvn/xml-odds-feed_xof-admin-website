namespace Fanex.OddsFeed.Core
{
    using System.Globalization;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class Utilities
    {
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
    }
}
