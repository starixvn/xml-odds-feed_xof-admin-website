namespace Fanex.OddsFeed.Common.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class NumberConverter
    {
        public static IList<int> ConvertStringToIntList(string stringArray)
        {
            var items = stringArray.Split(new [] { "," }, StringSplitOptions.RemoveEmptyEntries);

            return items.Select(item => Convert.ToInt32(item)).ToList();
        }
    }
}
