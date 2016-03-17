namespace Nexcel.OddsFeed.Library.Matchs
{
    using Nexcel.OddsFeed.Library.Matches;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    class MatchOddNodeBuilder
    {
        public static readonly Dictionary<string, NodeInfo[]> BetTypeToOddNodeMapping = new Dictionary<string, NodeInfo[]>
        {
            {
                "ou_h",
                new NodeInfo[2] {
                    new NodeInfo { XmlAttrName = "o", MatchOddPropertyName = "O", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "u", MatchOddPropertyName = "U", NeedToConvert = true }
                }
            },
            {
                "ou_a",
                new NodeInfo[2] {
                    new NodeInfo { XmlAttrName = "o", MatchOddPropertyName = "O", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "u", MatchOddPropertyName = "U", NeedToConvert = true }
                }
            },
            {
                "ou_h1",
                new NodeInfo[2] {
                    new NodeInfo { XmlAttrName = "o", MatchOddPropertyName = "O", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "u", MatchOddPropertyName = "U", NeedToConvert = true }
                }
            },
            {
                "ou_a1",
                new NodeInfo[2] {
                    new NodeInfo { XmlAttrName = "o", MatchOddPropertyName = "O", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "u", MatchOddPropertyName = "U", NeedToConvert = true }
                }
            },
            {
                "cs_2",
                new NodeInfo[17] {
                    new NodeInfo { XmlAttrName = "cs10", MatchOddPropertyName = "CS10", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs20", MatchOddPropertyName = "CS20", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs21", MatchOddPropertyName = "CS21", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs30", MatchOddPropertyName = "CS30", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs31", MatchOddPropertyName = "CS31", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs32", MatchOddPropertyName = "CS32", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs01", MatchOddPropertyName = "CS01", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs02", MatchOddPropertyName = "CS02", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs12", MatchOddPropertyName = "CS12", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs03", MatchOddPropertyName = "CS03", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs13", MatchOddPropertyName = "CS13", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs23", MatchOddPropertyName = "CS23", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs00", MatchOddPropertyName = "CS00", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs11", MatchOddPropertyName = "CS11", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs22", MatchOddPropertyName = "CS22", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs33", MatchOddPropertyName = "CS33", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "csoas", MatchOddPropertyName = "CSOAS", NeedToConvert = true }
                }
            },
            {
                "etg",
                new NodeInfo[7] {
                    new NodeInfo { XmlAttrName = "o0", MatchOddPropertyName = "O0", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "o1", MatchOddPropertyName = "O1", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "o2", MatchOddPropertyName = "O2", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "o3", MatchOddPropertyName = "O3", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "o4", MatchOddPropertyName = "O4", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "o5", MatchOddPropertyName = "O5", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "o6", MatchOddPropertyName = "O6", NeedToConvert = true }
                }
            },
            {
                "heg",
                new NodeInfo[4] {
                    new NodeInfo { XmlAttrName = "o0", MatchOddPropertyName = "O0", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "o1", MatchOddPropertyName = "O1", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "o2", MatchOddPropertyName = "O2", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "o3", MatchOddPropertyName = "O3", NeedToConvert = true }
                }
            },
            {
                "win",
                new NodeInfo[8] {
                    new NodeInfo { XmlAttrName = "h1", MatchOddPropertyName = "H1", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "h2", MatchOddPropertyName = "H2", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "h3", MatchOddPropertyName = "H3", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "d", MatchOddPropertyName = "D", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "a1", MatchOddPropertyName = "A1", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "a2", MatchOddPropertyName = "A2", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "a3", MatchOddPropertyName = "A3", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "ng", MatchOddPropertyName = "NG", NeedToConvert = true }
                }
            },
            {
                "aeg",
                new NodeInfo[4] {
                    new NodeInfo { XmlAttrName = "o0", MatchOddPropertyName = "O0", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "o1", MatchOddPropertyName = "O1", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "o2", MatchOddPropertyName = "O2", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "o3", MatchOddPropertyName = "O3", NeedToConvert = true }
                }
            },
            {
                "x2_1",
                new NodeInfo[3] {
                    new NodeInfo { XmlAttrName = "hd", MatchOddPropertyName = "hd", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "ha", MatchOddPropertyName = "ha", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "ad", MatchOddPropertyName = "ad", NeedToConvert = true }
                }
            },
            {
                "dnb_1",
                new NodeInfo[2] {
                    new NodeInfo { XmlAttrName = "h", MatchOddPropertyName = "H", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "a", MatchOddPropertyName = "A", NeedToConvert = true }
                }
            },
            {
                "egs_1st",
                new NodeInfo[4] {
                    new NodeInfo { XmlAttrName = "o0", MatchOddPropertyName = "O0", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "o1", MatchOddPropertyName = "O1", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "o2", MatchOddPropertyName = "O2", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "o3", MatchOddPropertyName = "O3", NeedToConvert = true }
                }
            },
            {
                "cs",
                new NodeInfo[26] {
                    new NodeInfo { XmlAttrName = "cs10", MatchOddPropertyName = "CS10", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs20", MatchOddPropertyName = "CS20", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs21", MatchOddPropertyName = "CS21", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs30", MatchOddPropertyName = "CS30", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs31", MatchOddPropertyName = "CS31", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs32", MatchOddPropertyName = "CS32", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs40", MatchOddPropertyName = "CS40", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs41", MatchOddPropertyName = "CS41", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs42", MatchOddPropertyName = "CS42", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs43", MatchOddPropertyName = "CS43", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs01", MatchOddPropertyName = "CS01", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs02", MatchOddPropertyName = "CS02", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs12", MatchOddPropertyName = "CS12", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs03", MatchOddPropertyName = "CS03", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs13", MatchOddPropertyName = "CS13", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs23", MatchOddPropertyName = "CS23", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs04", MatchOddPropertyName = "CS04", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs14", MatchOddPropertyName = "CS14", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs24", MatchOddPropertyName = "CS24", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs34", MatchOddPropertyName = "CS34", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs00", MatchOddPropertyName = "CS00", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs11", MatchOddPropertyName = "CS11", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs22", MatchOddPropertyName = "CS22", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs33", MatchOddPropertyName = "CS33", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs44", MatchOddPropertyName = "CS44", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "csoas", MatchOddPropertyName = "CSOAS", NeedToConvert = true }


                }
            },
            {
                "cs_1",
                new NodeInfo[17] {
                    new NodeInfo { XmlAttrName = "cs10", MatchOddPropertyName = "CS10", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs20", MatchOddPropertyName = "CS20", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs21", MatchOddPropertyName = "CS21", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs30", MatchOddPropertyName = "CS30", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs31", MatchOddPropertyName = "CS31", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs32", MatchOddPropertyName = "CS32", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs01", MatchOddPropertyName = "CS01", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs02", MatchOddPropertyName = "CS02", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs12", MatchOddPropertyName = "CS12", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs03", MatchOddPropertyName = "CS03", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs13", MatchOddPropertyName = "CS13", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs23", MatchOddPropertyName = "CS23", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs00", MatchOddPropertyName = "CS00", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs11", MatchOddPropertyName = "CS11", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs22", MatchOddPropertyName = "CS22", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "cs33", MatchOddPropertyName = "CS33", NeedToConvert = true },
                    new NodeInfo { XmlAttrName = "csoas", MatchOddPropertyName = "CSOAS", NeedToConvert = true }
                }
            }
        };

        public struct NodeInfo
        {
            public string XmlAttrName { get; set; }
            public string MatchOddPropertyName { get; set; }
            public bool NeedToConvert { get; set; }
        }

        public static void BuildXmlOddNodeAttributes(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd, string betType)
        {
            var nodeInfoList = BetTypeToOddNodeMapping[betType];
            foreach(NodeInfo nodeInfo in nodeInfoList)
            {
                var xmlAttribute = xd.CreateAttribute(nodeInfo.XmlAttrName);
                var value = item.GetType().GetProperty(nodeInfo.MatchOddPropertyName).GetValue(item, null);
                if(nodeInfo.NeedToConvert)
                {
                    xmlAttribute.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(value)).ToString();
                }
                else
                {
                    xmlAttribute.Value = value.ToString();
                }
                
                odd.Attributes.Append(xmlAttribute);
            }
        }
    }
}
