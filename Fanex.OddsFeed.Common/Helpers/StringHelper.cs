namespace Fanex.OddsFeed.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class StringHelper
    {
        public static bool ContainInList(IList<string> list, string keyWord)
        {
            return list.Any(value => Contain(value, keyWord));
        }

        public static bool Contain(string value, string keyWord)
        {
            return value.IndexOf(keyWord, StringComparison.CurrentCultureIgnoreCase) > -1;
        }
    }
}
