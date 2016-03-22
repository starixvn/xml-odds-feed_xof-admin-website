namespace Nexcel.OddsFeed.Library.Matchs.OddNodeBuilder
{
    using Fanex.Common;
    using Nexcel.OddsFeed.Library.Matches;
    using System;
    using System.Data;
    using System.Xml;

    class OddNodeHandicap : OddNodeBase
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
            var value = string.Format("{0:0.#########}", Converter.ToDecimal(item[DBAttribute]));
            odd.GetType().GetProperty(MatchOddAttribute).SetValue(this, value, null);
        }
    }
}
