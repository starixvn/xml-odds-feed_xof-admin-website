namespace Fanex.OddsFeed.Common.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class StringConverter
    {
        public static IList<string> ConvertToListString(string stringArray, string separator)
        {
            return stringArray.Split(new[] { separator }, StringSplitOptions.None).ToList();
        }

        public static IList<string> ConvertToListString(string stringArray, bool isTrim = false)
        {
            var items = stringArray.Split(new [] { "," }, StringSplitOptions.RemoveEmptyEntries);

            return isTrim ? items.Select(item => item.Trim()).ToList() : items.ToList();
        }
    }
}
