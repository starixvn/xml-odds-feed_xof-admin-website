namespace Fanex.OddsFeed.Library.Matchs.OddNodeBuilder
{
    using Fanex.Common;
    using Fanex.OddsFeed.Library.Matches;
    using System;
    using System.Data;
    using System.Xml;

    public class OddNodeAttrBase
    {
        public string XmlAttribute { get; set; }

        public string DBAttribute { get; set; }

        public string MatchOddAttribute { get; set; }

        public string Description { get; set; }

        public bool NotToBuildData { get; set; }

        public bool NotToConvertOdd { get; set; }

        public virtual void UpdateAttributeForXmlNode(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var xmlAttribute = xd.CreateAttribute(XmlAttribute);
            var value = item.GetType().GetProperty(MatchOddAttribute).GetValue(item, null);

            if (NotToConvertOdd)
            {
                xmlAttribute.Value = Convert.ToString(value);
            }
            else
            {
                xmlAttribute.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(value)).ToString();
            }

            odd.Attributes.Append(xmlAttribute);
        }

        public virtual void UpdateAttributeForMatchOdd(DataRow item, MatchOdd odd)
        {
            if (!NotToBuildData && !(item[DBAttribute] == null) && !(item[DBAttribute].GetType() == typeof(System.DBNull)))
            {
                string value;
                if (NotToConvertOdd)
                {
                    value = string.Format("{0:0.#########}", Converter.ToDecimal(item[DBAttribute]));
                }
                else
                {
                    value = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item[DBAttribute])));
                }

                odd.GetType().GetProperty(MatchOddAttribute).SetValue(odd, value, null);
            }
        }
    }
}
