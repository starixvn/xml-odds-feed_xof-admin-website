namespace Fanex.OddsFeed.Library.Helpers
{
    using System.Collections.Generic;
    using Model;

    public static class BetTypeHelper
    {
        public static string GetBetTypeName(IDictionary<int, BetType> betTypeList, int betTypeId)
        {
            if (betTypeList.ContainsKey(betTypeId))
            {
                return betTypeList[betTypeId].BetTypeName;
            }

            return "b" + betTypeId;
        }
    }
}
