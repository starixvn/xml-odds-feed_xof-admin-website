namespace Nexcel.OddsFeed.Library.Matchs.OddNodeBuilder
{
    using Nexcel.OddsFeed.Library.Matches;
    using System;
    using System.Data;
    using System.Xml;

    public class OddNodeBase
    {
        public int ID { get; set; }
        public string Tag { get; set; }
        public OddNodeAttrBase[] Odds { get; set; }
        public string Desc { get; set; }
        public bool IsChildBettype { get; set; }

        public void UpdateAttributesForXmlNode(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            foreach (OddNodeAttrBase oddNode in Odds)
            {
                oddNode.UpdateAttributeForXmlNode(xd, op, item, odd);
            }
        }

        public virtual MatchOdd CreateMatchOddBettype(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = Tag,
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };

            foreach (OddNodeAttrBase oddNode in Odds)
            {
                oddNode.UpdateAttributeForMatchOdd(item, odd);
            }

            return odd;
        }

       
    }
}
