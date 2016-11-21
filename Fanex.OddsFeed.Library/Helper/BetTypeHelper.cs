namespace Fanex.OddsFeed.Library.Helper
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

        public static bool IsChildBetType(IDictionary<int, BetType> betTypeList, int betTypeId)
        {
            if (betTypeList.ContainsKey(betTypeId))
            {
                return betTypeList[betTypeId].IsChildBetType;
            }

            return false;
        }
    }
}
