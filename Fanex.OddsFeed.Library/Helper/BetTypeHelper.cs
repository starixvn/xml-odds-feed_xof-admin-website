namespace Fanex.OddsFeed.Library.Helper
{
    using System.Collections.Generic;

    public static class BetTypeHelper
    {
        public static string GetBetTypeName(IDictionary<int, string> betTypeList, int betTypeId)
        {
            if (betTypeList.ContainsKey(betTypeId))
            {
                return betTypeList[betTypeId];
            }

            return "b" + betTypeId;
        }
    }
}
