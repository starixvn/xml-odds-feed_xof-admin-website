namespace Nexcel.OddsFeed.Library.Matchs.OddNodeBuilder
{
    using System.Collections.Generic;

    class BetOddNodeMapping
    {
        // Notes:
        // When adding a new bettype here, remember to check if the Tag is already in old bettypes yet
        // The idea to build xml based on Tag is not ok anymore, need to change to use bettype ID instead
        public static readonly List<OddNodeBase> BetTypes = new List<OddNodeBase> {
            new OddNodeBase
            {
                ID = 2,
                Tag = "oe",
                Odds = new OddNodeAttrBase[2] {
                    new OddNodeAttrBase { XmlAttribute = "o", DBAttribute = "odds1a", MatchOddAttribute = "O", Description = "Odd"},
                    new OddNodeAttrBase { XmlAttribute = "e", DBAttribute = "odds2a", MatchOddAttribute = "E", Description = "Even"}
                }
            },
            new OddNodeBase
            {
                ID = 401,
                Tag = "ou_h",
                Odds = new OddNodeAttrBase[3] {
                    new OddNodeAttrBase { XmlAttribute = "o", DBAttribute = "odds1a", MatchOddAttribute = "O", Description = "Over"},
                    new OddNodeAttrBase { XmlAttribute = "u", DBAttribute = "odds2a", MatchOddAttribute = "U", Description = "Under"},
                    new OddNodeAttrHandicap { XmlAttribute = "h", DBAttribute = "hdp1", MatchOddAttribute = "H", Description = "Over/Under value"}
                }
            },
            new OddNodeBase
            {
                ID = 402,
                Tag = "ou_a",
                Odds = new OddNodeAttrBase[3] {
                    new OddNodeAttrBase { XmlAttribute = "o", DBAttribute = "odds1a", MatchOddAttribute = "O", Description = "Over"},
                    new OddNodeAttrBase { XmlAttribute = "u", DBAttribute = "odds2a", MatchOddAttribute = "U", Description = "Under"},
                    new OddNodeAttrHandicap { XmlAttribute = "h", DBAttribute = "hdp1", MatchOddAttribute = "H", Description = "Over/Under value"}
                }
            },
            new OddNodeBase
            {
                ID = 403,
                Tag = "ou_h1",
                Odds = new OddNodeAttrBase[3] {
                    new OddNodeAttrBase { XmlAttribute = "o", DBAttribute = "odds1a", MatchOddAttribute = "O", Description = "Over"},
                    new OddNodeAttrBase { XmlAttribute = "u", DBAttribute = "odds2a", MatchOddAttribute = "U", Description = "Under"},
                    new OddNodeAttrHandicap { XmlAttribute = "h", DBAttribute = "hdp1", MatchOddAttribute = "H", Description = "Over/Under value"}
                }
            },
            new OddNodeBase
            {
                ID = 404,
                Tag = "ou_a1",
                Odds = new OddNodeAttrBase[3] {
                    new OddNodeAttrBase { XmlAttribute = "o", DBAttribute = "odds1a", MatchOddAttribute = "O", Description = "Over"},
                    new OddNodeAttrBase { XmlAttribute = "u", DBAttribute = "odds2a", MatchOddAttribute = "U", Description = "Under"},
                    new OddNodeAttrHandicap { XmlAttribute = "h", DBAttribute = "hdp1", MatchOddAttribute = "H", Description = "Over/Under value"}
                }
            },
            new OddNodeBase
            {
                ID = 405,
                Tag = "cs_2",
                Odds = new OddNodeAttrBase[17] {
                    new OddNodeAttrBase { XmlAttribute = "cs10", MatchOddAttribute = "CS10", Description = "1-0", DBAttribute = "cs10"},
                    new OddNodeAttrBase { XmlAttribute = "cs20", MatchOddAttribute = "CS20", Description = "2-0", DBAttribute = "cs20"},
                    new OddNodeAttrBase { XmlAttribute = "cs21", MatchOddAttribute = "CS21", Description = "2-1", DBAttribute = "cs21"},
                    new OddNodeAttrBase { XmlAttribute = "cs30", MatchOddAttribute = "CS30", Description = "3-0", DBAttribute = "cs30"},
                    new OddNodeAttrBase { XmlAttribute = "cs31", MatchOddAttribute = "CS31", Description = "3-1", DBAttribute = "cs31"},
                    new OddNodeAttrBase { XmlAttribute = "cs32", MatchOddAttribute = "CS32", Description = "3-2", DBAttribute = "cs32"},
                    new OddNodeAttrBase { XmlAttribute = "cs01", MatchOddAttribute = "CS01", Description = "0-1", DBAttribute = "cs01"},
                    new OddNodeAttrBase { XmlAttribute = "cs02", MatchOddAttribute = "CS02", Description = "0-2", DBAttribute = "cs02"},
                    new OddNodeAttrBase { XmlAttribute = "cs12", MatchOddAttribute = "CS12", Description = "1-2", DBAttribute = "cs12"},
                    new OddNodeAttrBase { XmlAttribute = "cs03", MatchOddAttribute = "CS03", Description = "0-3", DBAttribute = "cs03"},
                    new OddNodeAttrBase { XmlAttribute = "cs13", MatchOddAttribute = "CS13", Description = "1-3", DBAttribute = "cs13"},
                    new OddNodeAttrBase { XmlAttribute = "cs23", MatchOddAttribute = "CS23", Description = "2-3", DBAttribute = "cs23"},
                    new OddNodeAttrBase { XmlAttribute = "cs00", MatchOddAttribute = "CS00", Description = "0-0", DBAttribute = "cs00"},
                    new OddNodeAttrBase { XmlAttribute = "cs11", MatchOddAttribute = "CS11", Description = "1-1", DBAttribute = "cs11"},
                    new OddNodeAttrBase { XmlAttribute = "cs22", MatchOddAttribute = "CS22", Description = "2-2", DBAttribute = "cs22"},
                    new OddNodeAttrBase { XmlAttribute = "cs33", MatchOddAttribute = "CS33", Description = "3-3", DBAttribute = "cs33"},
                    new OddNodeAttrBase { XmlAttribute = "csoas", MatchOddAttribute = "CSOAS", Description = "AOS", DBAttribute = "cs99"}
                }
            },
            new OddNodeBase
            {
                ID = 406,
                Tag = "etg",
                Odds = new OddNodeAttrBase[7] {
                    new OddNodeAttrBase { XmlAttribute = "o0", MatchOddAttribute = "O0", Description = "0", DBAttribute = "cs01"},
                    new OddNodeAttrBase { XmlAttribute = "o1", MatchOddAttribute = "O1", Description = "1", DBAttribute = "cs02"},
                    new OddNodeAttrBase { XmlAttribute = "o2", MatchOddAttribute = "O2", Description = "2", DBAttribute = "cs03"},
                    new OddNodeAttrBase { XmlAttribute = "o3", MatchOddAttribute = "O3", Description = "3", DBAttribute = "cs04"},
                    new OddNodeAttrBase { XmlAttribute = "o4", MatchOddAttribute = "O4", Description = "4", DBAttribute = "cs10"},
                    new OddNodeAttrBase { XmlAttribute = "o5", MatchOddAttribute = "O5", Description = "5", DBAttribute = "cs20"},
                    new OddNodeAttrBase { XmlAttribute = "o6", MatchOddAttribute = "O6", Description = "6&Over", DBAttribute = "cs30"}
                }
            },
            new OddNodeBase
            {
                ID = 407,
                Tag = "heg",
                Odds = new OddNodeAttrBase[4] {
                    new OddNodeAttrBase { XmlAttribute = "o0", MatchOddAttribute = "O0", Description = "0", DBAttribute = "cs00"},
                    new OddNodeAttrBase { XmlAttribute = "o1", MatchOddAttribute = "O1", Description = "1", DBAttribute = "cs01"},
                    new OddNodeAttrBase { XmlAttribute = "o2", MatchOddAttribute = "O2", Description = "2", DBAttribute = "cs02"},
                    new OddNodeAttrBase { XmlAttribute = "o3", MatchOddAttribute = "O3", Description = "3&Over", DBAttribute = "cs03"}
                }
            },
            new OddNodeBase
            {
                ID = 408,
                Tag = "win",
                Odds = new OddNodeAttrBase[8] {
                    new OddNodeAttrBase { XmlAttribute = "h1", MatchOddAttribute = "H1", Description = "Home & 1 Goal", DBAttribute = "cs01"},
                    new OddNodeAttrBase { XmlAttribute = "h2", MatchOddAttribute = "H2", Description = "Home & 2 Goals", DBAttribute = "cs02"},
                    new OddNodeAttrBase { XmlAttribute = "h3", MatchOddAttribute = "H3", Description = "Home & 3 Goals up", DBAttribute = "cs03"},
                    new OddNodeAttrBase { XmlAttribute = "d", MatchOddAttribute = "D", Description = "S.Draw", DBAttribute = "cs04"},
                    new OddNodeAttrBase { XmlAttribute = "a1", MatchOddAttribute = "A1", Description = "Away & 1 Goal", DBAttribute = "cs10"},
                    new OddNodeAttrBase { XmlAttribute = "a2", MatchOddAttribute = "A2", Description = "Away & 2 Goals", DBAttribute = "cs20"},
                    new OddNodeAttrBase { XmlAttribute = "a3", MatchOddAttribute = "A3", Description = "Away & 3 Goals up", DBAttribute = "cs30"},
                    new OddNodeAttrBase { XmlAttribute = "ng", MatchOddAttribute = "NG", Description = "No Goal", DBAttribute = "cs00" }
                }
            },
            new OddNodeBase
            {
                ID = 409,
                Tag = "aeg",
                Odds = new OddNodeAttrBase[4] {
                    new OddNodeAttrBase { XmlAttribute = "o0", MatchOddAttribute = "O0", Description = "0", DBAttribute = "cs00"},
                    new OddNodeAttrBase { XmlAttribute = "o1", MatchOddAttribute = "O1", Description = "1", DBAttribute = "cs01"},
                    new OddNodeAttrBase { XmlAttribute = "o2", MatchOddAttribute = "O2", Description = "2", DBAttribute = "cs02"},
                    new OddNodeAttrBase { XmlAttribute = "o3", MatchOddAttribute = "O3", Description = "3 Goals & Over", DBAttribute = "cs03"}
                }
            },
            new OddNodeBase
            {
                ID = 410,
                Tag = "x2_1",
                Odds = new OddNodeAttrBase[3] {
                    new OddNodeAttrBase { XmlAttribute = "hd", MatchOddAttribute = "HD", Description = "Home or Draw", DBAttribute = "cs10"},
                    new OddNodeAttrBase { XmlAttribute = "ha", MatchOddAttribute = "HA", Description = "Home or Away", DBAttribute = "cs00"},
                    new OddNodeAttrBase { XmlAttribute = "ad", MatchOddAttribute = "AD", Description = "Away or Draw", DBAttribute = "cs20"}
                }
            },
            new OddNodeBase
            {
                ID = 411,
                Tag = "dnb_1",
                Odds = new OddNodeAttrBase[2] {
                    new OddNodeAttrBase { XmlAttribute = "h", MatchOddAttribute = "H", Description = "Home", DBAttribute = "odds1a"},
                    new OddNodeAttrBase { XmlAttribute = "a", MatchOddAttribute = "A", Description = "Away", DBAttribute = "odds2a"}
                }
            },
            new OddNodeBase
            {
                ID = 412,
                Tag = "egs_1st",
                Odds = new OddNodeAttrBase[4] {
                    new OddNodeAttrBase { XmlAttribute = "o0", MatchOddAttribute = "O0", Description = "0", DBAttribute = "cs00"},
                    new OddNodeAttrBase { XmlAttribute = "o1", MatchOddAttribute = "O1", Description = "1", DBAttribute = "cs01"},
                    new OddNodeAttrBase { XmlAttribute = "o2", MatchOddAttribute = "O2", Description = "2", DBAttribute = "cs10"},
                    new OddNodeAttrBase { XmlAttribute = "o3", MatchOddAttribute = "O3", Description = "3 Goals & Over", DBAttribute = "cs11"}
                }
            },
            new OddNodeBase
            {
                ID = 413,
                Tag = "cs",
                Odds = new OddNodeAttrBase[28] {
                    new OddNodeAttrBase { XmlAttribute = "cs10", MatchOddAttribute = "CS10", Description = "1-0", DBAttribute = "cs10"},
                    new OddNodeAttrBase { XmlAttribute = "cs20", MatchOddAttribute = "CS20", Description = "2-0", DBAttribute = "cs20"},
                    new OddNodeAttrBase { XmlAttribute = "cs21", MatchOddAttribute = "CS21", Description = "2-1", DBAttribute = "cs21"},
                    new OddNodeAttrBase { XmlAttribute = "cs30", MatchOddAttribute = "CS30", Description = "3-0", DBAttribute = "cs30"},
                    new OddNodeAttrBase { XmlAttribute = "cs31", MatchOddAttribute = "CS31", Description = "3-1", DBAttribute = "cs31"},
                    new OddNodeAttrBase { XmlAttribute = "cs32", MatchOddAttribute = "CS32", Description = "3-2", DBAttribute = "cs32"},
                    new OddNodeAttrBase { XmlAttribute = "cs40", MatchOddAttribute = "CS40", Description = "4-0", DBAttribute = "cs40"},
                    new OddNodeAttrBase { XmlAttribute = "cs41", MatchOddAttribute = "CS41", Description = "4-1", DBAttribute = "cs41"},
                    new OddNodeAttrBase { XmlAttribute = "cs42", MatchOddAttribute = "CS42", Description = "4-2", DBAttribute = "cs42"},
                    new OddNodeAttrBase { XmlAttribute = "cs43", MatchOddAttribute = "CS43", Description = "4-3", DBAttribute = "cs43"},
                    new OddNodeAttrBase { XmlAttribute = "cs01", MatchOddAttribute = "CS01", Description = "0-1", DBAttribute = "cs01"},
                    new OddNodeAttrBase { XmlAttribute = "cs02", MatchOddAttribute = "CS02", Description = "0-2", DBAttribute = "cs02"},
                    new OddNodeAttrBase { XmlAttribute = "cs12", MatchOddAttribute = "CS12", Description = "1-2", DBAttribute = "cs12"},
                    new OddNodeAttrBase { XmlAttribute = "cs03", MatchOddAttribute = "CS03", Description = "0-3", DBAttribute = "cs03"},
                    new OddNodeAttrBase { XmlAttribute = "cs13", MatchOddAttribute = "CS13", Description = "1-3", DBAttribute = "cs13"},
                    new OddNodeAttrBase { XmlAttribute = "cs23", MatchOddAttribute = "CS23", Description = "2-3", DBAttribute = "cs23"},
                    new OddNodeAttrBase { XmlAttribute = "cs04", MatchOddAttribute = "CS04", Description = "0-4", DBAttribute = "cs04"},
                    new OddNodeAttrBase { XmlAttribute = "cs14", MatchOddAttribute = "CS14", Description = "1-4", DBAttribute = "cs14"},
                    new OddNodeAttrBase { XmlAttribute = "cs24", MatchOddAttribute = "CS24", Description = "2-4", DBAttribute = "cs24"},
                    new OddNodeAttrBase { XmlAttribute = "cs34", MatchOddAttribute = "CS34", Description = "3-4", DBAttribute = "cs34"},
                    new OddNodeAttrBase { XmlAttribute = "cs00", MatchOddAttribute = "CS00", Description = "0-0", DBAttribute = "cs00"},
                    new OddNodeAttrBase { XmlAttribute = "cs11", MatchOddAttribute = "CS11", Description = "1-1", DBAttribute = "cs11"},
                    new OddNodeAttrBase { XmlAttribute = "cs22", MatchOddAttribute = "CS22", Description = "2-2", DBAttribute = "cs22"},
                    new OddNodeAttrBase { XmlAttribute = "cs33", MatchOddAttribute = "CS33", Description = "3-3", DBAttribute = "cs33"},
                    new OddNodeAttrBase { XmlAttribute = "cs44", MatchOddAttribute = "CS44", Description = "4-4", DBAttribute = "cs44"},
                    new OddNodeAttrBase { XmlAttribute = "csoas", MatchOddAttribute = "CSOAS", Description = "AOS", DBAttribute = "cs99"},
                    // for bettype 4
                    new OddNodeAttrBase { NotToBuildData = true, XmlAttribute = "cs05", MatchOddAttribute = "CS05", Description = "0-5 UP", DBAttribute = ""},
                    new OddNodeAttrBase { NotToBuildData = true, XmlAttribute = "cs50", MatchOddAttribute = "CS50", Description = "5-0 UP", DBAttribute = ""}
                }
            },
            new OddNodeBase
            {
                ID = 414,
                Tag = "cs_1",
                Odds = new OddNodeAttrBase[19] {
                    new OddNodeAttrBase { XmlAttribute = "cs10", MatchOddAttribute = "CS10", Description = "1-0", DBAttribute = "cs10"},
                    new OddNodeAttrBase { XmlAttribute = "cs20", MatchOddAttribute = "CS20", Description = "2-0", DBAttribute = "cs20"},
                    new OddNodeAttrBase { XmlAttribute = "cs21", MatchOddAttribute = "CS21", Description = "2-1", DBAttribute = "cs21"},
                    new OddNodeAttrBase { XmlAttribute = "cs30", MatchOddAttribute = "CS30", Description = "3-0", DBAttribute = "cs30"},
                    new OddNodeAttrBase { XmlAttribute = "cs31", MatchOddAttribute = "CS31", Description = "3-1", DBAttribute = "cs31"},
                    new OddNodeAttrBase { XmlAttribute = "cs32", MatchOddAttribute = "CS32", Description = "3-2", DBAttribute = "cs32"},
                    new OddNodeAttrBase { XmlAttribute = "cs01", MatchOddAttribute = "CS01", Description = "0-1", DBAttribute = "cs01"},
                    new OddNodeAttrBase { XmlAttribute = "cs02", MatchOddAttribute = "CS02", Description = "0-2", DBAttribute = "cs02"},
                    new OddNodeAttrBase { XmlAttribute = "cs12", MatchOddAttribute = "CS12", Description = "1-2", DBAttribute = "cs12"},
                    new OddNodeAttrBase { XmlAttribute = "cs03", MatchOddAttribute = "CS03", Description = "0-3", DBAttribute = "cs03"},
                    new OddNodeAttrBase { XmlAttribute = "cs13", MatchOddAttribute = "CS13", Description = "1-3", DBAttribute = "cs13"},
                    new OddNodeAttrBase { XmlAttribute = "cs23", MatchOddAttribute = "CS23", Description = "2-3", DBAttribute = "cs23"},
                    new OddNodeAttrBase { XmlAttribute = "cs00", MatchOddAttribute = "CS00", Description = "0-0", DBAttribute = "cs00"},
                    new OddNodeAttrBase { XmlAttribute = "cs11", MatchOddAttribute = "CS11", Description = "1-1", DBAttribute = "cs11"},
                    new OddNodeAttrBase { XmlAttribute = "cs22", MatchOddAttribute = "CS22", Description = "2-2", DBAttribute = "cs22"},
                    new OddNodeAttrBase { XmlAttribute = "cs33", MatchOddAttribute = "CS33", Description = "3-3", DBAttribute = "cs33"},
                    new OddNodeAttrBase { XmlAttribute = "csoas", MatchOddAttribute = "CSOAS", Description = "AOS", DBAttribute = "cs99"},
                    // for bettype 30
                    new OddNodeAttrBase { NotToBuildData = true, XmlAttribute = "cs04", MatchOddAttribute = "CS04", Description = "0-4 UP", DBAttribute = ""},
                    new OddNodeAttrBase { NotToBuildData = true,  XmlAttribute = "cs40", MatchOddAttribute = "CS40", Description = "4-0 UP", DBAttribute = ""}
                }
            }
        };
    }
}
