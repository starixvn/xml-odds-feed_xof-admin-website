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
    using OddNodeBuilder;
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
                case "fglg":
                    BuildXmlNodeOdd_fglg(xd, op, item, odd);
                    break;
                case "cs":
                    BuildXmlNodeOdd_cs(xd, op, item, odd);
                    break;
                case "ml":
                    BuildXmlNodeOdd_ml(xd, op, item, odd);
                    break;
                //// Generated by tool
                case "x2":
                    BuildXmlNodeOdd_x2(xd, op, item, odd);
                    break;
                case "dnb":
                    BuildXmlNodeOdd_dnb(xd, op, item, odd);
                    break;
                case "bon":
                    BuildXmlNodeOdd_bon(xd, op, item, odd);
                    break;
                case "twtn":
                    BuildXmlNodeOdd_twtn(xd, op, item, odd);
                    break;
                case "3h":
                    BuildXmlNodeOdd_3h(xd, op, item, odd);
                    break;
                case "ah_2":
                    BuildXmlNodeOdd_ah_2(xd, op, item, odd);
                    break;
                case "ml_1":
                    BuildXmlNodeOdd_ml_1(xd, op, item, odd);
                    break;
                case "so":
                    BuildXmlNodeOdd_so(xd, op, item, odd);
                    break;
                case "cs_1":
                    BuildXmlNodeOdd_cs_1(xd, op, item, odd);
                    break;
                case "hnb":
                    BuildXmlNodeOdd_hnb(xd, op, item, odd);
                    break;
                case "anb":
                    BuildXmlNodeOdd_anb(xd, op, item, odd);
                    break;
                case "dnd":
                    BuildXmlNodeOdd_dnd(xd, op, item, odd);
                    break;
                case "tg_1":
                    BuildXmlNodeOdd_tg_1(xd, op, item, odd);
                    break;
                case "fglg_1":
                    BuildXmlNodeOdd_fglg_1(xd, op, item, odd);
                    break;
                case "oe_12":
                    BuildXmlNodeOdd_oe_12(xd, op, item, odd);
                    break;
                case "h2h":
                    BuildXmlNodeOdd_h2h(xd, op, item, odd);
                    break;
                case "a2h":
                    BuildXmlNodeOdd_a2h(xd, op, item, odd);
                    break;
                case "pen":
                    BuildXmlNodeOdd_pen(xd, op, item, odd);
                    break;
                case "high":
                    BuildXmlNodeOdd_high(xd, op, item, odd);
                    break;
                case "hhigh":
                    BuildXmlNodeOdd_hhigh(xd, op, item, odd);
                    break;
                case "ahigh":
                    BuildXmlNodeOdd_ahigh(xd, op, item, odd);
                    break;
                case "bs":
                    BuildXmlNodeOdd_bs(xd, op, item, odd);
                    break;
                case "bs_2":
                    BuildXmlNodeOdd_bs_2(xd, op, item, odd);
                    break;
                case "hs":
                    BuildXmlNodeOdd_hs(xd, op, item, odd);
                    break;
                case "as":
                    BuildXmlNodeOdd_as(xd, op, item, odd);
                    break;
                case "hw":
                    BuildXmlNodeOdd_hw(xd, op, item, odd);
                    break;
                case "aw":
                    BuildXmlNodeOdd_aw(xd, op, item, odd);
                    break;
                case "x2_1":
                    BuildXmlNodeOdd_x2_1(xd, op, item, odd);
                    break;
                case "tah":
                    BuildXmlNodeOdd_tah(xd, op, item, odd);
                    break;
                case "sw":
                    BuildXmlNodeOdd_sw(xd, op, item, odd);
                    break;
                case "gh":
                    BuildXmlNodeOdd_gh(xd, op, item, odd);
                    break;
                case "gou":
                    BuildXmlNodeOdd_gou(xd, op, item, odd);
                    break;
                case "etg":
                    BuildXmlNodeOdd_etg(xd, op, item, odd);
                    break;
                case "ng":
                    BuildXmlNodeOdd_ng(xd, op, item, odd);
                    break;
                case "heg":
                    BuildXmlNodeOdd_heg(xd, op, item, odd);
                    break;
                case "aeg":
                    BuildXmlNodeOdd_aeg(xd, op, item, odd);
                    break;
                case "sttg":
                    BuildXmlNodeOdd_sttg(xd, op, item, odd);
                    break;
                case "nxtg_ex":
                    BuildXmlNodeOdd_nxtg_ex(xd, op, item, odd);
                    break;
                case "cs_ex1":
                    BuildXmlNodeOdd_cs_ex1(xd, op, item, odd);
                    break;
                case "cs_ex":
                    BuildXmlNodeOdd_cs_ex(xd, op, item, odd);
                    break;
                case "st_ex1":
                    BuildXmlNodeOdd_st_ex1(xd, op, item, odd);
                    break;
                case "nxtrnd":
                    BuildXmlNodeOdd_nxtrnd(xd, op, item, odd);
                    break;
                case "nxtg":
                    BuildXmlNodeOdd_nxtg(xd, op, item, odd);
                    break;
                case "ts":
                    BuildXmlNodeOdd_ts(xd, op, item, odd);
                    break;
                case "win":
                    BuildXmlNodeOdd_win(xd, op, item, odd);
                    break;
                case "stts":
                    BuildXmlNodeOdd_stts(xd, op, item, odd);
                    break;
                case "ex":
                    BuildXmlNodeOdd_ex(xd, op, item, odd);
                    break;
                case "exg":
                    BuildXmlNodeOdd_exg(xd, op, item, odd);
                    break;
                case "mtd":
                    BuildXmlNodeOdd_mtd(xd, op, item, odd);
                    break;
                case "st_10":
                    BuildXmlNodeOdd_st_10(xd, op, item, odd);
                    break;
                case "st_2":
                    BuildXmlNodeOdd_st_2(xd, op, item, odd);
                    break;
                case "ou_2":
                    BuildXmlNodeOdd_ou_2(xd, op, item, odd);
                    break;
                case "egs_1":
                    BuildXmlNodeOdd_egs_1(xd, op, item, odd);
                    break;
                case "ng_1":
                    BuildXmlNodeOdd_ng_1(xd, op, item, odd);
                    break;
                case "heg_1":
                    BuildXmlNodeOdd_heg_1(xd, op, item, odd);
                    break;
                case "aeg_1":
                    BuildXmlNodeOdd_aeg_1(xd, op, item, odd);
                    break;
                case "hdp_2":
                    BuildXmlNodeOdd_hdp_2(xd, op, item, odd);
                    break;
                case "oe_2":
                    BuildXmlNodeOdd_oe_2(xd, op, item, odd);
                    break;
                case "dnb_2":
                    BuildXmlNodeOdd_dnb_2(xd, op, item, odd);
                    break;
                case "x2_2":
                    BuildXmlNodeOdd_x2_2(xd, op, item, odd);
                    break;
                case "eg_2":
                    BuildXmlNodeOdd_eg_2(xd, op, item, odd);
                    break;
                case "b_1":
                    BuildXmlNodeOdd_b_1(xd, op, item, odd);
                    break;
                case "oyn":
                    BuildXmlNodeOdd_oyn(xd, op, item, odd);
                    break;
                case "uyn":
                    BuildXmlNodeOdd_uyn(xd, op, item, odd);
                    break;
                case "dnb_1":
                    BuildXmlNodeOdd_dnb_1(xd, op, item, odd);
                    break;
                case "fg10":
                    BuildXmlNodeOdd_fg10(xd, op, item, odd);
                    break;
                case "fg15":
                    BuildXmlNodeOdd_fg15(xd, op, item, odd);
                    break;
                case "coe":
                    BuildXmlNodeOdd_coe(xd, op, item, odd);
                    break;
                case "hc":
                    BuildXmlNodeOdd_hc(xd, op, item, odd);
                    break;
                case "ac":
                    BuildXmlNodeOdd_ac(xd, op, item, odd);
                    break;
                case "hcou":
                    BuildXmlNodeOdd_hcou(xd, op, item, odd);
                    break;
                case "acou":
                    BuildXmlNodeOdd_acou(xd, op, item, odd);
                    break;
                case "tc":
                    BuildXmlNodeOdd_tc(xd, op, item, odd);
                    break;
                case "ec":
                    BuildXmlNodeOdd_ec(xd, op, item, odd);
                    break;
                case "ec_1":
                    BuildXmlNodeOdd_ec_1(xd, op, item, odd);
                    break;
                case "tc_1":
                    BuildXmlNodeOdd_tc_1(xd, op, item, odd);
                    break;
                case "coe_1":
                    BuildXmlNodeOdd_coe_1(xd, op, item, odd);
                    break;
                case "hcou_1":
                    BuildXmlNodeOdd_hcou_1(xd, op, item, odd);
                    break;
                case "acou_1":
                    BuildXmlNodeOdd_acou_1(xd, op, item, odd);
                    break;
                case "fc":
                    BuildXmlNodeOdd_fc(xd, op, item, odd);
                    break;
                case "fc_1":
                    BuildXmlNodeOdd_fc_1(xd, op, item, odd);
                    break;
                case "lc":
                    BuildXmlNodeOdd_lc(xd, op, item, odd);
                    break;
                case "lc_1":
                    BuildXmlNodeOdd_lc_1(xd, op, item, odd);
                    break;
                case "sent":
                    BuildXmlNodeOdd_sent(xd, op, item, odd);
                    break;
                case "sent_1":
                    BuildXmlNodeOdd_sent_1(xd, op, item, odd);
                    break;
                case "hsent":
                    BuildXmlNodeOdd_hsent(xd, op, item, odd);
                    break;
                case "hsent_1":
                    BuildXmlNodeOdd_hsent_1(xd, op, item, odd);
                    break;
                case "asent":
                    BuildXmlNodeOdd_asent(xd, op, item, odd);
                    break;
                case "asent_1":
                    BuildXmlNodeOdd_asent_1(xd, op, item, odd);
                    break;
                case "cs_12":
                    BuildXmlNodeOdd_cs_12(xd, op, item, odd);
                    break;
                default:
                    BetOddNodeBuilder.BuildXmlOddNodeAttributes(xd, op, item, odd, item.Type);
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
            cs_c00.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C00)).ToString(); ////item.C00;
            odd.Attributes.Append(cs_c00);

            var cs_11 = xd.CreateAttribute("c11");
            cs_11.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C11)).ToString(); ////item.C11;
            odd.Attributes.Append(cs_11);

            var cs_22 = xd.CreateAttribute("c22");
            cs_22.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C22)).ToString(); ////item.C22;
            odd.Attributes.Append(cs_22);

            var cs_33 = xd.CreateAttribute("c33");
            cs_33.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C33)).ToString(); ////item.C33;
            odd.Attributes.Append(cs_33);

            var cs_44 = xd.CreateAttribute("c44");
            cs_44.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C44)).ToString(); ////item.C44;
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
            cs_41.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C41)).ToString(); ////item.C41;
            odd.Attributes.Append(cs_41);

            var cs_42 = xd.CreateAttribute("c42");
            cs_42.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C42)).ToString(); ////item.C42;
            odd.Attributes.Append(cs_42);

            var cs_43 = xd.CreateAttribute("c43");
            cs_43.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C43)).ToString(); ////item.C43;
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
            cs_21.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C21)).ToString(); ////item.C21;
            odd.Attributes.Append(cs_21);

            var cs_31 = xd.CreateAttribute("c31");
            cs_31.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C31)).ToString(); ////item.C31;
            odd.Attributes.Append(cs_31);

            var cs_32 = xd.CreateAttribute("c32");
            cs_32.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C32)).ToString(); ////item.C32;
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
            cs_14.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C14)).ToString(); ////item.C14;
            odd.Attributes.Append(cs_14);

            var cs_24 = xd.CreateAttribute("c24");
            cs_24.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C24)).ToString(); ////item.C24;
            odd.Attributes.Append(cs_24);

            var cs_34 = xd.CreateAttribute("c34");
            cs_34.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C34)).ToString(); ////item.C34;
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
            cs_12.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C12)).ToString(); ////item.C12;
            odd.Attributes.Append(cs_12);

            var cs_13 = xd.CreateAttribute("c13");
            cs_13.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C13)).ToString(); ////item.C13;
            odd.Attributes.Append(cs_13);

            var cs_23 = xd.CreateAttribute("c23");
            cs_23.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C23)).ToString(); ////item.C23;
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
            cs_10.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C10)).ToString(); ////item.C10;
            odd.Attributes.Append(cs_10);

            var cs_20 = xd.CreateAttribute("c20");
            cs_20.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C20)).ToString(); ////item.C20;
            odd.Attributes.Append(cs_20);

            var cs_30 = xd.CreateAttribute("c30");
            cs_30.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C30)).ToString(); ////item.C30;
            odd.Attributes.Append(cs_30);

            var cs_40 = xd.CreateAttribute("c40");
            cs_40.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C40)).ToString(); ////item.C40;
            odd.Attributes.Append(cs_40);

            var cs_50 = xd.CreateAttribute("c50");
            cs_50.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C50)).ToString(); ////item.C50;
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
            cs_01.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C01)).ToString(); ////item.C01;
            odd.Attributes.Append(cs_01);

            var cs_02 = xd.CreateAttribute("c02");
            cs_02.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C02)).ToString(); ////item.C02;
            odd.Attributes.Append(cs_02);

            var cs_03 = xd.CreateAttribute("c03");
            cs_03.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C03)).ToString(); ////item.C03;
            odd.Attributes.Append(cs_03);

            var cs_04 = xd.CreateAttribute("c04");
            cs_04.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C04)).ToString(); ////item.C04;
            odd.Attributes.Append(cs_04);

            var cs_05 = xd.CreateAttribute("c05");
            cs_05.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C05)).ToString(); ////item.C05;
            odd.Attributes.Append(cs_05);
        }

        /// <summary>
        /// News the METHOD_FL.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="item">The match node.</param>
        /// <param name="odd">The Xml Node.</param>
        private void BuildXmlNodeOdd_fglg(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var fl_hf = xd.CreateAttribute("hf");
            fl_hf.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.HF)).ToString(); ////item.HF;
            odd.Attributes.Append(fl_hf);

            var fl_hl = xd.CreateAttribute("hl");
            fl_hl.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.HL)).ToString(); //// item.HL;
            odd.Attributes.Append(fl_hl);

            var fl_af = xd.CreateAttribute("af");
            fl_af.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.AF)).ToString(); ////item.AF;
            odd.Attributes.Append(fl_af);

            var fl_al = xd.CreateAttribute("al");
            fl_al.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.AL)).ToString(); ////item.AL;
            odd.Attributes.Append(fl_al);

            var fl_ng = xd.CreateAttribute("ng");
            fl_ng.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.NG)).ToString(); ////item.NG;
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
            htft_hh.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.HH)).ToString(); //// item.HH;
            odd.Attributes.Append(htft_hh);

            var htft_hd = xd.CreateAttribute("hd");
            htft_hd.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.HD)).ToString(); ////item.HD;
            odd.Attributes.Append(htft_hd);

            var htft_ha = xd.CreateAttribute("ha");
            htft_ha.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.HA)).ToString(); ////item.HA;
            odd.Attributes.Append(htft_ha);

            var htft_dh = xd.CreateAttribute("dh");
            htft_dh.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.DH)).ToString(); ////item.DH;
            odd.Attributes.Append(htft_dh);

            var htft_dd = xd.CreateAttribute("dd");
            htft_dd.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.DD)).ToString(); ////item.DD;
            odd.Attributes.Append(htft_dd);

            var htft_da = xd.CreateAttribute("da");
            htft_da.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.DA)).ToString(); ////item.DA;
            odd.Attributes.Append(htft_da);

            var htft_ah = xd.CreateAttribute("ah");
            htft_ah.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.AH)).ToString(); ////item.AH;
            odd.Attributes.Append(htft_ah);

            var htft_ad = xd.CreateAttribute("ad");
            htft_ad.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.AD)).ToString(); ////item.AD;
            odd.Attributes.Append(htft_ad);

            var htft_aa = xd.CreateAttribute("aa");
            htft_aa.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.AA)).ToString(); ////item.AA;
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
            o1234_o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O1)).ToString(); //// item.O1;
            odd.Attributes.Append(o1234_o1);

            var o1234_o2 = xd.CreateAttribute("o2");
            o1234_o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O2)).ToString(); //// item.O2;
            odd.Attributes.Append(o1234_o2);

            var o1234_o3 = xd.CreateAttribute("o3");
            o1234_o3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O3)).ToString(); //// item.O3;
            odd.Attributes.Append(o1234_o3);

            var o1234_o4 = xd.CreateAttribute("o4");
            o1234_o4.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O4)).ToString(); //// item.O4;
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
            oe_o.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O)).ToString();
            odd.Attributes.Append(oe_o);

            var oe_e = xd.CreateAttribute("e");
            oe_e.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.E)).ToString();
            odd.Attributes.Append(oe_e);
        }

        /// <summary>
        /// Builds the XML node Money line.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="op">The odd profile.</param>
        /// <param name="item">The match node.</param>
        /// <param name="odd">The Xml Node.</param>
        private void BuildXmlNodeOdd_ml(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o1 = xd.CreateAttribute("o1");
            o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O1)).ToString();
            odd.Attributes.Append(o1);

            var o2 = xd.CreateAttribute("o2");
            o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O2)).ToString();
            odd.Attributes.Append(o2);
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
            o123_o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O1)).ToString(); //// item.O1;
            odd.Attributes.Append(o123_o1);

            var o123_o2 = xd.CreateAttribute("o2");
            o123_o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O2)).ToString(); //// item.O2;
            odd.Attributes.Append(o123_o2);

            var o123_o3 = xd.CreateAttribute("o3");
            o123_o3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O3)).ToString(); //// item.O3;
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
            hou_o.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O)).ToString();
            odd.Attributes.Append(hou_o);

            var hou_u = xd.CreateAttribute("u");
            hou_u.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.U)).ToString();
            odd.Attributes.Append(hou_u);
        }

        // Generated by tool
        private void BuildXmlNodeOdd_ml_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o1 = xd.CreateAttribute("o1");
            o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O1)).ToString();
            odd.Attributes.Append(o1);

            var o2 = xd.CreateAttribute("o2");
            o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O2)).ToString();
            odd.Attributes.Append(o2);
        }

        private void BuildXmlNodeOdd_so(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var hy = xd.CreateAttribute("hy");
            hy.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.HY)).ToString();
            odd.Attributes.Append(hy);

            var hn = xd.CreateAttribute("hn");
            hn.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.HN)).ToString();
            odd.Attributes.Append(hn);

            var ay = xd.CreateAttribute("ay");
            ay.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.AY)).ToString();
            odd.Attributes.Append(ay);

            var an = xd.CreateAttribute("an");
            an.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.AN)).ToString();
            odd.Attributes.Append(an);
        }

        private void BuildXmlNodeOdd_cs_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var c10 = xd.CreateAttribute("c10");
            c10.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C10)).ToString();
            odd.Attributes.Append(c10);
            var c20 = xd.CreateAttribute("c20");
            c20.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C20)).ToString();
            odd.Attributes.Append(c20);
            var c21 = xd.CreateAttribute("c21");
            c21.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C21)).ToString();
            odd.Attributes.Append(c21);
            var c30 = xd.CreateAttribute("c30");
            c30.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C30)).ToString();
            odd.Attributes.Append(c30);
            var c31 = xd.CreateAttribute("c31");
            c31.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C31)).ToString();
            odd.Attributes.Append(c31);
            var c32 = xd.CreateAttribute("c32");
            c32.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C32)).ToString();
            odd.Attributes.Append(c32);
            var c40 = xd.CreateAttribute("c40");
            c40.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C40)).ToString();
            odd.Attributes.Append(c40);
            var c01 = xd.CreateAttribute("c01");
            c01.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C01)).ToString();
            odd.Attributes.Append(c01);
            var c02 = xd.CreateAttribute("c02");
            c02.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C02)).ToString();
            odd.Attributes.Append(c02);
            var c12 = xd.CreateAttribute("c12");
            c12.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C12)).ToString();
            odd.Attributes.Append(c12);
            var c03 = xd.CreateAttribute("c03");
            c03.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C03)).ToString();
            odd.Attributes.Append(c03);
            var c13 = xd.CreateAttribute("c13");
            c13.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C13)).ToString();
            odd.Attributes.Append(c13);
            var c23 = xd.CreateAttribute("c23");
            c23.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C23)).ToString();
            odd.Attributes.Append(c23);
            var c04 = xd.CreateAttribute("c04");
            c04.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C04)).ToString();
            odd.Attributes.Append(c04);
            var c00 = xd.CreateAttribute("c00");
            c00.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C00)).ToString();
            odd.Attributes.Append(c00);
            var c11 = xd.CreateAttribute("c11");
            c11.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C11)).ToString();
            odd.Attributes.Append(c11);
            var c22 = xd.CreateAttribute("c22");
            c22.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C22)).ToString();
            odd.Attributes.Append(c22);
            var c33 = xd.CreateAttribute("c33");
            c33.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.C33)).ToString();
            odd.Attributes.Append(c33);
        }

        private void BuildXmlNodeOdd_hnb(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var a = xd.CreateAttribute("a");
            a.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A)).ToString();
            odd.Attributes.Append(a);
            var d = xd.CreateAttribute("d");
            d.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.D)).ToString();
            odd.Attributes.Append(d);
        }

        private void BuildXmlNodeOdd_anb(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h = xd.CreateAttribute("h");
            h.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H)).ToString();
            odd.Attributes.Append(h);
            var d = xd.CreateAttribute("d");
            d.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.D)).ToString();
            odd.Attributes.Append(d);
        }

        private void BuildXmlNodeOdd_dnd(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var d = xd.CreateAttribute("d");
            d.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.D)).ToString();
            odd.Attributes.Append(d);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_tg_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o1 = xd.CreateAttribute("o1");
            o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O1)).ToString();
            odd.Attributes.Append(o1);
            var o2 = xd.CreateAttribute("o2");
            o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O2)).ToString();
            odd.Attributes.Append(o2);
            var o3 = xd.CreateAttribute("o3");
            o3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O3)).ToString();
            odd.Attributes.Append(o3);
        }

        private void BuildXmlNodeOdd_fglg_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var hf = xd.CreateAttribute("hf");
            hf.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.HF)).ToString();
            odd.Attributes.Append(hf);
            var hl = xd.CreateAttribute("hl");
            hl.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.HL)).ToString();
            odd.Attributes.Append(hl);
            var af = xd.CreateAttribute("af");
            af.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.AF)).ToString();
            odd.Attributes.Append(af);
            var al = xd.CreateAttribute("al");
            al.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.AL)).ToString();
            odd.Attributes.Append(al);
            var ng = xd.CreateAttribute("ng");
            ng.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.NG)).ToString();
            odd.Attributes.Append(ng);
        }

        private void BuildXmlNodeOdd_oe_12(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var oe = xd.CreateAttribute("oe");
            oe.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.OE)).ToString();
            odd.Attributes.Append(oe);
            var eo = xd.CreateAttribute("eo");
            eo.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.EO)).ToString();
            odd.Attributes.Append(eo);
            var oo = xd.CreateAttribute("oo");
            oo.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.OO)).ToString();
            odd.Attributes.Append(oo);
            var ee = xd.CreateAttribute("ee");
            ee.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.EE)).ToString();
            odd.Attributes.Append(ee);
        }

        private void BuildXmlNodeOdd_h2h(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var y = xd.CreateAttribute("y");
            y.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Y)).ToString();
            odd.Attributes.Append(y);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_a2h(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var y = xd.CreateAttribute("y");
            y.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Y)).ToString();
            odd.Attributes.Append(y);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_pen(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var y = xd.CreateAttribute("y");
            y.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Y)).ToString();
            odd.Attributes.Append(y);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_high(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h1 = xd.CreateAttribute("h1");
            h1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.FirstH)).ToString();
            odd.Attributes.Append(h1);
            var h2 = xd.CreateAttribute("h2");
            h2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.SecondH)).ToString();
            odd.Attributes.Append(h2);
            var t = xd.CreateAttribute("tie");
            t.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.T)).ToString();
            odd.Attributes.Append(t);
        }

        private void BuildXmlNodeOdd_hhigh(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var firstH = xd.CreateAttribute("h1");
            firstH.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.FirstH)).ToString();
            odd.Attributes.Append(firstH);

            var secondH = xd.CreateAttribute("h2");
            secondH.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.SecondH)).ToString();
            odd.Attributes.Append(secondH);

            var tie = xd.CreateAttribute("tie");
            tie.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.T)).ToString();
            odd.Attributes.Append(tie);
        }

        private void BuildXmlNodeOdd_ahigh(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var firstH = xd.CreateAttribute("h1");
            firstH.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.FirstH)).ToString();
            odd.Attributes.Append(firstH);

            var secondH = xd.CreateAttribute("h2");
            secondH.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.SecondH)).ToString();
            odd.Attributes.Append(secondH);

            var tie = xd.CreateAttribute("tie");
            tie.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.T)).ToString();
            odd.Attributes.Append(tie);
        }

        private void BuildXmlNodeOdd_bs(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var y = xd.CreateAttribute("y");
            y.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Y)).ToString();
            odd.Attributes.Append(y);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_bs_2(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var y = xd.CreateAttribute("y");
            y.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Y)).ToString();
            odd.Attributes.Append(y);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_hs(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var y = xd.CreateAttribute("y");
            y.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Y)).ToString();
            odd.Attributes.Append(y);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_as(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var y = xd.CreateAttribute("y");
            y.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Y)).ToString();
            odd.Attributes.Append(y);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_hw(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var y = xd.CreateAttribute("y");
            y.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Y)).ToString();
            odd.Attributes.Append(y);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_aw(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var y = xd.CreateAttribute("y");
            y.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Y)).ToString();
            odd.Attributes.Append(y);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_x2_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var hd = xd.CreateAttribute("hd");
            hd.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.HD)).ToString();
            odd.Attributes.Append(hd);
            var ha = xd.CreateAttribute("ha");
            ha.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.HA)).ToString();
            odd.Attributes.Append(ha);
            var ad = xd.CreateAttribute("ad");
            ad.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.AD)).ToString();
            odd.Attributes.Append(ad);
        }

        private void BuildXmlNodeOdd_tah(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o = xd.CreateAttribute("o");
            o.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O)).ToString();
            odd.Attributes.Append(o);
            var u = xd.CreateAttribute("u");
            u.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.U)).ToString();
            odd.Attributes.Append(u);
            var h = xd.CreateAttribute("h");
            h.Value = Convert.ToDecimal(item.H).ToString();
            odd.Attributes.Append(h);
        }

        private void BuildXmlNodeOdd_sw(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h = xd.CreateAttribute("h");
            h.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H)).ToString();
            odd.Attributes.Append(h);
            var a = xd.CreateAttribute("a");
            a.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A)).ToString();
            odd.Attributes.Append(a);
            var x = xd.CreateAttribute("x");
            x.Value = item.X;
            odd.Attributes.Append(x);
        }

        private void BuildXmlNodeOdd_gh(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o = xd.CreateAttribute("o");
            o.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O)).ToString();
            odd.Attributes.Append(o);
            var u = xd.CreateAttribute("u");
            u.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.U)).ToString();
            odd.Attributes.Append(u);

            var h = xd.CreateAttribute("h");
            h.Value = Convert.ToDecimal(item.H).ToString();
            odd.Attributes.Append(h);

            var x = xd.CreateAttribute("x");
            x.Value = item.X;
            odd.Attributes.Append(x);
        }

        private void BuildXmlNodeOdd_gou(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o = xd.CreateAttribute("o");
            o.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O)).ToString();
            odd.Attributes.Append(o);
            var u = xd.CreateAttribute("u");
            u.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.U)).ToString();
            odd.Attributes.Append(u);

            var h = xd.CreateAttribute("h");
            h.Value = Convert.ToDecimal(item.H).ToString();
            odd.Attributes.Append(h);

            var x = xd.CreateAttribute("x");
            x.Value = item.X;
            odd.Attributes.Append(x);
        }

        private void BuildXmlNodeOdd_etg(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o01 = xd.CreateAttribute("o01");
            o01.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O01)).ToString();
            odd.Attributes.Append(o01);
            var o02 = xd.CreateAttribute("o02");
            o02.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O02)).ToString();
            odd.Attributes.Append(o02);
            var o03 = xd.CreateAttribute("o03");
            o03.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O03)).ToString();
            odd.Attributes.Append(o03);
            var o04 = xd.CreateAttribute("o04");
            o04.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O04)).ToString();
            odd.Attributes.Append(o04);
            var o05 = xd.CreateAttribute("o05");
            o05.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O05)).ToString();
            odd.Attributes.Append(o05);
            var o06 = xd.CreateAttribute("o06");
            o06.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O06)).ToString();
            odd.Attributes.Append(o06);
            var o07 = xd.CreateAttribute("o07");
            o07.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O07)).ToString();
            odd.Attributes.Append(o07);
        }

        private void BuildXmlNodeOdd_ng(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h = xd.CreateAttribute("h");
            h.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H)).ToString();
            odd.Attributes.Append(h);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
            var a = xd.CreateAttribute("a");
            a.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A)).ToString();
            odd.Attributes.Append(a);
        }

        private void BuildXmlNodeOdd_heg(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o1 = xd.CreateAttribute("o1");
            o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O1)).ToString();
            odd.Attributes.Append(o1);
            var o2 = xd.CreateAttribute("o2");
            o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O2)).ToString();
            odd.Attributes.Append(o2);
            var o3 = xd.CreateAttribute("o3");
            o3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O3)).ToString();
            odd.Attributes.Append(o3);
            var o4 = xd.CreateAttribute("o4");
            o4.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O4)).ToString();
            odd.Attributes.Append(o4);
        }

        private void BuildXmlNodeOdd_aeg(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o1 = xd.CreateAttribute("o1");
            o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O1)).ToString();
            odd.Attributes.Append(o1);
            var o2 = xd.CreateAttribute("o2");
            o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O2)).ToString();
            odd.Attributes.Append(o2);
            var o3 = xd.CreateAttribute("o3");
            o3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O3)).ToString();
            odd.Attributes.Append(o3);
            var o4 = xd.CreateAttribute("o4");
            o4.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O4)).ToString();
            odd.Attributes.Append(o4);
        }

        private void BuildXmlNodeOdd_sttg(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var ho = xd.CreateAttribute("ho");
            ho.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.HO)).ToString();
            odd.Attributes.Append(ho);

            var hu = xd.CreateAttribute("hu");
            hu.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.HU)).ToString();
            odd.Attributes.Append(hu);

            var ao = xd.CreateAttribute("ao");
            ao.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.AO)).ToString();
            odd.Attributes.Append(ao);

            var au = xd.CreateAttribute("au");
            au.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.AU)).ToString();
            odd.Attributes.Append(au);

            var @do = xd.CreateAttribute("do");
            @do.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.DO)).ToString();
            odd.Attributes.Append(@do);

            var du = xd.CreateAttribute("du");
            du.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.DU)).ToString();
            odd.Attributes.Append(du);

            var h = xd.CreateAttribute("h");
            h.Value = Convert.ToDecimal(item.H).ToString();
            odd.Attributes.Append(h);
        }

        private void BuildXmlNodeOdd_nxtg_ex(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h = xd.CreateAttribute("h");
            h.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H)).ToString();
            odd.Attributes.Append(h);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
            var a = xd.CreateAttribute("a");
            a.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A)).ToString();
            odd.Attributes.Append(a);
        }

        private void BuildXmlNodeOdd_cs_ex1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var cs00 = xd.CreateAttribute("cs00");
            cs00.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.CS00)).ToString();
            odd.Attributes.Append(cs00);
            var cs10 = xd.CreateAttribute("cs10");
            cs10.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.CS10)).ToString();
            odd.Attributes.Append(cs10);
            var cs01 = xd.CreateAttribute("cs01");
            cs01.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.CS01)).ToString();
            odd.Attributes.Append(cs01);
            var cs11 = xd.CreateAttribute("cs11");
            cs11.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.CS11)).ToString();
            odd.Attributes.Append(cs11);
            var cs20 = xd.CreateAttribute("cs20");
            cs20.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.CS20)).ToString();
            odd.Attributes.Append(cs20);
            var cs02 = xd.CreateAttribute("cs02");
            cs02.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.CS02)).ToString();
            odd.Attributes.Append(cs02);
        }

        private void BuildXmlNodeOdd_cs_ex(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var cs00 = xd.CreateAttribute("cs00");
            cs00.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.CS00)).ToString();
            odd.Attributes.Append(cs00);
            var cs10 = xd.CreateAttribute("cs10");
            cs10.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.CS10)).ToString();
            odd.Attributes.Append(cs10);
            var cs01 = xd.CreateAttribute("cs01");
            cs01.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.CS01)).ToString();
            odd.Attributes.Append(cs01);
            var cs11 = xd.CreateAttribute("cs11");
            cs11.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.CS11)).ToString();
            odd.Attributes.Append(cs11);
            var cs20 = xd.CreateAttribute("cs20");
            cs20.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.CS20)).ToString();
            odd.Attributes.Append(cs20);
            var cs02 = xd.CreateAttribute("cs02");
            cs02.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.CS02)).ToString();
            odd.Attributes.Append(cs02);
            var cs21 = xd.CreateAttribute("cs21");
            cs21.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.CS21)).ToString();
            odd.Attributes.Append(cs21);
            var cs12 = xd.CreateAttribute("cs12");
            cs12.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.CS12)).ToString();
            odd.Attributes.Append(cs12);
            var cs30 = xd.CreateAttribute("cs30");
            cs30.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.CS30)).ToString();
            odd.Attributes.Append(cs30);
            var cs03 = xd.CreateAttribute("cs03");
            cs03.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.CS03)).ToString();
            odd.Attributes.Append(cs03);
        }

        private void BuildXmlNodeOdd_st_ex1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h = xd.CreateAttribute("h");
            h.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H)).ToString();
            odd.Attributes.Append(h);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
            var a = xd.CreateAttribute("a");
            a.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A)).ToString();
            odd.Attributes.Append(a);
        }

        private void BuildXmlNodeOdd_nxtrnd(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h = xd.CreateAttribute("h");
            h.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H)).ToString();
            odd.Attributes.Append(h);
            var a = xd.CreateAttribute("a");
            a.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A)).ToString();
            odd.Attributes.Append(a);
        }

        private void BuildXmlNodeOdd_nxtg(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o1 = xd.CreateAttribute("o1");
            o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O1)).ToString();
            odd.Attributes.Append(o1);
            var o2 = xd.CreateAttribute("o2");
            o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O2)).ToString();
            odd.Attributes.Append(o2);
            var o3 = xd.CreateAttribute("o3");
            o3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O3)).ToString();
            odd.Attributes.Append(o3);
            var o4 = xd.CreateAttribute("o4");
            o4.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O4)).ToString();
            odd.Attributes.Append(o4);
            var o5 = xd.CreateAttribute("o5");
            o5.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O5)).ToString();
            odd.Attributes.Append(o5);
            var o6 = xd.CreateAttribute("o6");
            o6.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O6)).ToString();
            odd.Attributes.Append(o6);
            var o7 = xd.CreateAttribute("o7");
            o7.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O7)).ToString();
            odd.Attributes.Append(o7);
        }

        private void BuildXmlNodeOdd_ts(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h = xd.CreateAttribute("h");
            h.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H)).ToString();
            odd.Attributes.Append(h);
            var a = xd.CreateAttribute("a");
            a.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A)).ToString();
            odd.Attributes.Append(a);
            var b = xd.CreateAttribute("b");
            b.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.B)).ToString();
            odd.Attributes.Append(b);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_win(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h1 = xd.CreateAttribute("h1");
            h1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H1)).ToString();
            odd.Attributes.Append(h1);
            var h2 = xd.CreateAttribute("h2");
            h2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H2)).ToString();
            odd.Attributes.Append(h2);
            var h3 = xd.CreateAttribute("h3");
            h3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H3)).ToString();
            odd.Attributes.Append(h3);
            var d = xd.CreateAttribute("d");
            d.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.D)).ToString();
            odd.Attributes.Append(d);
            var a1 = xd.CreateAttribute("a1");
            a1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A1)).ToString();
            odd.Attributes.Append(a1);
            var a2 = xd.CreateAttribute("a2");
            a2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A2)).ToString();
            odd.Attributes.Append(a2);
            var a3 = xd.CreateAttribute("a3");
            a3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A3)).ToString();
            odd.Attributes.Append(a3);
        }

        private void BuildXmlNodeOdd_stts(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var hh = xd.CreateAttribute("hh");
            hh.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.HH)).ToString();
            odd.Attributes.Append(hh);
            var dh = xd.CreateAttribute("dh");
            dh.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.DH)).ToString();
            odd.Attributes.Append(dh);
            var ah = xd.CreateAttribute("ah");
            ah.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.AH)).ToString();
            odd.Attributes.Append(ah);
            var ha = xd.CreateAttribute("ha");
            ha.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.HA)).ToString();
            odd.Attributes.Append(ha);
            var da = xd.CreateAttribute("da");
            da.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.DA)).ToString();
            odd.Attributes.Append(da);
            var aa = xd.CreateAttribute("aa");
            aa.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.AA)).ToString();
            odd.Attributes.Append(aa);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_ex(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var y = xd.CreateAttribute("y");
            y.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Y)).ToString();
            odd.Attributes.Append(y);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_exg(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var y = xd.CreateAttribute("y");
            y.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Y)).ToString();
            odd.Attributes.Append(y);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_mtd(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h1 = xd.CreateAttribute("h1");
            h1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H1)).ToString();
            odd.Attributes.Append(h1);
            var h2 = xd.CreateAttribute("h2");
            h2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H2)).ToString();
            odd.Attributes.Append(h2);
            var h3 = xd.CreateAttribute("h3");
            h3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H3)).ToString();
            odd.Attributes.Append(h3);
            var a1 = xd.CreateAttribute("a1");
            a1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A1)).ToString();
            odd.Attributes.Append(a1);
            var a2 = xd.CreateAttribute("a2");
            a2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A2)).ToString();
            odd.Attributes.Append(a2);
            var a3 = xd.CreateAttribute("a3");
            a3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A3)).ToString();
            odd.Attributes.Append(a3);
        }

        private void BuildXmlNodeOdd_st_10(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h = xd.CreateAttribute("h");
            h.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H)).ToString();
            odd.Attributes.Append(h);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
            var a = xd.CreateAttribute("a");
            a.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A)).ToString();
            odd.Attributes.Append(a);
        }

        private void BuildXmlNodeOdd_st_2(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h = xd.CreateAttribute("h");
            h.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H)).ToString();
            odd.Attributes.Append(h);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
            var a = xd.CreateAttribute("a");
            a.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A)).ToString();
            odd.Attributes.Append(a);
        }

        private void BuildXmlNodeOdd_egs_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o1 = xd.CreateAttribute("o1");
            o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O1)).ToString();
            odd.Attributes.Append(o1);
            var o2 = xd.CreateAttribute("o2");
            o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O2)).ToString();
            odd.Attributes.Append(o2);
            var o3 = xd.CreateAttribute("o3");
            o3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O3)).ToString();
            odd.Attributes.Append(o3);
            var o4 = xd.CreateAttribute("o4");
            o4.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O4)).ToString();
            odd.Attributes.Append(o4);
            var o5 = xd.CreateAttribute("o5");
            o5.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O5)).ToString();
            odd.Attributes.Append(o5);
        }

        private void BuildXmlNodeOdd_ng_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h = xd.CreateAttribute("h");
            h.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H)).ToString();
            odd.Attributes.Append(h);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
            var a = xd.CreateAttribute("a");
            a.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A)).ToString();
            odd.Attributes.Append(a);
        }

        private void BuildXmlNodeOdd_heg_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o1 = xd.CreateAttribute("o1");
            o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O1)).ToString();
            odd.Attributes.Append(o1);
            var o2 = xd.CreateAttribute("o2");
            o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O2)).ToString();
            odd.Attributes.Append(o2);
            var o3 = xd.CreateAttribute("o3");
            o3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O3)).ToString();
            odd.Attributes.Append(o3);
            var o4 = xd.CreateAttribute("o4");
            o4.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O4)).ToString();
            odd.Attributes.Append(o4);
        }

        private void BuildXmlNodeOdd_aeg_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o1 = xd.CreateAttribute("o1");
            o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O1)).ToString();
            odd.Attributes.Append(o1);
            var o2 = xd.CreateAttribute("o2");
            o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O2)).ToString();
            odd.Attributes.Append(o2);
            var o3 = xd.CreateAttribute("o3");
            o3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O3)).ToString();
            odd.Attributes.Append(o3);
            var o4 = xd.CreateAttribute("o4");
            o4.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O4)).ToString();
            odd.Attributes.Append(o4);
        }

        private void BuildXmlNodeOdd_hdp_2(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o = xd.CreateAttribute("o");
            o.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O)).ToString();
            odd.Attributes.Append(o);
            var u = xd.CreateAttribute("u");
            u.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.U)).ToString();
            odd.Attributes.Append(u);

            var h = xd.CreateAttribute("h");
            h.Value = Convert.ToDecimal(item.H).ToString();
            odd.Attributes.Append(h);
        }

        private void BuildXmlNodeOdd_oe_2(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o = xd.CreateAttribute("o");
            o.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O)).ToString();
            odd.Attributes.Append(o);
            var e = xd.CreateAttribute("e");
            e.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.E)).ToString();
            odd.Attributes.Append(e);
        }

        private void BuildXmlNodeOdd_dnb_2(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h = xd.CreateAttribute("h");
            h.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H)).ToString();
            odd.Attributes.Append(h);
            var a = xd.CreateAttribute("a");
            a.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A)).ToString();
            odd.Attributes.Append(a);
        }

        private void BuildXmlNodeOdd_x2_2(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var hd = xd.CreateAttribute("hd");
            hd.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.HD)).ToString();
            odd.Attributes.Append(hd);
            var ha = xd.CreateAttribute("ha");
            ha.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.HA)).ToString();
            odd.Attributes.Append(ha);
            var ad = xd.CreateAttribute("ad");
            ad.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.AD)).ToString();
            odd.Attributes.Append(ad);
        }

        private void BuildXmlNodeOdd_eg_2(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o1 = xd.CreateAttribute("o1");
            o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O1)).ToString();
            odd.Attributes.Append(o1);
            var o2 = xd.CreateAttribute("o2");
            o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O2)).ToString();
            odd.Attributes.Append(o2);
            var o3 = xd.CreateAttribute("o3");
            o3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O3)).ToString();
            odd.Attributes.Append(o3);
        }

        private void BuildXmlNodeOdd_b_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var y = xd.CreateAttribute("y");
            y.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Y)).ToString();
            odd.Attributes.Append(y);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_oyn(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var y = xd.CreateAttribute("y");
            y.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Y)).ToString();
            odd.Attributes.Append(y);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_uyn(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var y = xd.CreateAttribute("y");
            y.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Y)).ToString();
            odd.Attributes.Append(y);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_dnb_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h = xd.CreateAttribute("h");
            h.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H)).ToString();
            odd.Attributes.Append(h);
            var a = xd.CreateAttribute("a");
            a.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A)).ToString();
            odd.Attributes.Append(a);
        }

        private void BuildXmlNodeOdd_fg10(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o1 = xd.CreateAttribute("o1");
            o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O1)).ToString();
            odd.Attributes.Append(o1);
            var o2 = xd.CreateAttribute("o2");
            o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O2)).ToString();
            odd.Attributes.Append(o2);
            var o3 = xd.CreateAttribute("o3");
            o3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O3)).ToString();
            odd.Attributes.Append(o3);
            var o4 = xd.CreateAttribute("o4");
            o4.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O4)).ToString();
            odd.Attributes.Append(o4);
            var o5 = xd.CreateAttribute("o5");
            o5.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O5)).ToString();
            odd.Attributes.Append(o5);
            var o6 = xd.CreateAttribute("o6");
            o6.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O6)).ToString();
            odd.Attributes.Append(o6);
            var o7 = xd.CreateAttribute("o7");
            o7.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O7)).ToString();
            odd.Attributes.Append(o7);
            var o8 = xd.CreateAttribute("o8");
            o8.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O8)).ToString();
            odd.Attributes.Append(o8);
            var o9 = xd.CreateAttribute("o9");
            o9.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O9)).ToString();
            odd.Attributes.Append(o9);
            var o10 = xd.CreateAttribute("o10");
            o10.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O10)).ToString();
            odd.Attributes.Append(o10);
        }

        private void BuildXmlNodeOdd_fg15(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o1 = xd.CreateAttribute("o1");
            o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O1)).ToString();
            odd.Attributes.Append(o1);
            var o2 = xd.CreateAttribute("o2");
            o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O2)).ToString();
            odd.Attributes.Append(o2);
            var o3 = xd.CreateAttribute("o3");
            o3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O3)).ToString();
            odd.Attributes.Append(o3);
            var o4 = xd.CreateAttribute("o4");
            o4.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O4)).ToString();
            odd.Attributes.Append(o4);
            var o5 = xd.CreateAttribute("o5");
            o5.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O5)).ToString();
            odd.Attributes.Append(o5);
            var o6 = xd.CreateAttribute("o6");
            o6.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O6)).ToString();
            odd.Attributes.Append(o6);
            var o7 = xd.CreateAttribute("o7");
            o7.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O7)).ToString();
            odd.Attributes.Append(o7);
        }

        private void BuildXmlNodeOdd_coe(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o = xd.CreateAttribute("o");
            o.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O)).ToString();
            odd.Attributes.Append(o);
            var e = xd.CreateAttribute("e");
            e.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.E)).ToString();
            odd.Attributes.Append(e);
        }

        private void BuildXmlNodeOdd_hc(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o1 = xd.CreateAttribute("o1");
            o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O1)).ToString();
            odd.Attributes.Append(o1);
            var o2 = xd.CreateAttribute("o2");
            o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O2)).ToString();
            odd.Attributes.Append(o2);
            var o3 = xd.CreateAttribute("o3");
            o3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O3)).ToString();
            odd.Attributes.Append(o3);
            var o4 = xd.CreateAttribute("o4");
            o4.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O4)).ToString();
            odd.Attributes.Append(o4);
        }

        private void BuildXmlNodeOdd_ac(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o1 = xd.CreateAttribute("o1");
            o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O1)).ToString();
            odd.Attributes.Append(o1);
            var o2 = xd.CreateAttribute("o2");
            o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O2)).ToString();
            odd.Attributes.Append(o2);
            var o3 = xd.CreateAttribute("o3");
            o3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O3)).ToString();
            odd.Attributes.Append(o3);
            var o4 = xd.CreateAttribute("o4");
            o4.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O4)).ToString();
            odd.Attributes.Append(o4);
        }

        private void BuildXmlNodeOdd_hcou(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o = xd.CreateAttribute("o");
            o.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O)).ToString();
            odd.Attributes.Append(o);
            var u = xd.CreateAttribute("u");
            u.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.U)).ToString();
            odd.Attributes.Append(u);

            var h = xd.CreateAttribute("h");
            h.Value = Convert.ToDecimal(item.H).ToString();
            odd.Attributes.Append(h);
        }

        private void BuildXmlNodeOdd_acou(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o = xd.CreateAttribute("o");
            o.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O)).ToString();
            odd.Attributes.Append(o);
            var u = xd.CreateAttribute("u");
            u.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.U)).ToString();
            odd.Attributes.Append(u);

            var h = xd.CreateAttribute("h");
            h.Value = Convert.ToDecimal(item.H).ToString();
            odd.Attributes.Append(h);
        }

        private void BuildXmlNodeOdd_tc(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o1 = xd.CreateAttribute("o1");
            o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O1)).ToString();
            odd.Attributes.Append(o1);
            var o2 = xd.CreateAttribute("o2");
            o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O2)).ToString();
            odd.Attributes.Append(o2);
            var o3 = xd.CreateAttribute("o3");
            o3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O3)).ToString();
            odd.Attributes.Append(o3);
        }

        private void BuildXmlNodeOdd_ec(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o1 = xd.CreateAttribute("o1");
            o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O1)).ToString();
            odd.Attributes.Append(o1);
            var o2 = xd.CreateAttribute("o2");
            o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O2)).ToString();
            odd.Attributes.Append(o2);
            var o3 = xd.CreateAttribute("o3");
            o3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O3)).ToString();
            odd.Attributes.Append(o3);
            var o4 = xd.CreateAttribute("o4");
            o4.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O4)).ToString();
            odd.Attributes.Append(o4);
        }

        private void BuildXmlNodeOdd_ec_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o1 = xd.CreateAttribute("o1");
            o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O1)).ToString();
            odd.Attributes.Append(o1);
            var o2 = xd.CreateAttribute("o2");
            o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O2)).ToString();
            odd.Attributes.Append(o2);
            var o3 = xd.CreateAttribute("o3");
            o3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O3)).ToString();
            odd.Attributes.Append(o3);
            var o4 = xd.CreateAttribute("o4");
            o4.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O4)).ToString();
            odd.Attributes.Append(o4);
        }

        private void BuildXmlNodeOdd_tc_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o1 = xd.CreateAttribute("o1");
            o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O1)).ToString();
            odd.Attributes.Append(o1);
            var o2 = xd.CreateAttribute("o2");
            o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O2)).ToString();
            odd.Attributes.Append(o2);
            var o3 = xd.CreateAttribute("o3");
            o3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O3)).ToString();
            odd.Attributes.Append(o3);
        }

        private void BuildXmlNodeOdd_coe_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o = xd.CreateAttribute("o");
            o.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O)).ToString();
            odd.Attributes.Append(o);
            var e = xd.CreateAttribute("e");
            e.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.E)).ToString();
            odd.Attributes.Append(e);
        }

        private void BuildXmlNodeOdd_hcou_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o = xd.CreateAttribute("o");
            o.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O)).ToString();
            odd.Attributes.Append(o);
            var u = xd.CreateAttribute("u");
            u.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.U)).ToString();
            odd.Attributes.Append(u);

            var h = xd.CreateAttribute("h");
            h.Value = Convert.ToDecimal(item.H).ToString();
            odd.Attributes.Append(h);
        }

        private void BuildXmlNodeOdd_acou_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o = xd.CreateAttribute("o");
            o.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O)).ToString();
            odd.Attributes.Append(o);
            var u = xd.CreateAttribute("u");
            u.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.U)).ToString();
            odd.Attributes.Append(u);

            var h = xd.CreateAttribute("h");
            h.Value = Convert.ToDecimal(item.H).ToString();
            odd.Attributes.Append(h);
        }

        private void BuildXmlNodeOdd_fc(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h = xd.CreateAttribute("h");
            h.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H)).ToString();
            odd.Attributes.Append(h);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
            var a = xd.CreateAttribute("a");
            a.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A)).ToString();
            odd.Attributes.Append(a);
        }

        private void BuildXmlNodeOdd_fc_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h = xd.CreateAttribute("h");
            h.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H)).ToString();
            odd.Attributes.Append(h);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
            var a = xd.CreateAttribute("a");
            a.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A)).ToString();
            odd.Attributes.Append(a);
        }

        private void BuildXmlNodeOdd_lc(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h = xd.CreateAttribute("h");
            h.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H)).ToString();
            odd.Attributes.Append(h);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
            var a = xd.CreateAttribute("a");
            a.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A)).ToString();
            odd.Attributes.Append(a);
        }

        private void BuildXmlNodeOdd_lc_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h = xd.CreateAttribute("h");
            h.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H)).ToString();
            odd.Attributes.Append(h);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
            var a = xd.CreateAttribute("a");
            a.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A)).ToString();
            odd.Attributes.Append(a);
        }

        private void BuildXmlNodeOdd_sent(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var y = xd.CreateAttribute("y");
            y.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Y)).ToString();
            odd.Attributes.Append(y);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_sent_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var y = xd.CreateAttribute("y");
            y.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Y)).ToString();
            odd.Attributes.Append(y);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_hsent(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var y = xd.CreateAttribute("y");
            y.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Y)).ToString();
            odd.Attributes.Append(y);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_hsent_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var y = xd.CreateAttribute("y");
            y.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Y)).ToString();
            odd.Attributes.Append(y);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_asent(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var y = xd.CreateAttribute("y");
            y.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Y)).ToString();
            odd.Attributes.Append(y);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_asent_1(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var y = xd.CreateAttribute("y");
            y.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.Y)).ToString();
            odd.Attributes.Append(y);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_x2(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var hd = xd.CreateAttribute("hd");
            hd.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.HD)).ToString();
            odd.Attributes.Append(hd);
            var ha = xd.CreateAttribute("ha");
            ha.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.HA)).ToString();
            odd.Attributes.Append(ha);
            var ad = xd.CreateAttribute("ad");
            ad.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.AD)).ToString();
            odd.Attributes.Append(ad);
        }

        private void BuildXmlNodeOdd_dnb(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h = xd.CreateAttribute("h");
            h.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H)).ToString();
            odd.Attributes.Append(h);
            var a = xd.CreateAttribute("a");
            a.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A)).ToString();
            odd.Attributes.Append(a);
        }

        private void BuildXmlNodeOdd_bon(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var b = xd.CreateAttribute("b");
            b.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.B)).ToString();
            odd.Attributes.Append(b);
            var o = xd.CreateAttribute("o");
            o.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O)).ToString();
            odd.Attributes.Append(o);
            var n = xd.CreateAttribute("n");
            n.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.N)).ToString();
            odd.Attributes.Append(n);
        }

        private void BuildXmlNodeOdd_twtn(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h = xd.CreateAttribute("h");
            h.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H)).ToString();
            odd.Attributes.Append(h);
            var a = xd.CreateAttribute("a");
            a.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A)).ToString();
            odd.Attributes.Append(a);
        }

        private void BuildXmlNodeOdd_3h(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var h = xd.CreateAttribute("h");
            h.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.H)).ToString();
            odd.Attributes.Append(h);
            var d = xd.CreateAttribute("d");
            d.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.D)).ToString();
            odd.Attributes.Append(d);
            var a = xd.CreateAttribute("a");
            a.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.A)).ToString();
            odd.Attributes.Append(a);
        }

        private void BuildXmlNodeOdd_ah_2(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o = xd.CreateAttribute("o");
            o.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O)).ToString();
            odd.Attributes.Append(o);
            var u = xd.CreateAttribute("u");
            u.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.U)).ToString();
            odd.Attributes.Append(u);

            var h = xd.CreateAttribute("h");
            h.Value = Convert.ToDecimal(item.H).ToString();
            odd.Attributes.Append(h);
        }

        private void BuildXmlNodeOdd_ou_2(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o = xd.CreateAttribute("o");
            o.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O)).ToString();
            odd.Attributes.Append(o);
            var u = xd.CreateAttribute("u");
            u.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.U)).ToString();
            odd.Attributes.Append(u);

            var h = xd.CreateAttribute("h");
            h.Value = Convert.ToDecimal(item.H).ToString();
            odd.Attributes.Append(h);
        }

        private void BuildXmlNodeOdd_cs_12(XmlDocument xd, OddsProfile op, MatchOdd item, XmlNode odd)
        {
            var o1 = xd.CreateAttribute("o01");
            o1.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O01)).ToString();
            odd.Attributes.Append(o1);

            var o2 = xd.CreateAttribute("o02");
            o2.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O02)).ToString();
            odd.Attributes.Append(o2);

            var o3 = xd.CreateAttribute("o03");
            o3.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O03)).ToString();
            odd.Attributes.Append(o3);

            var oO4 = xd.CreateAttribute("o04");
            oO4.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O04)).ToString();
            odd.Attributes.Append(oO4);

            var oO5 = xd.CreateAttribute("o05");
            oO5.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O05)).ToString();
            odd.Attributes.Append(oO5);

            var oO6 = xd.CreateAttribute("o06");
            oO6.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O06)).ToString();
            odd.Attributes.Append(oO6);

            var oO7 = xd.CreateAttribute("o07");
            oO7.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O07)).ToString();
            odd.Attributes.Append(oO7);

            var oO8 = xd.CreateAttribute("o08");
            oO8.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O08)).ToString();
            odd.Attributes.Append(oO8);

            var oO9 = xd.CreateAttribute("o09");
            oO9.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O09)).ToString();
            odd.Attributes.Append(oO9);

            var o10 = xd.CreateAttribute("o10");
            o10.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O10)).ToString();
            odd.Attributes.Append(o10);

            var o11 = xd.CreateAttribute("o11");
            o11.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O11)).ToString();
            odd.Attributes.Append(o11);

            var o12 = xd.CreateAttribute("o12");
            o12.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O12)).ToString();
            odd.Attributes.Append(o12);

            var o13 = xd.CreateAttribute("o13");
            o13.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O13)).ToString();
            odd.Attributes.Append(o13);

            var o14 = xd.CreateAttribute("o14");
            o14.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O14)).ToString();
            odd.Attributes.Append(o14);

            var o15 = xd.CreateAttribute("o15");
            o15.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O15)).ToString();
            odd.Attributes.Append(o15);

            var o16 = xd.CreateAttribute("o16");
            o16.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O16)).ToString();
            odd.Attributes.Append(o16);

            var o17 = xd.CreateAttribute("o17");
            o17.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O17)).ToString();
            odd.Attributes.Append(o17);

            var o18 = xd.CreateAttribute("o18");
            o18.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O18)).ToString();
            odd.Attributes.Append(o18);

            var o19 = xd.CreateAttribute("o19");
            o19.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O19)).ToString();
            odd.Attributes.Append(o19);

            var o20 = xd.CreateAttribute("o20");
            o20.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O20)).ToString();
            odd.Attributes.Append(o20);

            var o21 = xd.CreateAttribute("o21");
            o21.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O21)).ToString();
            odd.Attributes.Append(o21);

            var o22 = xd.CreateAttribute("o22");
            o22.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O22)).ToString();
            odd.Attributes.Append(o22);

            var o23 = xd.CreateAttribute("o23");
            o23.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O23)).ToString();
            odd.Attributes.Append(o23);

            var o24 = xd.CreateAttribute("o24");
            o24.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O24)).ToString();
            odd.Attributes.Append(o24);

            var o25 = xd.CreateAttribute("o25");
            o25.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O25)).ToString();
            odd.Attributes.Append(o25);

            var o26 = xd.CreateAttribute("o26");
            o26.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O26)).ToString();
            odd.Attributes.Append(o26);

            var o27 = xd.CreateAttribute("o27");
            o27.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O27)).ToString();
            odd.Attributes.Append(o27);

            var o28 = xd.CreateAttribute("o28");
            o28.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O28)).ToString();
            odd.Attributes.Append(o28);

            var o29 = xd.CreateAttribute("o29");
            o29.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O29)).ToString();
            odd.Attributes.Append(o29);

            var o30 = xd.CreateAttribute("o30");
            o30.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O30)).ToString();
            odd.Attributes.Append(o30);

            var o31 = xd.CreateAttribute("o31");
            o31.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O31)).ToString();
            odd.Attributes.Append(o31);

            var o32 = xd.CreateAttribute("o32");
            o32.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O32)).ToString();
            odd.Attributes.Append(o32);

            var o33 = xd.CreateAttribute("o33");
            o33.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O33)).ToString();
            odd.Attributes.Append(o33);

            var o34 = xd.CreateAttribute("o34");
            o34.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O34)).ToString();
            odd.Attributes.Append(o34);

            var o35 = xd.CreateAttribute("o35");
            o35.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O35)).ToString();
            odd.Attributes.Append(o35);

            var o36 = xd.CreateAttribute("o36");
            o36.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O36)).ToString();
            odd.Attributes.Append(o36);

            var o37 = xd.CreateAttribute("o37");
            o37.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O37)).ToString();
            odd.Attributes.Append(o37);

            var o38 = xd.CreateAttribute("o38");
            o38.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O38)).ToString();
            odd.Attributes.Append(o38);

            var o39 = xd.CreateAttribute("o39");
            o39.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O39)).ToString();
            odd.Attributes.Append(o39);

            var o40 = xd.CreateAttribute("o40");
            o40.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O40)).ToString();
            odd.Attributes.Append(o40);

            var o41 = xd.CreateAttribute("o41");
            o41.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O41)).ToString();
            odd.Attributes.Append(o41);

            var o42 = xd.CreateAttribute("o42");
            o42.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O42)).ToString();
            odd.Attributes.Append(o42);

            var o43 = xd.CreateAttribute("o43");
            o43.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O43)).ToString();
            odd.Attributes.Append(o43);

            var o44 = xd.CreateAttribute("o44");
            o44.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O44)).ToString();
            odd.Attributes.Append(o44);

            var o45 = xd.CreateAttribute("o45");
            o45.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O45)).ToString();
            odd.Attributes.Append(o45);

            var o46 = xd.CreateAttribute("o46");
            o46.Value = Utilities.ConvertOdds(op.OddRegion, Convert.ToDecimal(item.O46)).ToString();
            odd.Attributes.Append(o46);
        }

        #endregion
    }
}