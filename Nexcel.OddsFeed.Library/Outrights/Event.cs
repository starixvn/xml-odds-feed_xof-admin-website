// --------------------------------------------------------------------------
// <copyright file="Event.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Nexcel.OddsFeed.Library.Outrights
{
    using Matchs.OddNodeBuilder;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// Class Event.
    /// </summary>    
    [Serializable]
    public class Event
    {
        public Event()
        {
            Content = new List<Language>();
            Odds = new List<OutrightsOdd>();
        }

        #region Properties

        public int Id { get; set; }

        public string CountryCode { get; set; }

        public IList<Language> Content { get; set; }

        public string Sport { get; set; }

        public int SportId { get; set; }

        public DateTime Time { get; set; }

        public IList<OutrightsOdd> Odds { get; set; }

        #endregion

        /// <summary>
        /// Builds the XML node.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The OddsProfile.</param>
        /// <returns>Return Xml Node.</returns>
        public XmlNode BuildXmlNodeOutRight(XmlDocument xd, OddsProfile op)
        {
            var e = CreateNode(xd, op, "event");

            var mt = xd.CreateAttribute("mt");
            mt.Value = Time.ToString();
            e.Attributes.Append(mt);

            var sport = xd.CreateAttribute("sport");
            sport.Value = Sport;
            e.Attributes.Append(sport);

            ////add odds
            foreach (var item in Odds)
            {
                if (op.Outrights != null)
                {
                    if (!op.Outrights.Contains(item.Id))
                    {
                        continue;
                    }
                }

                var odd = CreateNodeOdd(xd, op, item);
                e.AppendChild(odd);
            }

            return e;
        }

        /// <summary>
        /// Builds the XML node event.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The OddsProfile.</param>
        /// <returns>Return Xml Node.</returns>
        public XmlNode BuildXmlNodeOutRightEvent(XmlDocument xd, OddsProfile op)
        {
            var e = CreateNode(xd, op, "event");

            var mt = xd.CreateAttribute("mt");
            mt.Value = Time.ToString();
            e.Attributes.Append(mt);

            var sport = xd.CreateAttribute("sport");
            sport.Value = Sport;
            e.Attributes.Append(sport);

            ////add odds
            foreach (var item in Odds)
            {
                if (op.Outrights != null)
                {
                    if (!op.Outrights.Contains(item.Id))
                    {
                        continue;
                    }
                }

                var odd = CreateTeamNode(xd, op, item);
                e.AppendChild(odd);
            }

            return e;
        }

        #region Private methods

        /// <summary>
        /// Creates the node.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The OddsProfile.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns>Return Xml Node.</returns>
        private XmlNode CreateNode(XmlDocument xd, OddsProfile op, string nodeName)
        {
            var e = xd.CreateNode(XmlNodeType.Element, nodeName, null);

            var id = xd.CreateAttribute("id");
            id.Value = Id.ToString();
            e.Attributes.Append(id);

            if (op.Languages.Contains("en-US"))
            {
                var ge = xd.CreateAttribute("ge");
                ge.Value = Content.Where(i => i.Code == "en-US").SingleOrDefault().Content;
                e.Attributes.Append(ge);
            }

            if (op.Languages.Contains("zh-TW"))
            {
                var gs = xd.CreateAttribute("gt");
                gs.Value = Content.Where(i => i.Code == "zh-TW").SingleOrDefault().Content;
                e.Attributes.Append(gs);
            }

            if (op.Languages.Contains("zh-CN"))
            {
                var gt = xd.CreateAttribute("gs");
                gt.Value = Content.Where(i => i.Code == "zh-CN").SingleOrDefault().Content;
                e.Attributes.Append(gt);
            }

            return e;
        }

        /// <summary>
        /// Creates the node odd.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The OddsProfile.</param>
        /// <param name="item">The OutrightsOdd item.</param>
        /// <returns>Return XmlNode.</returns>        
        private XmlNode CreateNodeOdd(XmlDocument xd, OddsProfile op, OutrightsOdd item)
        {
            var odd = xd.CreateNode(XmlNodeType.Element, "odd", null);

            var oid = xd.CreateAttribute("id");
            oid.Value = item.Id.ToString();
            odd.Attributes.Append(oid);

            if (op.Languages.Contains("en-US"))
            {
                var ote = xd.CreateAttribute("te");
                ote.Value = BetOddNodeBuilder.EscapeXML(item.Team.Where(i => i.Code == "en-US").SingleOrDefault().Content);
                odd.Attributes.Append(ote);
            }

            if (op.Languages.Contains("zh-TW"))
            {
                var ots = xd.CreateAttribute("tt");
                ots.Value = BetOddNodeBuilder.EscapeXML(item.Team.Where(i => i.Code == "zh-TW").SingleOrDefault().Content);
                odd.Attributes.Append(ots);
            }

            if (op.Languages.Contains("zh-CN"))
            {
                var ott = xd.CreateAttribute("ts");
                ott.Value = BetOddNodeBuilder.EscapeXML(item.Team.Where(i => i.Code == "zh-CN").SingleOrDefault().Content);
                odd.Attributes.Append(ott);
            }

            var oo = xd.CreateAttribute("o");
            oo.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Value)).ToString();  // item.Value;                
            odd.Attributes.Append(oo);
            return odd;
        }

        /// <summary>
        /// Creates the team node.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The OddsProfile.</param>
        /// <param name="item">The OutrightsOdd item.</param>
        /// <returns>Return XmlNode.</returns>
        private XmlNode CreateTeamNode(XmlDocument xd, OddsProfile op, OutrightsOdd item)
        {
            var odd = xd.CreateNode(XmlNodeType.Element, "team", null);

            if (op.Languages.Contains("en-US"))
            {
                var ote = xd.CreateAttribute("te");
                ote.Value = item.Team.Where(i => i.Code == "en-US").SingleOrDefault().Content;
                odd.Attributes.Append(ote);
            }

            if (op.Languages.Contains("zh-TW"))
            {
                var ots = xd.CreateAttribute("tt");
                ots.Value = item.Team.Where(i => i.Code == "zh-TW").SingleOrDefault().Content;
                odd.Attributes.Append(ots);
            }

            if (op.Languages.Contains("zh-CN"))
            {
                var ott = xd.CreateAttribute("ts");
                ott.Value = item.Team.Where(i => i.Code == "zh-CN").SingleOrDefault().Content;
                odd.Attributes.Append(ott);
            }

            return odd;
        }

        #endregion
    }
}
