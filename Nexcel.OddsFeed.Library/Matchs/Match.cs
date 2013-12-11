// --------------------------------------------------------------------------
// <copyright file="Match.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>
namespace Nexcel.OddsFeed.Library.Matchs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Xml;
    using Nexcel.OddsFeed.Library.Matches;

    /// <summary>
    /// Class Match.
    /// </summary>
    [Serializable]
    public class Match
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Match"/> class.
        /// </summary>
        public Match()
        {
            League = new List<Language>();
            Home = new List<Language>();
            Away = new List<Language>();
        }

        #region Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the league identifier.
        /// </summary>
        /// <value>The league identifier.</value>
        public int LeagueId { get; set; }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        /// <value>The country code.</value>
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the league.
        /// </summary>
        /// <value>The league.</value>
        public IList<Language> League { get; set; }

        /// <summary>
        /// Gets or sets the home.
        /// </summary>
        /// <value>The home team.</value>
        public IList<Language> Home { get; set; }

        /// <summary>
        /// Gets or sets the away.
        /// </summary>
        /// <value>The away team.</value>
        public IList<Language> Away { get; set; }

        /// <summary>
        /// Gets or sets the sport.
        /// </summary>
        /// <value>The sport.</value>
        public string Sport { get; set; }

        /// <summary>
        /// Gets or sets the sport identifier.
        /// </summary>
        /// <value>The sport identifier.</value>
        public int SportId { get; set; }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>The date time.</value>
        public DateTime Time { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Match"/> is running.
        /// </summary>
        /// <value><c>true</c> if running; otherwise, <c>false</c>.</value>
        public bool Running { get; set; }

        /// <summary>
        /// Gets or sets the bookie.
        /// </summary>
        /// <value>The bookie.</value>
        public Bookie Bookie { get; set; }

        /// <summary>
        /// Gets or sets the type of the bet.
        /// </summary>
        /// <value>The type of the bet.</value>
        public int BetType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is live.
        /// </summary>
        /// <value><c>true</c> if this instance is live; otherwise, <c>false</c>.</value>
        public bool IsLive { get; set; }

        /// <summary>
        /// Gets or sets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        public int TimeStamp { get; set; }

        #endregion

        /// <summary>
        /// Build Xml Node.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="bookiename">Bookie name.</param>
        /// <param name="filter">Odds feed filter.</param>
        /// <returns>The XmlNode.</returns>
        public XmlNode BuildXmlNodeMatch(XmlDocument xd, OddsProfile op, string bookiename, OddsFeedFilter filter)
        {
            var bookie = BuildXmlNodeBookie(xd, op, bookiename, filter);
            if (bookie == null)
            {
                return null;
            }

            var match = xd.CreateNode(XmlNodeType.Element, "match", null);

            var id = xd.CreateAttribute("id");
            id.Value = Id.ToString();
            match.Attributes.Append(id);

            BuildXmlNodeLeague(xd, op, ref match);
            BuildXmlNodeHomeTeam(xd, op, ref match);
            BuildXmlNodeAwayTeam(xd, op, ref match);

            var mt = xd.CreateAttribute("mt");
            mt.Value = Time.ToString();
            match.Attributes.Append(mt);

            var sport = xd.CreateAttribute("sport");
            sport.Value = Sport;
            match.Attributes.Append(sport);

            var running = xd.CreateAttribute("running");
            running.Value = Running ? "1" : "0";
            match.Attributes.Append(running);

            match.AppendChild(bookie);
            return match;     
        }

        /// <summary>
        /// Build Xml Node Event.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="bookiename">Bookie name.</param>
        /// <returns>The XmlNode.</returns>
        public XmlNode BuildXmlNodeEvent(XmlDocument xd, OddsProfile op, string bookiename)
        {
            var match = xd.CreateNode(XmlNodeType.Element, "match", null);

            var id = xd.CreateAttribute("id");
            id.Value = Id.ToString();
            match.Attributes.Append(id);

            BuildXmlNodeHomeTeam(xd, op, ref match);
            BuildXmlNodeAwayTeam(xd, op, ref match);

            var mt = xd.CreateAttribute("mt");
            mt.Value = Time.ToString();
            match.Attributes.Append(mt);

            var running = xd.CreateAttribute("running");
            running.Value = Running ? "1" : "0";
            match.Attributes.Append(running);

            return match;
        }

        /// <summary>
        /// Build Xml Node League.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="match">The XmlNode.</param>
        private void BuildXmlNodeLeague(XmlDocument xd, OddsProfile op, ref XmlNode match)
        {
            if (op.Languages.Contains("en-US"))
            {
                var ge = xd.CreateAttribute("ge");
                ge.Value = League.Where(i => i.Code == "en-US").SingleOrDefault().Content;
                match.Attributes.Append(ge);
            }

            if (op.Languages.Contains("zh-TW"))
            {
                var gs = xd.CreateAttribute("gt");
                gs.Value = League.Where(i => i.Code == "zh-TW").SingleOrDefault().Content;
                match.Attributes.Append(gs);
            }

            if (op.Languages.Contains("zh-CN"))
            {
                var gt = xd.CreateAttribute("gs");
                gt.Value = League.Where(i => i.Code == "zh-CN").SingleOrDefault().Content;
                match.Attributes.Append(gt);
            }
        }

        /// <summary>
        /// Build Xml Node Home Team.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="match">The XmlNode.</param>
        private void BuildXmlNodeHomeTeam(XmlDocument xd, OddsProfile op, ref XmlNode match)
        {
            ////home
            if (op.Languages.Contains("en-US"))
            {
                var he = xd.CreateAttribute("he");
                he.Value = Home.Where(i => i.Code == "en-US").SingleOrDefault().Content;
                match.Attributes.Append(he);
            }

            if (op.Languages.Contains("zh-TW"))
            {
                var hs = xd.CreateAttribute("ht");
                hs.Value = Home.Where(i => i.Code == "zh-TW").SingleOrDefault().Content;
                match.Attributes.Append(hs);
            }

            if (op.Languages.Contains("zh-CN"))
            {
                var ht = xd.CreateAttribute("hs");
                ht.Value = Home.Where(i => i.Code == "zh-CN").SingleOrDefault().Content;
                match.Attributes.Append(ht);
            }
        }

        /// <summary>
        /// Build Xml Node Away Team.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="match">The XmlNode.</param>
        private void BuildXmlNodeAwayTeam(XmlDocument xd, OddsProfile op, ref XmlNode match)
        {
            ////away
            if (op.Languages.Contains("en-US"))
            {
                var ae = xd.CreateAttribute("ae");
                ae.Value = Away.Where(i => i.Code == "en-US").SingleOrDefault().Content;
                match.Attributes.Append(ae);
            }

            if (op.Languages.Contains("zh-TW"))
            {
                var tas = xd.CreateAttribute("at");
                tas.Value = Away.Where(i => i.Code == "zh-TW").SingleOrDefault().Content;
                match.Attributes.Append(tas);
            }

            if (op.Languages.Contains("zh-CN"))
            {
                var at = xd.CreateAttribute("as");
                at.Value = Away.Where(i => i.Code == "zh-CN").SingleOrDefault().Content;
                match.Attributes.Append(at);
            }
        }

        /// <summary>
        /// Build Xml Node Bookie.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="bookiename">Bookie name.</param>
        /// <param name="filter">Odds Feed Filter.</param>        
        /// <returns>The xml node.</returns>
        private XmlNode BuildXmlNodeBookie(XmlDocument xd, OddsProfile op, string bookiename, OddsFeedFilter filter)
        {
            ////bookie
            var bookie = xd.CreateNode(XmlNodeType.Element, "bookie", null);        

            BuildXmlNodeOdd(xd, op, bookiename, filter, ref bookie);
            if (bookie.ChildNodes.Count == 0)
            {
                return null;
            }

            var bname = xd.CreateAttribute("name");
            bname.Value = bookiename; ////this.Bookie.Name.Where(i => i.Code == "en-US").SingleOrDefault().Content;
            bookie.Attributes.Append(bname);

            var bsimp = xd.CreateAttribute("simp");
            bsimp.Value = bookiename; ////this.Bookie.Name.Where(i => i.Code == "zh-TW").SingleOrDefault().Content;
            bookie.Attributes.Append(bsimp);

            var btrad = xd.CreateAttribute("trad");
            btrad.Value = bookiename; ////this.Bookie.Name.Where(i => i.Code == "zh-CN").SingleOrDefault().Content;
            bookie.Attributes.Append(btrad);

            return bookie;
        }

        /// <summary>
        /// Build Xml Node Odds.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="bookiename">Bookie name.</param>
        /// <param name="filter">Odds Feed Filter.</param>
        /// <param name="bookie">The Xml Node.</param>
        private void BuildXmlNodeOdd(XmlDocument xd, OddsProfile op, string bookiename, OddsFeedFilter filter, ref XmlNode bookie)
        {
            ////odds
            foreach (var item in Bookie.Odds)
            {
                if (op.BetTypes != null)
                {
                    if (!op.BetTypes.Contains(item.BetTypeId))
                    {
                        continue;
                    }
                }

                ////filter by Bettype from URL link.
                if (filter.BetTypes != null && filter.BetTypes.Count > 0)
                {
                    if (!filter.BetTypes.Contains(item.BetTypeId))
                    {
                        continue;
                    }
                }

                var odd = CreateNodeOdd(xd, op, item);

                bookie.AppendChild(odd);
            }
        }

        /// <summary>
        /// Creates the node odd.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="item">The match odd.</param>
        /// <returns>The Xml Node.</returns>
        private XmlNode CreateNodeOdd(XmlDocument xd, OddsProfile op, MatchOdd item)
        {
            var odd = xd.CreateNode(XmlNodeType.Element, "odd", null);

            var ot = xd.CreateAttribute("t");
            ot.Value = item.Type;
            odd.Attributes.Append(ot);
            ////for bet types: 1,2,3,7,8,12 -> convert by region
            switch (item.Type)
            {
                case "ah":
                case "ou":
                case "ah_1":
                case "ou_1":
                    BuildXmlNodeOdd_ah_ou(xd, op, item, odd);
                    break;
                case "st":
                case "st_1":
                    BuildXmlNodeOdd_st(xd, op, item, odd);
                    break;
                case "oe":
                case "oe_1":
                    BuildXmlNodeOdd_oe(xd, op, item, odd);
                    break;
                case "tg":
                    BuildXmlNodeOdd_tg(xd, op, item, odd);
                    break;
                case "htft":
                    BuildXmlNodeOdd_htft(xd, op, item, odd);
                    break;
                case "fl":
                    BuildXmlNodeOdd_fl(xd, op, item, odd);
                    break;
                case "cs":
                    BuildXmlNodeOdd_cs(xd, op, item, odd);
                    break;
            }

            var otime = xd.CreateAttribute("time");
            otime.Value = item.Time.ToString();
            odd.Attributes.Append(otime);

            ////if (op.SpParams == "timestamp")
            {
                var otimestamp = xd.CreateAttribute("timestamp");
                otimestamp.Value = item.TimeStamp.ToString();
                odd.Attributes.Append(otimestamp);
            }

            return odd;
        }

        #region Build Xml Node

        /// <summary>
        /// Builds the XML node odd_cs.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="item">The match node.</param>
        /// <param name="odd">The Xml Node.</param>
        private void BuildXmlNodeOdd_cs(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var cs_c00 = xd.CreateAttribute("c00");
            cs_c00.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C00)).ToString(); ////item.C00;
            odd.Attributes.Append(cs_c00);

            var cs_11 = xd.CreateAttribute("c11");
            cs_11.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C11)).ToString(); ////item.C11;
            odd.Attributes.Append(cs_11);

            var cs_22 = xd.CreateAttribute("c22");
            cs_22.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C22)).ToString(); ////item.C22;
            odd.Attributes.Append(cs_22);

            var cs_33 = xd.CreateAttribute("c33");
            cs_33.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C33)).ToString(); ////item.C33;
            odd.Attributes.Append(cs_33);

            var cs_44 = xd.CreateAttribute("c44");
            cs_44.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C44)).ToString(); ////item.C44;
            odd.Attributes.Append(cs_44);

            BuildXmlNodeOdd_cs_0x(xd, op, item, odd);

            BuildXmlNodeOdd_cs_x0(xd, op, item, odd);

            BuildXmlNodeOdd_cs_123(xd, op, item, odd);

            BuildXmlNodeOdd_cs_x4(xd, op, item, odd);

            BuildXmlNodeOdd_cs_213(xd, op, item, odd);

            BuildXmlNodeOdd_cs_4x(xd, op, item, odd);
        }

        /// <summary>
        /// Builds the XML node odd_cs_4x.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="item">The match node.</param>
        /// <param name="odd">The Xml Node.</param>
        private void BuildXmlNodeOdd_cs_4x(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var cs_41 = xd.CreateAttribute("c41");
            cs_41.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C41)).ToString(); ////item.C41;
            odd.Attributes.Append(cs_41);

            var cs_42 = xd.CreateAttribute("c42");
            cs_42.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C42)).ToString(); ////item.C42;
            odd.Attributes.Append(cs_42);

            var cs_43 = xd.CreateAttribute("c43");
            cs_43.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C43)).ToString(); ////item.C43;
            odd.Attributes.Append(cs_43);
        }

        /// <summary>
        /// Builds the XML node odd_cs_213.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="item">The match node.</param>
        /// <param name="odd">The Xml Node.</param>
        private void BuildXmlNodeOdd_cs_213(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var cs_21 = xd.CreateAttribute("c21");
            cs_21.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C21)).ToString(); ////item.C21;
            odd.Attributes.Append(cs_21);

            var cs_31 = xd.CreateAttribute("c31");
            cs_31.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C31)).ToString(); ////item.C31;
            odd.Attributes.Append(cs_31);

            var cs_32 = xd.CreateAttribute("c32");
            cs_32.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C32)).ToString(); ////item.C32;
            odd.Attributes.Append(cs_32);
        }

        /// <summary>
        /// Builds the XML node odd_cs_x4.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="item">The match node.</param>
        /// <param name="odd">The Xml Node.</param>
        private void BuildXmlNodeOdd_cs_x4(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var cs_14 = xd.CreateAttribute("c14");
            cs_14.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C14)).ToString(); ////item.C14;
            odd.Attributes.Append(cs_14);

            var cs_24 = xd.CreateAttribute("c24");
            cs_24.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C24)).ToString(); ////item.C24;
            odd.Attributes.Append(cs_24);

            var cs_34 = xd.CreateAttribute("c34");
            cs_34.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C34)).ToString(); ////item.C34;
            odd.Attributes.Append(cs_34);
        }

        /// <summary>
        /// Builds the XML node odd_cs_123.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="item">The match node.</param>
        /// <param name="odd">The Xml Node.</param>
        private void BuildXmlNodeOdd_cs_123(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var cs_12 = xd.CreateAttribute("c12");
            cs_12.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C12)).ToString(); ////item.C12;
            odd.Attributes.Append(cs_12);

            var cs_13 = xd.CreateAttribute("c13");
            cs_13.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C13)).ToString(); ////item.C13;
            odd.Attributes.Append(cs_13);

            var cs_23 = xd.CreateAttribute("c23");
            cs_23.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C23)).ToString(); ////item.C23;
            odd.Attributes.Append(cs_23);
        }

        /// <summary>
        /// Builds the XML node odd_cs_x0.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="item">The match node.</param>
        /// <param name="odd">The Xml Node.</param>
        private void BuildXmlNodeOdd_cs_x0(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var cs_10 = xd.CreateAttribute("c10");
            cs_10.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C10)).ToString(); ////item.C10;
            odd.Attributes.Append(cs_10);

            var cs_20 = xd.CreateAttribute("c20");
            cs_20.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C20)).ToString(); ////item.C20;
            odd.Attributes.Append(cs_20);

            var cs_30 = xd.CreateAttribute("c30");
            cs_30.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C30)).ToString(); ////item.C30;
            odd.Attributes.Append(cs_30);

            var cs_40 = xd.CreateAttribute("c40");
            cs_40.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C40)).ToString(); ////item.C40;
            odd.Attributes.Append(cs_40);

            var cs_50 = xd.CreateAttribute("c50");
            cs_50.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C50)).ToString(); ////item.C50;
            odd.Attributes.Append(cs_50);
        }

        /// <summary>
        /// Builds the XML node odd_cs_0x.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="item">The match node.</param>
        /// <param name="odd">The Xml Node.</param>
        private void BuildXmlNodeOdd_cs_0x(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var cs_01 = xd.CreateAttribute("c01");
            cs_01.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C01)).ToString(); ////item.C01;
            odd.Attributes.Append(cs_01);

            var cs_02 = xd.CreateAttribute("c02");
            cs_02.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C02)).ToString(); ////item.C02;
            odd.Attributes.Append(cs_02);

            var cs_03 = xd.CreateAttribute("c03");
            cs_03.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C03)).ToString(); ////item.C03;
            odd.Attributes.Append(cs_03);

            var cs_04 = xd.CreateAttribute("c04");
            cs_04.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C04)).ToString(); ////item.C04;
            odd.Attributes.Append(cs_04);

            var cs_05 = xd.CreateAttribute("c05");
            cs_05.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.C05)).ToString(); ////item.C05;
            odd.Attributes.Append(cs_05);
        }

        /// <summary>
        /// News the METHOD_FL.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="item">The match node.</param>
        /// <param name="odd">The Xml Node.</param>
        private void BuildXmlNodeOdd_fl(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var fl_hf = xd.CreateAttribute("hf");
            fl_hf.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.HF)).ToString(); ////item.HF;
            odd.Attributes.Append(fl_hf);

            var fl_hl = xd.CreateAttribute("hl");
            fl_hl.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.HL)).ToString(); //// item.HL;
            odd.Attributes.Append(fl_hl);

            var fl_af = xd.CreateAttribute("af");
            fl_af.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.AF)).ToString(); ////item.AF;
            odd.Attributes.Append(fl_af);

            var fl_al = xd.CreateAttribute("al");
            fl_al.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.AL)).ToString(); ////item.AL;
            odd.Attributes.Append(fl_al);

            var fl_ng = xd.CreateAttribute("ng");
            fl_ng.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.NG)).ToString(); ////item.NG;
            odd.Attributes.Append(fl_ng);
        }

        /// <summary>
        /// Builds the XML node ODD_HTFT.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="item">The match node.</param>
        /// <param name="odd">The Xml Node.</param>
        private void BuildXmlNodeOdd_htft(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var htft_hh = xd.CreateAttribute("hh");
            htft_hh.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.HH)).ToString(); //// item.HH;
            odd.Attributes.Append(htft_hh);

            var htft_hd = xd.CreateAttribute("hd");
            htft_hd.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.HD)).ToString(); ////item.HD;
            odd.Attributes.Append(htft_hd);

            var htft_ha = xd.CreateAttribute("ha");
            htft_ha.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.HA)).ToString(); ////item.HA;
            odd.Attributes.Append(htft_ha);

            var htft_dh = xd.CreateAttribute("dh");
            htft_dh.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.DH)).ToString(); ////item.DH;
            odd.Attributes.Append(htft_dh);

            var htft_dd = xd.CreateAttribute("dd");
            htft_dd.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.DD)).ToString(); ////item.DD;
            odd.Attributes.Append(htft_dd);

            var htft_da = xd.CreateAttribute("da");
            htft_da.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.DA)).ToString(); ////item.DA;
            odd.Attributes.Append(htft_da);

            var htft_ah = xd.CreateAttribute("ah");
            htft_ah.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.AH)).ToString(); ////item.AH;
            odd.Attributes.Append(htft_ah);

            var htft_ad = xd.CreateAttribute("ad");
            htft_ad.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.AD)).ToString(); ////item.AD;
            odd.Attributes.Append(htft_ad);

            var htft_aa = xd.CreateAttribute("aa");
            htft_aa.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.AA)).ToString(); ////item.AA;
            odd.Attributes.Append(htft_aa);
        }

        /// <summary>
        /// Builds the XML node ODD_TG.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="item">The match node.</param>
        /// <param name="odd">The Xml Node.</param>
        private void BuildXmlNodeOdd_tg(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o1234_o1 = xd.CreateAttribute("o1");
            o1234_o1.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.O1)).ToString(); //// item.O1;
            odd.Attributes.Append(o1234_o1);

            var o1234_o2 = xd.CreateAttribute("o2");
            o1234_o2.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.O2)).ToString(); //// item.O2;
            odd.Attributes.Append(o1234_o2);

            var o1234_o3 = xd.CreateAttribute("o3");
            o1234_o3.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.O3)).ToString(); //// item.O3;
            odd.Attributes.Append(o1234_o3);

            var o1234_o4 = xd.CreateAttribute("o4");
            o1234_o4.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.O4)).ToString(); //// item.O4;
            odd.Attributes.Append(o1234_o4);
        }

        /// <summary>
        /// Builds the XML node ODD_OE_1.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="item">The match node.</param>
        /// <param name="odd">The Xml Node.</param>
        private void BuildXmlNodeOdd_oe(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var oe_o = xd.CreateAttribute("o");
            oe_o.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.O)).ToString();
            odd.Attributes.Append(oe_o);

            var oe_e = xd.CreateAttribute("e");
            oe_e.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.E)).ToString();
            odd.Attributes.Append(oe_e);
        }

        /// <summary>
        /// Builds the XML node ODD_ST_1.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="item">The match node.</param>
        /// <param name="odd">The Xml Node.</param>
        private void BuildXmlNodeOdd_st(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o123_o1 = xd.CreateAttribute("o1");
            o123_o1.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.O1)).ToString(); //// item.O1;
            odd.Attributes.Append(o123_o1);

            var o123_o2 = xd.CreateAttribute("o2");
            o123_o2.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.O2)).ToString(); //// item.O2;
            odd.Attributes.Append(o123_o2);

            var o123_o3 = xd.CreateAttribute("o3");
            o123_o3.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.O3)).ToString(); //// item.O3;
            odd.Attributes.Append(o123_o3);
        }

        /// <summary>
        /// Builds the XML node ODD_OU_1.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="item">The match node.</param>
        /// <param name="odd">The Xml Node.</param>
        private void BuildXmlNodeOdd_ah_ou(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var hou_h = xd.CreateAttribute("h");
            hou_h.Value = item.H;
            odd.Attributes.Append(hou_h);

            var hou_o = xd.CreateAttribute("o");
            hou_o.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.O)).ToString();
            odd.Attributes.Append(hou_o);

            var hou_u = xd.CreateAttribute("u");
            hou_u.Value = Utilities.ConvertOdds(op.OddRegion, decimal.Parse(item.U)).ToString();
            odd.Attributes.Append(hou_u);
        }

        #endregion
    }
}