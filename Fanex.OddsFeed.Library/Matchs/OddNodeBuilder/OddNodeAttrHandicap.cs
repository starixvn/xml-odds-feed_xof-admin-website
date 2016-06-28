namespace Fanex.OddsFeed.Library.Matchs.OddNodeBuilder
{
    using Fanex.Common;
    using Fanex.OddsFeed.Library.Matches;
    using System;
    using System.Data;
    using System.Xml;

    class OddNodeAttrHandicap : OddNodeAttrBase
    {
        public override void UpdateAttributeForXmlNode(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var xmlAttribute = xd.CreateAttribute(XmlAttribute);
            var value = item.GetType().GetProperty(MatchOddAttribute).GetValue(item, null);
            xmlAttribute.Value = Convert.ToDecimal(value).ToString();

            odd.Attributes.Append(xmlAttribute);
        }

        public override void UpdateAttributeForMatchOdd(DataRow item, MatchOdd odd)
        {
            if (!NotToBuildData)
            {
                var value = string.Format("{0:0.#########}", Converter.ToDecimal(item[DBAttribute]));
                odd.GetType().GetProperty(MatchOddAttribute).SetValue(odd, value, null);
            }
        }
    }
}
