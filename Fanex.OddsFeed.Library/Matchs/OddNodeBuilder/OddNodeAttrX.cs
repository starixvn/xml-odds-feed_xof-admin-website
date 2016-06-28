namespace Fanex.OddsFeed.Library.Matchs.OddNodeBuilder
{
    using System.Xml;
    using Fanex.OddsFeed.Library.Matches;

    class OddNodeAttrX : OddNodeAttrBase
    {
        public override void UpdateAttributeForXmlNode(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var xmlAttribute = xd.CreateAttribute(XmlAttribute);
            var value = item.GetType().GetProperty(MatchOddAttribute).GetValue(item, null);
            xmlAttribute.Value = value.ToString();

            odd.Attributes.Append(xmlAttribute);
        }
    }
}
