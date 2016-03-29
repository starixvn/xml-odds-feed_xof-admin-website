namespace Nexcel.OddsFeed.Library.Matchs.OddNodeBuilder
{
    using Fanex.Logging;
    using Nexcel.OddsFeed.Library.Matches;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Xml;

    public class BetOddNodeBuilder
    {
        public static string BetTypeNameFromId(int id)
        {
            return BetOddNodeMapping.BetTypeIdMap.ContainsKey(id)
                ? BetOddNodeMapping.BetTypeIdMap[id]
                : "b" + id;
        }

        public static void BuildXmlOddNodeAttributes(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd, string betType)
        {
            try {
                var betTypeNode = BetOddNodeMapping.BetTypes.Find(bt => bt.Tag.Equals(betType));
                if(betTypeNode != null)
                {
                    betTypeNode.UpdateAttributesForXmlNode(xd, op, item, odd);
                }                
            }
            catch(Exception ex)
            {
                LogService.Error("BetOddNodeBuilder.BuildXmlOddNodeAttributes errors.  Exception: " + ex.Message);
            }
        }

        public static MatchOdd CreateMatchOddBettype(DataRow item, int betTypeId)
        {
            MatchOdd odd = null;
            try
            {
                var betTypeNode = BetOddNodeMapping.BetTypes.Find(bt => bt.ID.Equals(betTypeId));
                if(betTypeNode != null)
                {
                    odd = betTypeNode.CreateMatchOddBettype(item);
                }                
            }
            catch(Exception ex)
            {
                LogService.Error("BetOddNodeBuilder.CreateMatchOddBettype errors.  Exception: " + ex.Message);
            }

            return odd;
        }

        public static MatchOdd CreateMatchOddBettype(DataRow item, int betTypeId, List<Model.OddFlex> flexOdds)
        {
            MatchOdd odd = null;
            try {
                var odds = flexOdds.Where(i => i.OddId == Convert.ToInt32(item["oddsid"]));
                var betTypeNode = BetOddNodeMapping.BetTypes.Find(bt => bt.ID.Equals(betTypeId));
                if (betTypeNode != null)
                {
                    odd = ((OddNodeFlex)betTypeNode).CreateMatchOddBettype(item, odds);
                }
            }
            catch(Exception ex)
            {
                LogService.Error("BetOddNodeBuilder.CreateMatchOddBettype errors.  Exception: " + ex.Message);
            }

            return odd;
        }        
    }
}
