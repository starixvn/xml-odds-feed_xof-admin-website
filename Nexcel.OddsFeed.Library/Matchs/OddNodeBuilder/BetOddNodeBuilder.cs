namespace Nexcel.OddsFeed.Library.Matchs.OddNodeBuilder
{
    using Nexcel.OddsFeed.Library.Matches;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Xml;

    class BetOddNodeBuilder
    {
        public static void BuildXmlOddNodeAttributes(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd, string betType)
        {
            var betTypeNode = BetTypeMapping.BetTypes.Find(bt => bt.Tag.Equals(betType));
            betTypeNode.UpdateAttributesForXmlNode(xd, op, item, odd);
        }

        public static MatchOdd CreateMatchOddBettype(DataRow item, int betTypeId)
        {
            var betTypeNode = BetTypeMapping.BetTypes.Find(bt => bt.ID.Equals(betTypeId));
            var odd = betTypeNode.CreateMatchOddBettype(item);

            return odd;
        }

        public static MatchOdd CreateMatchOddBettype(DataRow item, int betTypeId, List<Model.OddFlex> flexOdds)
        {
            var odds = flexOdds.Where(i => i.OddId == Convert.ToInt32(item["oddsid"]));

            var betTypeNode = BetTypeMapping.BetTypes.Find(bt => bt.ID.Equals(betTypeId));
            var odd = ((BetTypeNodeFlex)betTypeNode).CreateMatchOddBettype(item, flexOdds);

            return odd;
        }
    }
}
