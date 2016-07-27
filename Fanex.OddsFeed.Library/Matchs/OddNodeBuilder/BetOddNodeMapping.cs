namespace Fanex.OddsFeed.Library.Matchs.OddNodeBuilder
{
    using System.Collections.Generic;

    public class BetOddNodeMapping
    {
        public static readonly string HighlightBetType = "415,416,427,428,430,431,432,433,434,435,436,437,438,439,440,441,442,443,444";

        // Here are children bet type Tags which are not the same as their parent Tags
        // So we use their parent Tags instead of themselves
        public static readonly Dictionary<string, string> specialChildBetTags = new Dictionary<string, string>
        {
            // Child Tag, Parent Tag
            {"hdp_2", "ah_2"}
        };

        public static readonly Dictionary<int, string> BetTypeIdMap = new Dictionary<int, string>
        {
            { 1, "ah" },
            { 2, "oe" },
            { 3, "ou" },
            { 4, "cs" },
            { 5, "st" },
            { 6, "tg" },
            { 7, "ah_1" },
            { 8, "ou_1" },
            { 10, "outright" },
            { 12, "oe_1" },
            { 15, "st_1" },
            { 16, "htft" },
            { 20, "ml" },
            { 13, "so" },
            { 14, "fglg" },
            { 17, "ah_2" },
            { 18, "ou_2" },
            { 21, "ml_1" },
            { 24, "x2" },
            { 25, "dnb" },
            { 26, "bon" },
            { 27, "twtn" },
            { 28, "3h" },
            { 30, "cs_1" },
            { 121, "hnb" },
            { 122, "anb" },
            { 123, "dnd" },
            { 126, "tg_1" },
            { 127, "fglg_1" },
            { 128, "oe_12" },
            { 133, "h2h" },
            { 134, "a2h" },
            { 135, "pen" },
            { 140, "high" },
            { 141, "hhigh" },
            { 142, "ahigh" },
            { 145, "bs" },
            { 146, "bs_2" },
            { 147, "hs" },
            { 148, "as" },
            { 149, "hw" },
            { 150, "aw" },
            { 151, "x2_1" },
            { 152, "cs_12" },
            { 153, "tah" },
            { 154, "sw" },
            { 155, "gh" },
            { 156, "gou" },
            { 157, "oe" },
            { 159, "etg" },
            { 160, "ng" },
            { 161, "heg" },
            { 162, "aeg" },
            { 163, "sttg" },
            { 164, "nxtg_ex" },
            { 165, "cs_ex1" },
            { 166, "cs_ex" },
            { 167, "st_ex1" },
            { 168, "nxtrnd" },
            { 169, "nxtg" },
            { 170, "ts" },
            { 171, "win" },
            { 172, "stts" },
            { 173, "ex" },
            { 174, "exg" },
            { 175, "mtd" },
            { 176, "st_10" },
            { 177, "st_2" },
            { 178, "ou_2" },
            { 179, "egs_1" },
            { 180, "ng_1" },
            { 181, "heg_1" },
            { 182, "aeg_1" },
            { 183, "hdp_2" },
            { 184, "oe_2" },
            { 185, "dnb_2" },
            { 186, "x2_2" },
            { 187, "eg_2" },
            { 188, "b_1" },
            { 189, "oyn" },
            { 190, "uyn" },
            { 191, "dnb_1" },
            { 192, "fg10" },
            { 193, "fg15" },
            { 194, "coe" },
            { 195, "hc" },
            { 196, "ac" },
            { 197, "hcou" },
            { 198, "acou" },
            { 199, "tc" },
            { 200, "ec" },
            { 201, "ec_1" },
            { 202, "tc_1" },
            { 203, "coe_1" },
            { 204, "hcou_1" },
            { 205, "acou_1" },
            { 206, "fc" },
            { 207, "fc_1" },
            { 208, "lc" },
            { 209, "lc_1" },
            { 210, "sent" },
            { 211, "sent_1" },
            { 212, "hsent" },
            { 213, "hsent_1" },
            { 214, "asent" },
            { 215, "asent_1" },
            { 11, "tc" },
            { 19, "ml_1" },
            { 22, "ng" },
            { 124, "stah" },
            { 125, "stah_1" },
            { 129, "eg_1" },
            { 130, "eg" },
            { 131, "htg" },
            { 132, "atg" },
            { 136, "hoe_1" },
            { 137, "hoe" },
            { 138, "aoe_1" },
            { 139, "aoe" },
            { 143, "sttg_1" },
            { 144, "sttg" },
            { 216, "gsa" },
            { 217, "gs1" },
            { 221, "nxt1" },
            { 222, "nxt5" },
            { 223, "1nxt1" },
            { 224, "1nxt5" },
            { 401, "ou_h" },
            { 402, "ou_a" },
            { 403, "ou_h1" },
            { 404, "ou_a1" },
            { 405, "cs_2" },
            { 406, "etg" },
            { 407, "heg" },
            { 408, "win" },
            { 409, "aeg" },
            { 410, "x2_1" },
            { 411, "dnb_1" },
            { 412, "egs_1st" },
            { 413, "cs" },
            { 414, "cs_1" },
            {415, "stts"},
            {416, "cs_12"},
            {427, "b_1"},
            {428, "oe_2"},
            {430, "st_2"},
            {431, "x2_2"},
            {432, "dnb_2"},
            {433, "bs_2"},
            {434, "oyn"},
            {435, "uyn"},
            {436, "hs"},
            {437, "as"},
            {438, "h2h"},
            {439, "a2h"},
            {440, "hw"},
            {441, "aw"},
            {442, "high"},
            {443, "hhigh"},
            {444, "ahigh"}
       };

        // Notes:
        // When adding a new bettype here, remember to check if the Tag is already in old bettypes yet
        // The idea to build xml based on Tag is not ok anymore, need to change to use bettype ID instead
        public static readonly List<OddNodeBase> BetTypes = new List<OddNodeBase> {
            new OddNodeBase
            {
                ID = 221,
                Tag = "nxt1",
                Desc = "Next 1 minutes",
                Odds = new OddNodeAttrBase[6] {
                    new OddNodeAttrBase { XmlAttribute = "g", DBAttribute = "cs01", MatchOddAttribute = "G", Description = "Price for Goal"},
                    new OddNodeAttrBase { XmlAttribute = "c", DBAttribute = "cs02", MatchOddAttribute = "C", Description = "Price for Corner"},
                    new OddNodeAttrBase { XmlAttribute = "fk", DBAttribute = "cs03", MatchOddAttribute = "FK", Description = "Price for Free Kick"},
                    new OddNodeAttrBase { XmlAttribute = "gk", DBAttribute = "cs04", MatchOddAttribute = "GK", Description = "Price for Goal Kick"},
                    new OddNodeAttrBase { XmlAttribute = "ti", DBAttribute = "cs05", MatchOddAttribute = "TI", Description = "Price for Throw In"},
                    new OddNodeAttrBase {
                        XmlAttribute = "intervals",
                        DBAttribute = "hdp1",
                        MatchOddAttribute = "TIME_INTERVAL",
                        NotToConvertOdd = true,
                        Description = @"Indicates offered intervals. For example: 
                            <br>&nbsp;&nbsp;&nbsp; intervals = 1 for 00:00 - 00:59 
                            <br>&nbsp;&nbsp;&nbsp; intervals = 2 for 01:00 - 01:59
                            <br>&nbsp;&nbsp;&nbsp; intervals = 90 for 89:00 - 89:59"}
                }
            },
            new OddNodeBase
            {
                ID = 222,
                Tag = "nxt5",
                Desc = "Next 5 minutes",
                Odds = new OddNodeAttrBase[7] {
                    new OddNodeAttrBase { XmlAttribute = "gy", DBAttribute = "cs01", MatchOddAttribute = "GY", Description = "Price for Goal Yes"},
                    new OddNodeAttrBase { XmlAttribute = "gn", DBAttribute = "cs10", MatchOddAttribute = "GN", Description = "Price for Goal No"},
                    new OddNodeAttrBase { XmlAttribute = "cy", DBAttribute = "cs02", MatchOddAttribute = "CY", Description = "Price for Corner Yes"},
                    new OddNodeAttrBase { XmlAttribute = "cn", DBAttribute = "cs20", MatchOddAttribute = "CN", Description = "Price for Corner No"},
                    new OddNodeAttrBase { XmlAttribute = "py", DBAttribute = "cs13", MatchOddAttribute = "PY", Description = "Price for Penalty Yes"},
                    new OddNodeAttrBase { XmlAttribute = "pn", DBAttribute = "cs31", MatchOddAttribute = "PN", Description = "Price for Penalty No"},
                    new OddNodeAttrBase {
                        XmlAttribute = "intervals",
                        DBAttribute = "hdp1",
                        MatchOddAttribute = "TIME_INTERVAL",
                        NotToConvertOdd = true,
                        Description = @"Indicates offered intervals. For example: 
                            <br>&nbsp;&nbsp;&nbsp; intervals = 1 for 00:00 - 00:59 
                            <br>&nbsp;&nbsp;&nbsp; intervals = 2 for 01:00 - 01:59
                            <br>&nbsp;&nbsp;&nbsp; intervals = 90 for 89:00 - 89:59"}
                }
            },
            new OddNodeBase
            {
                ID = 223,
                Tag = "1nxt1",
                Desc = "What will happen first in next 1 minutes",
                Odds = new OddNodeAttrBase[7] {
                    new OddNodeAttrBase { XmlAttribute = "n", DBAttribute = "cs00", MatchOddAttribute = "N", Description = "Price for None"},
                    new OddNodeAttrBase { XmlAttribute = "g", DBAttribute = "cs01", MatchOddAttribute = "G", Description = "Price for Goal"},
                    new OddNodeAttrBase { XmlAttribute = "c", DBAttribute = "cs02", MatchOddAttribute = "C", Description = "Price for Corner"},
                    new OddNodeAttrBase { XmlAttribute = "fk", DBAttribute = "cs03", MatchOddAttribute = "FK", Description = "Price for Free Kick"},
                    new OddNodeAttrBase { XmlAttribute = "gk", DBAttribute = "cs04", MatchOddAttribute = "GK", Description = "Price for Goal Kick"},
                    new OddNodeAttrBase { XmlAttribute = "ti", DBAttribute = "cs05", MatchOddAttribute = "TI", Description = "Price for Throw In"},
                    new OddNodeAttrBase {
                        XmlAttribute = "intervals",
                        DBAttribute = "hdp1",
                        MatchOddAttribute = "TIME_INTERVAL",
                        NotToConvertOdd = true,
                        Description = @"Indicates offered intervals. For example: 
                            <br>&nbsp;&nbsp;&nbsp; intervals = 1 for 00:00 - 00:59 
                            <br>&nbsp;&nbsp;&nbsp; intervals = 2 for 01:00 - 01:59
                            <br>&nbsp;&nbsp;&nbsp; intervals = 90 for 89:00 - 89:59"}
                }
            },
            new OddNodeBase
            {
                ID = 224,
                Tag = "1nxt5",
                Desc = "What will happen first in next 5 minutes",
                Odds = new OddNodeAttrBase[6] {
                    new OddNodeAttrBase { XmlAttribute = "n", DBAttribute = "cs00", MatchOddAttribute = "N", Description = "Price for None"},
                    new OddNodeAttrBase { XmlAttribute = "g", DBAttribute = "cs01", MatchOddAttribute = "G", Description = "Price for Goal"},
                    new OddNodeAttrBase { XmlAttribute = "c", DBAttribute = "cs02", MatchOddAttribute = "C", Description = "Price for Corner"},
                    new OddNodeAttrBase { XmlAttribute = "b", DBAttribute = "cs12", MatchOddAttribute = "B", Description = "Price for Booking"},
                    new OddNodeAttrBase { XmlAttribute = "p", DBAttribute = "cs13", MatchOddAttribute = "P", Description = "Price for Penalty"},
                    new OddNodeAttrBase {
                        XmlAttribute = "intervals",
                        DBAttribute = "hdp1",
                        MatchOddAttribute = "TIME_INTERVAL",
                        NotToConvertOdd = true,
                        Description = @"Indicates offered intervals. For example: 
                            <br>&nbsp;&nbsp;&nbsp; intervals = 1 for 00:00 - 00:59 
                            <br>&nbsp;&nbsp;&nbsp; intervals = 2 for 01:00 - 01:59
                            <br>&nbsp;&nbsp;&nbsp; intervals = 90 for 89:00 - 89:59"}
                }
            },
            new OddNodeBase
            {
                ID = 401,
                Tag = "ou_h",
                Desc = "Home Team Over/Under",
                Odds = new OddNodeAttrBase[3] {
                    new OddNodeAttrBase { XmlAttribute = "o", DBAttribute = "odds1a", MatchOddAttribute = "O", Description = "Price for Over"},
                    new OddNodeAttrBase { XmlAttribute = "u", DBAttribute = "odds2a", MatchOddAttribute = "U", Description = "Price for Under"},
                    new OddNodeAttrHandicap {
                        NotToConvertOdd = true,
                        XmlAttribute = "h",
                        DBAttribute = "hdp1",
                        MatchOddAttribute = "H",
                        Description = "Price for Over/Under value"}
                }
            },
            new OddNodeBase
            {
                ID = 402,
                Tag = "ou_a",
                Desc = "Away Team Over/Under",
                Odds = new OddNodeAttrBase[3] {
                    new OddNodeAttrBase { XmlAttribute = "o", DBAttribute = "odds1a", MatchOddAttribute = "O", Description = "Price for Over"},
                    new OddNodeAttrBase { XmlAttribute = "u", DBAttribute = "odds2a", MatchOddAttribute = "U", Description = "Price for Under"},
                    new OddNodeAttrHandicap {
                        NotToConvertOdd = true,
                        XmlAttribute = "h",
                        DBAttribute = "hdp1",
                        MatchOddAttribute = "H",
                        Description = "Price for Over/Under value"}
                }
            },
            new OddNodeBase
            {
                ID = 403,
                Tag = "ou_h1",
                Desc = "1H Home Team Over/Under",
                Odds = new OddNodeAttrBase[3] {
                    new OddNodeAttrBase { XmlAttribute = "o", DBAttribute = "odds1a", MatchOddAttribute = "O", Description = "Price for Over"},
                    new OddNodeAttrBase { XmlAttribute = "u", DBAttribute = "odds2a", MatchOddAttribute = "U", Description = "Price for Under"},
                    new OddNodeAttrHandicap {
                        NotToConvertOdd = true,
                        XmlAttribute = "h",
                        DBAttribute = "hdp1",
                        MatchOddAttribute = "H",
                        Description = "Price for Over/Under value"}
                }
            },
            new OddNodeBase
            {
                ID = 404,
                Tag = "ou_a1",
                Desc = "1H Away Team Over/Under",
                Odds = new OddNodeAttrBase[3] {
                    new OddNodeAttrBase { XmlAttribute = "o", DBAttribute = "odds1a", MatchOddAttribute = "O", Description = "Price for Over"},
                    new OddNodeAttrBase { XmlAttribute = "u", DBAttribute = "odds2a", MatchOddAttribute = "U", Description = "Price for Under"},
                    new OddNodeAttrHandicap {
                        NotToConvertOdd = true,
                        XmlAttribute = "h",
                        DBAttribute = "hdp1",
                        MatchOddAttribute = "H",
                        Description = "Price for Over/Under value"}
                }
            },
            new OddNodeBase
            {
                ID = 405,
                Tag = "cs_2",
                Desc = "2H Correct Score",
                Odds = new OddNodeAttrBase[17] {
                    new OddNodeAttrBase { XmlAttribute = "cs10", MatchOddAttribute = "CS10", Description = "Price for 1-0", DBAttribute = "cs10"},
                    new OddNodeAttrBase { XmlAttribute = "cs20", MatchOddAttribute = "CS20", Description = "Price for 2-0", DBAttribute = "cs20"},
                    new OddNodeAttrBase { XmlAttribute = "cs21", MatchOddAttribute = "CS21", Description = "Price for 2-1", DBAttribute = "cs21"},
                    new OddNodeAttrBase { XmlAttribute = "cs30", MatchOddAttribute = "CS30", Description = "Price for 3-0", DBAttribute = "cs30"},
                    new OddNodeAttrBase { XmlAttribute = "cs31", MatchOddAttribute = "CS31", Description = "Price for 3-1", DBAttribute = "cs31"},
                    new OddNodeAttrBase { XmlAttribute = "cs32", MatchOddAttribute = "CS32", Description = "Price for 3-2", DBAttribute = "cs32"},
                    new OddNodeAttrBase { XmlAttribute = "cs01", MatchOddAttribute = "CS01", Description = "Price for 0-1", DBAttribute = "cs01"},
                    new OddNodeAttrBase { XmlAttribute = "cs02", MatchOddAttribute = "CS02", Description = "Price for 0-2", DBAttribute = "cs02"},
                    new OddNodeAttrBase { XmlAttribute = "cs12", MatchOddAttribute = "CS12", Description = "Price for 1-2", DBAttribute = "cs12"},
                    new OddNodeAttrBase { XmlAttribute = "cs03", MatchOddAttribute = "CS03", Description = "Price for 0-3", DBAttribute = "cs03"},
                    new OddNodeAttrBase { XmlAttribute = "cs13", MatchOddAttribute = "CS13", Description = "Price for 1-3", DBAttribute = "cs13"},
                    new OddNodeAttrBase { XmlAttribute = "cs23", MatchOddAttribute = "CS23", Description = "Price for 2-3", DBAttribute = "cs23"},
                    new OddNodeAttrBase { XmlAttribute = "cs00", MatchOddAttribute = "CS00", Description = "Price for 0-0", DBAttribute = "cs00"},
                    new OddNodeAttrBase { XmlAttribute = "cs11", MatchOddAttribute = "CS11", Description = "Price for 1-1", DBAttribute = "cs11"},
                    new OddNodeAttrBase { XmlAttribute = "cs22", MatchOddAttribute = "CS22", Description = "Price for 2-2", DBAttribute = "cs22"},
                    new OddNodeAttrBase { XmlAttribute = "cs33", MatchOddAttribute = "CS33", Description = "Price for 3-3", DBAttribute = "cs33"},
                    new OddNodeAttrBase { XmlAttribute = "csaos", MatchOddAttribute = "CSOAS", Description = "Price for AOS", DBAttribute = "cs99"}
                }
            },
            new OddNodeBase
            {
                ID = 406,
                Tag = "etg",
                Desc = "Exact Total Goals",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[7] {
                    new OddNodeAttrBase { XmlAttribute = "o01", MatchOddAttribute = "O01", Description = "Price for 0", DBAttribute = "cs01"},
                    new OddNodeAttrBase { XmlAttribute = "o02", MatchOddAttribute = "O02", Description = "Price for 1", DBAttribute = "cs02"},
                    new OddNodeAttrBase { XmlAttribute = "o03", MatchOddAttribute = "O03", Description = "Price for 2", DBAttribute = "cs03"},
                    new OddNodeAttrBase { XmlAttribute = "o04", MatchOddAttribute = "O04", Description = "Price for 3", DBAttribute = "cs04"},
                    new OddNodeAttrBase { XmlAttribute = "o05", MatchOddAttribute = "O05", Description = "Price for 4", DBAttribute = "cs10"},
                    new OddNodeAttrBase { XmlAttribute = "o06", MatchOddAttribute = "O06", Description = "Price for 5", DBAttribute = "cs20"},
                    new OddNodeAttrBase { XmlAttribute = "o07", MatchOddAttribute = "O07", Description = "Price for 6&Over", DBAttribute = "cs30"}
                }
            },
            new OddNodeBase
            {
                ID = 407,
                Tag = "heg",
                Desc = "Exact Home Team Goals",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[4] {
                    new OddNodeAttrBase { XmlAttribute = "o1", MatchOddAttribute = "O1", Description = "Price for 0", DBAttribute = "cs00"},
                    new OddNodeAttrBase { XmlAttribute = "o2", MatchOddAttribute = "O2", Description = "Price for 1", DBAttribute = "cs01"},
                    new OddNodeAttrBase { XmlAttribute = "o3", MatchOddAttribute = "O3", Description = "Price for 2", DBAttribute = "cs02"},
                    new OddNodeAttrBase { XmlAttribute = "o4", MatchOddAttribute = "O4", Description = "Price for 3&Over", DBAttribute = "cs03"}
                }
            },
            new OddNodeBase
            {
                ID = 408,
                Tag = "win",
                Desc = "Winning Margin",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[8] {
                    new OddNodeAttrBase { XmlAttribute = "h1", MatchOddAttribute = "H1", Description = "Price for Home & 1 Goal", DBAttribute = "cs01"},
                    new OddNodeAttrBase { XmlAttribute = "h2", MatchOddAttribute = "H2", Description = "Price for Home & 2 Goals", DBAttribute = "cs02"},
                    new OddNodeAttrBase { XmlAttribute = "h3", MatchOddAttribute = "H3", Description = "Price for Home & 3 Goals up", DBAttribute = "cs03"},
                    new OddNodeAttrBase { XmlAttribute = "d", MatchOddAttribute = "D", Description = "Price for S.Draw", DBAttribute = "cs04"},
                    new OddNodeAttrBase { XmlAttribute = "a1", MatchOddAttribute = "A1", Description = "Price for Away & 1 Goal", DBAttribute = "cs10"},
                    new OddNodeAttrBase { XmlAttribute = "a2", MatchOddAttribute = "A2", Description = "Price for Away & 2 Goals", DBAttribute = "cs20"},
                    new OddNodeAttrBase { XmlAttribute = "a3", MatchOddAttribute = "A3", Description = "Price for Away & 3 Goals up", DBAttribute = "cs30"},
                    new OddNodeAttrBase { XmlAttribute = "ng", MatchOddAttribute = "NG", Description = "Price for No Goal", DBAttribute = "cs00" }
                }
            },
            new OddNodeBase
            {
                ID = 409,
                Tag = "aeg",
                Desc = "Exact Away Team Goals",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[4] {
                    new OddNodeAttrBase { XmlAttribute = "o1", MatchOddAttribute = "O1", Description = "Price for 0", DBAttribute = "cs00"},
                    new OddNodeAttrBase { XmlAttribute = "o2", MatchOddAttribute = "O2", Description = "Price for 1", DBAttribute = "cs01"},
                    new OddNodeAttrBase { XmlAttribute = "o3", MatchOddAttribute = "O3", Description = "Price for 2", DBAttribute = "cs02"},
                    new OddNodeAttrBase { XmlAttribute = "o4", MatchOddAttribute = "O4", Description = "Price for 3 Goals & Over", DBAttribute = "cs03"}
                }
            },
            new OddNodeBase
            {
                ID = 410,
                Tag = "x2_1",
                Desc = "1H Double Chance",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[3] {
                    new OddNodeAttrBase { XmlAttribute = "hd", MatchOddAttribute = "HD", Description = "Price for Home or Draw", DBAttribute = "cs10"},
                    new OddNodeAttrBase { XmlAttribute = "ha", MatchOddAttribute = "HA", Description = "Price for Home or Away", DBAttribute = "cs00"},
                    new OddNodeAttrBase { XmlAttribute = "ad", MatchOddAttribute = "AD", Description = "Price for Away or Draw", DBAttribute = "cs20"}
                }
            },
            new OddNodeBase
            {
                ID = 411,
                Tag = "dnb_1",
                Desc = "1H Draw No Bet",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[2] {
                    new OddNodeAttrBase { XmlAttribute = "h", MatchOddAttribute = "H", Description = "Price for Home", DBAttribute = "odds1a"},
                    new OddNodeAttrBase { XmlAttribute = "a", MatchOddAttribute = "A", Description = "Price for Away", DBAttribute = "odds2a"}
                }
            },
            new OddNodeBase
            {
                ID = 412,
                Tag = "egs_1st",
                Desc = "Exact 1st Half Goal",
                Odds = new OddNodeAttrBase[4] {
                    new OddNodeAttrBase { XmlAttribute = "o0", MatchOddAttribute = "O0", Description = "Price for 0", DBAttribute = "cs00"},
                    new OddNodeAttrBase { XmlAttribute = "o1", MatchOddAttribute = "O1", Description = "Price for 1", DBAttribute = "cs01"},
                    new OddNodeAttrBase { XmlAttribute = "o2", MatchOddAttribute = "O2", Description = "Price for 2", DBAttribute = "cs10"},
                    new OddNodeAttrBase { XmlAttribute = "o3", MatchOddAttribute = "O3", Description = "Price for 3 Goals & Over", DBAttribute = "cs11"}
                }
            },
            new OddNodeBase
            {
                ID = 413,
                Tag = "cs",
                Desc = "Correct Score (aos)",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[28] {
                    new OddNodeAttrBase { XmlAttribute = "cs10", MatchOddAttribute = "C10", Description = "Price for 1-0", DBAttribute = "cs10"},
                    new OddNodeAttrBase { XmlAttribute = "cs20", MatchOddAttribute = "C20", Description = "Price for 2-0", DBAttribute = "cs20"},
                    new OddNodeAttrBase { XmlAttribute = "cs21", MatchOddAttribute = "C21", Description = "Price for 2-1", DBAttribute = "cs21"},
                    new OddNodeAttrBase { XmlAttribute = "cs30", MatchOddAttribute = "C30", Description = "Price for 3-0", DBAttribute = "cs30"},
                    new OddNodeAttrBase { XmlAttribute = "cs31", MatchOddAttribute = "C31", Description = "Price for 3-1", DBAttribute = "cs31"},
                    new OddNodeAttrBase { XmlAttribute = "cs32", MatchOddAttribute = "C32", Description = "Price for 3-2", DBAttribute = "cs32"},
                    new OddNodeAttrBase { XmlAttribute = "cs40", MatchOddAttribute = "C40", Description = "Price for 4-0", DBAttribute = "cs40"},
                    new OddNodeAttrBase { XmlAttribute = "cs41", MatchOddAttribute = "C41", Description = "Price for 4-1", DBAttribute = "cs41"},
                    new OddNodeAttrBase { XmlAttribute = "cs42", MatchOddAttribute = "C42", Description = "Price for 4-2", DBAttribute = "cs42"},
                    new OddNodeAttrBase { XmlAttribute = "cs43", MatchOddAttribute = "C43", Description = "Price for 4-3", DBAttribute = "cs43"},
                    new OddNodeAttrBase { XmlAttribute = "cs01", MatchOddAttribute = "C01", Description = "Price for 0-1", DBAttribute = "cs01"},
                    new OddNodeAttrBase { XmlAttribute = "cs02", MatchOddAttribute = "C02", Description = "Price for 0-2", DBAttribute = "cs02"},
                    new OddNodeAttrBase { XmlAttribute = "cs12", MatchOddAttribute = "C12", Description = "Price for 1-2", DBAttribute = "cs12"},
                    new OddNodeAttrBase { XmlAttribute = "cs03", MatchOddAttribute = "C03", Description = "Price for 0-3", DBAttribute = "cs03"},
                    new OddNodeAttrBase { XmlAttribute = "cs13", MatchOddAttribute = "C13", Description = "Price for 1-3", DBAttribute = "cs13"},
                    new OddNodeAttrBase { XmlAttribute = "cs23", MatchOddAttribute = "C23", Description = "Price for 2-3", DBAttribute = "cs23"},
                    new OddNodeAttrBase { XmlAttribute = "cs04", MatchOddAttribute = "C04", Description = "Price for 0-4", DBAttribute = "cs04"},
                    new OddNodeAttrBase { XmlAttribute = "cs14", MatchOddAttribute = "C14", Description = "Price for 1-4", DBAttribute = "cs14"},
                    new OddNodeAttrBase { XmlAttribute = "cs24", MatchOddAttribute = "C24", Description = "Price for 2-4", DBAttribute = "cs24"},
                    new OddNodeAttrBase { XmlAttribute = "cs34", MatchOddAttribute = "C34", Description = "Price for 3-4", DBAttribute = "cs34"},
                    new OddNodeAttrBase { XmlAttribute = "cs00", MatchOddAttribute = "C00", Description = "Price for 0-0", DBAttribute = "cs00"},
                    new OddNodeAttrBase { XmlAttribute = "cs11", MatchOddAttribute = "C11", Description = "Price for 1-1", DBAttribute = "cs11"},
                    new OddNodeAttrBase { XmlAttribute = "cs22", MatchOddAttribute = "C22", Description = "Price for 2-2", DBAttribute = "cs22"},
                    new OddNodeAttrBase { XmlAttribute = "cs33", MatchOddAttribute = "C33", Description = "Price for 3-3", DBAttribute = "cs33"},
                    new OddNodeAttrBase { XmlAttribute = "cs44", MatchOddAttribute = "C44", Description = "Price for 4-4", DBAttribute = "cs44"},
                    new OddNodeAttrBase { XmlAttribute = "csaos", MatchOddAttribute = "CSOAS", Description = "Price for AOS", DBAttribute = "cs99"},
                    // for bettype 4
                    new OddNodeAttrBase { XmlAttribute = "cs05", MatchOddAttribute = "C05", Description = "Price for 0-5 UP", DBAttribute = "cs05"},
                    new OddNodeAttrBase { XmlAttribute = "cs50", MatchOddAttribute = "C50", Description = "Price for 5-0 UP", DBAttribute = "cs50"}
                }
            },
            new OddNodeBase
            {
                ID = 414,
                Tag = "cs_1",
                Desc = "1H Correct Score (aos)",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[19] {
                    new OddNodeAttrBase { XmlAttribute = "cs10", MatchOddAttribute = "C10", Description = "Price for 1-0", DBAttribute = "cs10"},
                    new OddNodeAttrBase { XmlAttribute = "cs20", MatchOddAttribute = "C20", Description = "Price for 2-0", DBAttribute = "cs20"},
                    new OddNodeAttrBase { XmlAttribute = "cs21", MatchOddAttribute = "C21", Description = "Price for 2-1", DBAttribute = "cs21"},
                    new OddNodeAttrBase { XmlAttribute = "cs30", MatchOddAttribute = "C30", Description = "Price for 3-0", DBAttribute = "cs30"},
                    new OddNodeAttrBase { XmlAttribute = "cs31", MatchOddAttribute = "C31", Description = "Price for 3-1", DBAttribute = "cs31"},
                    new OddNodeAttrBase { XmlAttribute = "cs32", MatchOddAttribute = "C32", Description = "Price for 3-2", DBAttribute = "cs32"},
                    new OddNodeAttrBase { XmlAttribute = "cs01", MatchOddAttribute = "C01", Description = "Price for 0-1", DBAttribute = "cs01"},
                    new OddNodeAttrBase { XmlAttribute = "cs02", MatchOddAttribute = "C02", Description = "Price for 0-2", DBAttribute = "cs02"},
                    new OddNodeAttrBase { XmlAttribute = "cs12", MatchOddAttribute = "C12", Description = "Price for 1-2", DBAttribute = "cs12"},
                    new OddNodeAttrBase { XmlAttribute = "cs03", MatchOddAttribute = "C03", Description = "Price for 0-3", DBAttribute = "cs03"},
                    new OddNodeAttrBase { XmlAttribute = "cs13", MatchOddAttribute = "C13", Description = "Price for 1-3", DBAttribute = "cs13"},
                    new OddNodeAttrBase { XmlAttribute = "cs23", MatchOddAttribute = "C23", Description = "Price for 2-3", DBAttribute = "cs23"},
                    new OddNodeAttrBase { XmlAttribute = "cs00", MatchOddAttribute = "C00", Description = "Price for 0-0", DBAttribute = "cs00"},
                    new OddNodeAttrBase { XmlAttribute = "cs11", MatchOddAttribute = "C11", Description = "Price for 1-1", DBAttribute = "cs11"},
                    new OddNodeAttrBase { XmlAttribute = "cs22", MatchOddAttribute = "C22", Description = "Price for 2-2", DBAttribute = "cs22"},
                    new OddNodeAttrBase { XmlAttribute = "cs33", MatchOddAttribute = "C33", Description = "Price for 3-3", DBAttribute = "cs33"},
                    new OddNodeAttrBase { XmlAttribute = "csaos", MatchOddAttribute = "CSOAS", Description = "Price for AOS", DBAttribute = "cs99"},
                    // for bettype 30
                    new OddNodeAttrBase { XmlAttribute = "cs04", MatchOddAttribute = "C04", Description = "Price for 0-4 UP", DBAttribute = "cs04"},
                    new OddNodeAttrBase { XmlAttribute = "cs40", MatchOddAttribute = "C40", Description = "Price for 4-0 UP", DBAttribute = "cs40"}
                }
            },
            new OddNodeBase
            {
                ID = 415,
                Tag = "stts",
                Desc = "Result And First Team To Score (own)",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[7] {
                                new OddNodeAttrBase { XmlAttribute = "hh", DBAttribute = "odds.cs01", MatchOddAttribute = "HH", Description = "Price for Home Home"},
                                new OddNodeAttrBase { XmlAttribute = "dh", DBAttribute = "odds.cs02", MatchOddAttribute = "DH", Description = "Price for Draw Home"},
                                new OddNodeAttrBase { XmlAttribute = "ah", DBAttribute = "odds.cs03", MatchOddAttribute = "AH", Description = "Price for Away Home"},
                                new OddNodeAttrBase { XmlAttribute = "ha", DBAttribute = "odds.cs04", MatchOddAttribute = "HA", Description = "Price for Home Away"},
                                new OddNodeAttrBase { XmlAttribute = "da", DBAttribute = "odds.cs10", MatchOddAttribute = "DA", Description = "Price for Draw Away"},
                                new OddNodeAttrBase { XmlAttribute = "aa", DBAttribute = "odds.cs20", MatchOddAttribute = "AA", Description = "Price for Away Away"},
                                new OddNodeAttrBase { XmlAttribute = "n", DBAttribute = "odds.cs30", MatchOddAttribute = "N", Description = "Price for None"}
                     }
            },
            new OddNodeFlex
            {
                ID = 416,
                Tag = "cs_12",
                Desc = "Half Time/Full Time Correct Score (own)",
                IsChildBettype = true,
                Odds = new OddNodeAttrFlex[46] {
                                new OddNodeAttrFlex { XmlAttribute = "o01", DBAttribute = "", OddFlexBetChoice = "0:0/0:0", MatchOddAttribute = "O01", Description = "Price for 0:0/0:0"},
                                new OddNodeAttrFlex { XmlAttribute = "o02", DBAttribute = "", OddFlexBetChoice = "0:0/1:0", MatchOddAttribute = "O02", Description = "Price for 0:0/1:0"},
                                new OddNodeAttrFlex { XmlAttribute = "o03", DBAttribute = "", OddFlexBetChoice = "0:0/0:1", MatchOddAttribute = "O03", Description = "Price for 0:0/0:1"},
                                new OddNodeAttrFlex { XmlAttribute = "o04", DBAttribute = "", OddFlexBetChoice = "0:0/2:0", MatchOddAttribute = "O04", Description = "Price for 0:0/2:0"},
                                new OddNodeAttrFlex { XmlAttribute = "o05", DBAttribute = "", OddFlexBetChoice = "0:0/1:1", MatchOddAttribute = "O05", Description = "Price for 0:0/1:1"},
                                new OddNodeAttrFlex { XmlAttribute = "o06", DBAttribute = "", OddFlexBetChoice = "0:0/0:2", MatchOddAttribute = "O06", Description = "Price for 0:0/0:2"},
                                new OddNodeAttrFlex { XmlAttribute = "o07", DBAttribute = "", OddFlexBetChoice = "0:0/3:0", MatchOddAttribute = "O07", Description = "Price for 0:0/3:0"},
                                new OddNodeAttrFlex { XmlAttribute = "o08", DBAttribute = "", OddFlexBetChoice = "0:0/2:1", MatchOddAttribute = "O08", Description = "Price for 0:0/2:1"},
                                new OddNodeAttrFlex { XmlAttribute = "o09", DBAttribute = "", OddFlexBetChoice = "0:0/1:2", MatchOddAttribute = "O09", Description = "Price for 0:0/1:2"},
                                new OddNodeAttrFlex { XmlAttribute = "o10", DBAttribute = "", OddFlexBetChoice = "0:0/0:3", MatchOddAttribute = "O10", Description = "Price for 0:0/0:3"},
                                new OddNodeAttrFlex { XmlAttribute = "o11", DBAttribute = "", OddFlexBetChoice = "0:0/4+", MatchOddAttribute = "O11", Description = "Price for 0:0/4+"},
                                new OddNodeAttrFlex { XmlAttribute = "o12", DBAttribute = "", OddFlexBetChoice = "1:0/1:0", MatchOddAttribute = "O12", Description = "Price for 1:0/1:0"},
                                new OddNodeAttrFlex { XmlAttribute = "o13", DBAttribute = "", OddFlexBetChoice = "1:0/2:0", MatchOddAttribute = "O13", Description = "Price for 1:0/2:0"},
                                new OddNodeAttrFlex { XmlAttribute = "o14", DBAttribute = "", OddFlexBetChoice = "1:0/1:1", MatchOddAttribute = "O14", Description = "Price for 1:0/1:1"},
                                new OddNodeAttrFlex { XmlAttribute = "o15", DBAttribute = "", OddFlexBetChoice = "1:0/3:0", MatchOddAttribute = "O15", Description = "Price for 1:0/3:0"},
                                new OddNodeAttrFlex { XmlAttribute = "o16", DBAttribute = "", OddFlexBetChoice = "1:0/2:1", MatchOddAttribute = "O16", Description = "Price for 1:0/2:1"},
                                new OddNodeAttrFlex { XmlAttribute = "o17", DBAttribute = "", OddFlexBetChoice = "1:0/1:2", MatchOddAttribute = "O17", Description = "Price for 1:0/1:2"},
                                new OddNodeAttrFlex { XmlAttribute = "o18", DBAttribute = "", OddFlexBetChoice = "1:0/4+", MatchOddAttribute = "O18", Description = "Price for 1:0/4+"},
                                new OddNodeAttrFlex { XmlAttribute = "o19", DBAttribute = "", OddFlexBetChoice = "0:1/0:1", MatchOddAttribute = "O19", Description = "Price for 0:1/0:1"},
                                new OddNodeAttrFlex { XmlAttribute = "o20", DBAttribute = "", OddFlexBetChoice = "0:1/1:1", MatchOddAttribute = "O20", Description = "Price for 0:1/1:1"},
                                new OddNodeAttrFlex { XmlAttribute = "o21", DBAttribute = "", OddFlexBetChoice = "0:1/0:2", MatchOddAttribute = "O21", Description = "Price for 0:1/0:2"},
                                new OddNodeAttrFlex { XmlAttribute = "o22", DBAttribute = "", OddFlexBetChoice = "0:1/2:1", MatchOddAttribute = "O22", Description = "Price for 0:1/2:1"},
                                new OddNodeAttrFlex { XmlAttribute = "o23", DBAttribute = "", OddFlexBetChoice = "0:1/1:2", MatchOddAttribute = "O23", Description = "Price for 0:1/1:2"},
                                new OddNodeAttrFlex { XmlAttribute = "o24", DBAttribute = "", OddFlexBetChoice = "0:1/0:3", MatchOddAttribute = "O24", Description = "Price for 0:1/0:3"},
                                new OddNodeAttrFlex { XmlAttribute = "o25", DBAttribute = "", OddFlexBetChoice = "0:1/4+", MatchOddAttribute = "O25", Description = "Price for 0:1/4+"},
                                new OddNodeAttrFlex { XmlAttribute = "o26", DBAttribute = "", OddFlexBetChoice = "1:1/1:1", MatchOddAttribute = "O26", Description = "Price for 1:1/1:1"},
                                new OddNodeAttrFlex { XmlAttribute = "o27", DBAttribute = "", OddFlexBetChoice = "1:1/2:1", MatchOddAttribute = "O27", Description = "Price for 1:1/2:1"},
                                new OddNodeAttrFlex { XmlAttribute = "o28", DBAttribute = "", OddFlexBetChoice = "1:1/1:2", MatchOddAttribute = "O28", Description = "Price for 1:1/1:2"},
                                new OddNodeAttrFlex { XmlAttribute = "o29", DBAttribute = "", OddFlexBetChoice = "1:1/4+", MatchOddAttribute = "O29", Description = "Price for 1:1/4+"},
                                new OddNodeAttrFlex { XmlAttribute = "o30", DBAttribute = "", OddFlexBetChoice = "2:0/2:0", MatchOddAttribute = "O30", Description = "Price for 2:0/2:0"},
                                new OddNodeAttrFlex { XmlAttribute = "o31", DBAttribute = "", OddFlexBetChoice = "2:0/3:0", MatchOddAttribute = "O31", Description = "Price for 2:0/3:0"},
                                new OddNodeAttrFlex { XmlAttribute = "o32", DBAttribute = "", OddFlexBetChoice = "2:0/2:1", MatchOddAttribute = "O32", Description = "Price for 2:0/2:1"},
                                new OddNodeAttrFlex { XmlAttribute = "o33", DBAttribute = "", OddFlexBetChoice = "2:0/4+", MatchOddAttribute = "O33", Description = "Price for 2:0/4+"},
                                new OddNodeAttrFlex { XmlAttribute = "o34", DBAttribute = "", OddFlexBetChoice = "0:2/0:2", MatchOddAttribute = "O34", Description = "Price for 0:2/0:2"},
                                new OddNodeAttrFlex { XmlAttribute = "o35", DBAttribute = "", OddFlexBetChoice = "0:2/1:2", MatchOddAttribute = "O35", Description = "Price for 0:2/1:2"},
                                new OddNodeAttrFlex { XmlAttribute = "o36", DBAttribute = "", OddFlexBetChoice = "0:2/0:3", MatchOddAttribute = "O36", Description = "Price for 0:2/0:3"},
                                new OddNodeAttrFlex { XmlAttribute = "o37", DBAttribute = "", OddFlexBetChoice = "0:2/4+", MatchOddAttribute = "O37", Description = "Price for 0:2/4+"},
                                new OddNodeAttrFlex { XmlAttribute = "o38", DBAttribute = "", OddFlexBetChoice = "2:1/2:1", MatchOddAttribute = "O38", Description = "Price for 2:1/2:1"},
                                new OddNodeAttrFlex { XmlAttribute = "o39", DBAttribute = "", OddFlexBetChoice = "2:1/4+", MatchOddAttribute = "O39", Description = "Price for 2:1/4+"},
                                new OddNodeAttrFlex { XmlAttribute = "o40", DBAttribute = "", OddFlexBetChoice = "1:2/1:2", MatchOddAttribute = "O40", Description = "Price for 1:2/1:2"},
                                new OddNodeAttrFlex { XmlAttribute = "o41", DBAttribute = "", OddFlexBetChoice = "1:2/4+", MatchOddAttribute = "O41", Description = "Price for 1:2/4+"},
                                new OddNodeAttrFlex { XmlAttribute = "o42", DBAttribute = "", OddFlexBetChoice = "3:0/3:0", MatchOddAttribute = "O42", Description = "Price for 3:0/3:0"},
                                new OddNodeAttrFlex { XmlAttribute = "o43", DBAttribute = "", OddFlexBetChoice = "3:0/4+", MatchOddAttribute = "O43", Description = "Price for 3:0/4+"},
                                new OddNodeAttrFlex { XmlAttribute = "o44", DBAttribute = "", OddFlexBetChoice = "0:3/0:3", MatchOddAttribute = "O44", Description = "Price for 0:3/0:3"},
                                new OddNodeAttrFlex { XmlAttribute = "o45", DBAttribute = "", OddFlexBetChoice = "0:3/4+", MatchOddAttribute = "O45", Description = "Price for 0:3/4+"},
                                new OddNodeAttrFlex { XmlAttribute = "o46", DBAttribute = "", OddFlexBetChoice = "4+/4+", MatchOddAttribute = "O46", Description = "Price for 4+/4+"}
                     }
            },
            new OddNodeBase
            {
                ID = 427,
                Tag = "b_1",
                Desc = "1H Both Teams To Score (own)",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[2] {
                                new OddNodeAttrBase { XmlAttribute = "y", DBAttribute = "odds.odds1a", MatchOddAttribute = "Y", Description = "Price for Yes"},
                                new OddNodeAttrBase { XmlAttribute = "n", DBAttribute = "odds.odds2a", MatchOddAttribute = "N", Description = "Price for No"}
                     }
            },
            new OddNodeBase
            {
                ID = 428,
                Tag = "oe_2",
                Desc = "2H Odd/Even (own)",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[2] {
                                new OddNodeAttrBase { XmlAttribute = "o", DBAttribute = "odds.odds1a", MatchOddAttribute = "O", Description = "Price for Odd"},
                                new OddNodeAttrBase { XmlAttribute = "e", DBAttribute = "odds.odds2a", MatchOddAttribute = "E", Description = "Price for Even"}
                     }
            },
            new OddNodeBase
            {
                ID = 430,
                Tag = "st_2",
                Desc = "2H 1X2 (own)",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[3] {
                                new OddNodeAttrBase { XmlAttribute = "h", DBAttribute = "odds.com1", MatchOddAttribute = "H", Description = "Price for Home"},
                                new OddNodeAttrBase { XmlAttribute = "n", DBAttribute = "odds.comx", MatchOddAttribute = "N", Description = "Price for None"},
                                new OddNodeAttrBase { XmlAttribute = "a", DBAttribute = "odds.com2", MatchOddAttribute = "A", Description = "Price for Away"}
                     }
            },
            new OddNodeBase
            {
                ID = 431,
                Tag = "x2_2",
                Desc = "2H Double Chance (own)",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[3] {
                                new OddNodeAttrBase { XmlAttribute = "hd", DBAttribute = "odds.cs10", MatchOddAttribute = "HD", Description = "Price for Home Draw"},
                                new OddNodeAttrBase { XmlAttribute = "ha", DBAttribute = "odds.cs20", MatchOddAttribute = "HA", Description = "Price for Home Away"},
                                new OddNodeAttrBase { XmlAttribute = "ad", DBAttribute = "odds.cs00", MatchOddAttribute = "AD", Description = "Price for Away Draw"}
                     }
            },
            new OddNodeBase
            {
                ID = 432,
                Tag = "dnb_2",
                Desc = "2H Draw No Bet (own)",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[2] {
                                new OddNodeAttrBase { XmlAttribute = "h", DBAttribute = "odds.odds1a", MatchOddAttribute = "H", Description = "Price for Home"},
                                new OddNodeAttrBase { XmlAttribute = "a", DBAttribute = "odds.odds2a", MatchOddAttribute = "A", Description = "Price for Away"}
                     }
            },
            new OddNodeBase
            {
                ID = 433,
                Tag = "bs_2",
                Desc = "2H Both Teams To Score (own)",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[2] {
                                new OddNodeAttrBase { XmlAttribute = "y", DBAttribute = "odds.com1", MatchOddAttribute = "Y", Description = "Price for Yes"},
                                new OddNodeAttrBase { XmlAttribute = "n", DBAttribute = "odds.com2", MatchOddAttribute = "N", Description = "Price for No"}
                     }
            },
            new OddNodeBase
            {
                ID = 434,
                Tag = "oyn",
                Desc = "Both Halves Over 1.5 Yes/No (own)",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[2] {
                                new OddNodeAttrBase { XmlAttribute = "y", DBAttribute = "odds.odds1a", MatchOddAttribute = "Y", Description = "Price for Yes"},
                                new OddNodeAttrBase { XmlAttribute = "n", DBAttribute = "odds.odds2a", MatchOddAttribute = "N", Description = "Price for No"}
                     }
            },
            new OddNodeBase
            {
                ID = 435,
                Tag = "uyn",
                Desc = "Both Halves Under 1.5 Yes/No (own)",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[2] {
                                new OddNodeAttrBase { XmlAttribute = "y", DBAttribute = "odds.odds1a", MatchOddAttribute = "Y", Description = "Price for Yes"},
                                new OddNodeAttrBase { XmlAttribute = "n", DBAttribute = "odds.odds2a", MatchOddAttribute = "N", Description = "Price for No"}
                     }
            },
            new OddNodeBase
            {
                ID = 436,
                Tag = "hs",
                Desc = "Home To Score In Both Halves (own)",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[2] {
                                new OddNodeAttrBase { XmlAttribute = "y", DBAttribute = "odds.com1", MatchOddAttribute = "Y", Description = "Price for Yes"},
                                new OddNodeAttrBase { XmlAttribute = "n", DBAttribute = "odds.com2", MatchOddAttribute = "N", Description = "Price for No"}
                     }
            },
            new OddNodeBase
            {
                ID = 437,
                Tag = "as",
                Desc = "Away To Score In Both Halves (own)",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[2] {
                                new OddNodeAttrBase { XmlAttribute = "y", DBAttribute = "odds.com1", MatchOddAttribute = "Y", Description = "Price for Yes"},
                                new OddNodeAttrBase { XmlAttribute = "n", DBAttribute = "odds.com2", MatchOddAttribute = "N", Description = "Price for No"}
                     }
            },
            new OddNodeBase
            {
                ID = 438,
                Tag = "h2h",
                Desc = "Home To Win Both Halves (own)",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[2] {
                                new OddNodeAttrBase { XmlAttribute = "y", DBAttribute = "odds.com1", MatchOddAttribute = "Y", Description = "Price for Yes"},
                                new OddNodeAttrBase { XmlAttribute = "n", DBAttribute = "odds.com2", MatchOddAttribute = "N", Description = "Price for No"}
                     }
            },
            new OddNodeBase
            {
                ID = 439,
                Tag = "a2h",
                Desc = "Away To Win Both Halves (own)",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[2] {
                                new OddNodeAttrBase { XmlAttribute = "y", DBAttribute = "odds.com1", MatchOddAttribute = "Y", Description = "Price for Yes"},
                                new OddNodeAttrBase { XmlAttribute = "n", DBAttribute = "odds.com2", MatchOddAttribute = "N", Description = "Price for No"}
                     }
            },
            new OddNodeBase
            {
                ID = 440,
                Tag = "hw",
                Desc = "Home To Win Either Half (own)",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[2] {
                                new OddNodeAttrBase { XmlAttribute = "y", DBAttribute = "odds.com1", MatchOddAttribute = "Y", Description = "Price for Yes"},
                                new OddNodeAttrBase { XmlAttribute = "n", DBAttribute = "odds.com2", MatchOddAttribute = "N", Description = "Price for No"}
                     }
            },
            new OddNodeBase
            {
                ID = 441,
                Tag = "aw",
                Desc = "Away To Win Either Half (own)",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[2] {
                                new OddNodeAttrBase { XmlAttribute = "y", DBAttribute = "odds.com1", MatchOddAttribute = "Y", Description = "Price for Yes"},
                                new OddNodeAttrBase { XmlAttribute = "n", DBAttribute = "odds.com2", MatchOddAttribute = "N", Description = "Price for No"}
                     }
            },
            new OddNodeBase
            {
                ID = 442,
                Tag = "high",
                Desc = "Highest Scoring Half (own)",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[3] {
                                new OddNodeAttrBase { XmlAttribute = "h1", DBAttribute = "odds.cs10", MatchOddAttribute = "H1", Description = "Price for First half"},
                                new OddNodeAttrBase { XmlAttribute = "h2", DBAttribute = "odds.cs20", MatchOddAttribute = "H2", Description = "Price for Second half"},
                                new OddNodeAttrBase { XmlAttribute = "tie", DBAttribute = "odds.cs00", MatchOddAttribute = "TIE", Description = "Price for Tie"}
                     }
            },
            new OddNodeBase
            {
                ID = 443,
                Tag = "hhigh",
                Desc = "Highest Scoring Half Home Team (own)",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[3] {
                                new OddNodeAttrBase { XmlAttribute = "h1", DBAttribute = "odds.cs10", MatchOddAttribute = "H1", Description = "Price for First half"},
                                new OddNodeAttrBase { XmlAttribute = "h2", DBAttribute = "odds.cs20", MatchOddAttribute = "H2", Description = "Price for Second half"},
                                new OddNodeAttrBase { XmlAttribute = "tie", DBAttribute = "odds.cs00", MatchOddAttribute = "TIE", Description = "Price for Tie"}
                     }
            },
            new OddNodeBase
            {
                ID = 444,
                Tag = "ahigh",
                Desc = "Highest Scoring Half Away Team (own)",
                IsChildBettype = true,
                Odds = new OddNodeAttrBase[3] {
                                new OddNodeAttrBase { XmlAttribute = "h1", DBAttribute = "odds.cs10", MatchOddAttribute = "H1", Description = "Price for First half"},
                                new OddNodeAttrBase { XmlAttribute = "h2", DBAttribute = "odds.cs20", MatchOddAttribute = "H2", Description = "Price for Second half"},
                                new OddNodeAttrBase { XmlAttribute = "tie", DBAttribute = "odds.cs00", MatchOddAttribute = "TIE", Description = "Price for Tie"}
                     }
            }
        };
    }
}
