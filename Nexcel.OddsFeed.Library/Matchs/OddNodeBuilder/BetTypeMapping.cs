namespace Nexcel.OddsFeed.Library.Matchs.OddNodeBuilder
{
    using System.Collections.Generic;

    class BetTypeMapping
    {
        public static readonly List<BetTypeNodeBase> BetTypes = new List<BetTypeNodeBase> {
            new BetTypeNodeBase
            {
                ID = 401,
                Tag = "ou_h",
                Odds = new OddNodeBase[3] {
                    new OddNodeBase { XmlAttribute = "o", DBAttribute = "odds1a", MatchOddAttribute = "O", Description = "Over"},
                    new OddNodeBase { XmlAttribute = "u", DBAttribute = "odds2a", MatchOddAttribute = "U", Description = "Under"},
                    new OddNodeBase { XmlAttribute = "h", DBAttribute = "hdp1", MatchOddAttribute = "H", Description = "Over/Under value"}
                }
            },
            new BetTypeNodeBase
            {
                ID = 402,
                Tag = "ou_a",
                Odds = new OddNodeBase[3] {
                    new OddNodeBase { XmlAttribute = "o", DBAttribute = "odds1a", MatchOddAttribute = "O", Description = "Over"},
                    new OddNodeBase { XmlAttribute = "u", DBAttribute = "odds2a", MatchOddAttribute = "U", Description = "Under"},
                    new OddNodeBase { XmlAttribute = "h", DBAttribute = "hdp1", MatchOddAttribute = "H", Description = "Over/Under value"}
                }
            },
            new BetTypeNodeBase
            {
                ID = 403,
                Tag = "ou_h1",
                Odds = new OddNodeBase[3] {
                    new OddNodeBase { XmlAttribute = "o", DBAttribute = "odds1a", MatchOddAttribute = "O", Description = "Over"},
                    new OddNodeBase { XmlAttribute = "u", DBAttribute = "odds2a", MatchOddAttribute = "U", Description = "Under"},
                    new OddNodeBase { XmlAttribute = "h", DBAttribute = "hdp1", MatchOddAttribute = "H", Description = "Over/Under value"}
                }
            },
            new BetTypeNodeBase
            {
                ID = 404,
                Tag = "ou_a1",
                Odds = new OddNodeBase[3] {
                    new OddNodeBase { XmlAttribute = "o", DBAttribute = "odds1a", MatchOddAttribute = "O", Description = "Over"},
                    new OddNodeBase { XmlAttribute = "u", DBAttribute = "odds2a", MatchOddAttribute = "U", Description = "Under"},
                    new OddNodeBase { XmlAttribute = "h", DBAttribute = "hdp1", MatchOddAttribute = "H", Description = "Over/Under value"}
                }
            },
            new BetTypeNodeBase
            {
                ID = 405,
                Tag = "cs_2",
                Odds = new OddNodeBase[17] {
                    new OddNodeBase { XmlAttribute = "cs10", MatchOddAttribute = "CS10", Description = "1-0", DBAttribute = "cs10"},
                    new OddNodeBase { XmlAttribute = "cs20", MatchOddAttribute = "CS20", Description = "2-0", DBAttribute = "cs20"},
                    new OddNodeBase { XmlAttribute = "cs21", MatchOddAttribute = "CS21", Description = "2-1", DBAttribute = "cs21"},
                    new OddNodeBase { XmlAttribute = "cs30", MatchOddAttribute = "CS30", Description = "3-0", DBAttribute = "cs30"},
                    new OddNodeBase { XmlAttribute = "cs31", MatchOddAttribute = "CS31", Description = "3-1", DBAttribute = "cs31"},
                    new OddNodeBase { XmlAttribute = "cs32", MatchOddAttribute = "CS32", Description = "3-2", DBAttribute = "cs32"},
                    new OddNodeBase { XmlAttribute = "cs01", MatchOddAttribute = "CS01", Description = "0-1", DBAttribute = "cs01"},
                    new OddNodeBase { XmlAttribute = "cs02", MatchOddAttribute = "CS02", Description = "0-2", DBAttribute = "cs02"},
                    new OddNodeBase { XmlAttribute = "cs12", MatchOddAttribute = "CS12", Description = "1-2", DBAttribute = "cs12"},
                    new OddNodeBase { XmlAttribute = "cs03", MatchOddAttribute = "CS03", Description = "0-3", DBAttribute = "cs03"},
                    new OddNodeBase { XmlAttribute = "cs13", MatchOddAttribute = "CS13", Description = "1-3", DBAttribute = "cs13"},
                    new OddNodeBase { XmlAttribute = "cs23", MatchOddAttribute = "CS23", Description = "2-3", DBAttribute = "cs23"},
                    new OddNodeBase { XmlAttribute = "cs00", MatchOddAttribute = "CS00", Description = "0-0", DBAttribute = "cs00"},
                    new OddNodeBase { XmlAttribute = "cs11", MatchOddAttribute = "CS11", Description = "1-1", DBAttribute = "cs11"},
                    new OddNodeBase { XmlAttribute = "cs22", MatchOddAttribute = "CS22", Description = "2-2", DBAttribute = "cs22"},
                    new OddNodeBase { XmlAttribute = "cs33", MatchOddAttribute = "CS33", Description = "3-3", DBAttribute = "cs33"},
                    new OddNodeBase { XmlAttribute = "csoas", MatchOddAttribute = "CSOAS", Description = "AOS", DBAttribute = "cs99"}
                }
            },
            new BetTypeNodeBase
            {
                ID = 406,
                Tag = "etg",
                Odds = new OddNodeBase[7] {
                    new OddNodeBase { XmlAttribute = "o0", MatchOddAttribute = "O0", Description = "0", DBAttribute = "cs01"},
                    new OddNodeBase { XmlAttribute = "o1", MatchOddAttribute = "O1", Description = "1", DBAttribute = "cs02"},
                    new OddNodeBase { XmlAttribute = "o2", MatchOddAttribute = "O2", Description = "2", DBAttribute = "cs03"},
                    new OddNodeBase { XmlAttribute = "o3", MatchOddAttribute = "O3", Description = "3", DBAttribute = "cs04"},
                    new OddNodeBase { XmlAttribute = "o4", MatchOddAttribute = "O4", Description = "4", DBAttribute = "cs10"},
                    new OddNodeBase { XmlAttribute = "o5", MatchOddAttribute = "O5", Description = "5", DBAttribute = "cs20"},
                    new OddNodeBase { XmlAttribute = "o6", MatchOddAttribute = "O6", Description = "6&Over", DBAttribute = "cs30"}
                }
            },
            new BetTypeNodeBase
            {
                ID = 407,
                Tag = "heg",
                Odds = new OddNodeBase[4] {
                    new OddNodeBase { XmlAttribute = "o0", MatchOddAttribute = "O0", Description = "0", DBAttribute = "cs00"},
                    new OddNodeBase { XmlAttribute = "o1", MatchOddAttribute = "O1", Description = "1", DBAttribute = "cs01"},
                    new OddNodeBase { XmlAttribute = "o2", MatchOddAttribute = "O2", Description = "2", DBAttribute = "cs02"},
                    new OddNodeBase { XmlAttribute = "o3", MatchOddAttribute = "O3", Description = "3&Over", DBAttribute = "cs03"}
                }
            },
            new BetTypeNodeBase
            {
                ID = 408,
                Tag = "win",
                Odds = new OddNodeBase[8] {
                    new OddNodeBase { XmlAttribute = "h1", MatchOddAttribute = "H1", Description = "Home & 1 Goal", DBAttribute = "cs01"},
                    new OddNodeBase { XmlAttribute = "h2", MatchOddAttribute = "H2", Description = "Home & 2 Goals", DBAttribute = "cs02"},
                    new OddNodeBase { XmlAttribute = "h3", MatchOddAttribute = "H3", Description = "Home & 3 Goals up", DBAttribute = "cs03"},
                    new OddNodeBase { XmlAttribute = "d", MatchOddAttribute = "D", Description = "S.Draw", DBAttribute = "cs04"},
                    new OddNodeBase { XmlAttribute = "a1", MatchOddAttribute = "A1", Description = "Away & 1 Goal", DBAttribute = "cs10"},
                    new OddNodeBase { XmlAttribute = "a2", MatchOddAttribute = "A2", Description = "Away & 2 Goals", DBAttribute = "cs20"},
                    new OddNodeBase { XmlAttribute = "a3", MatchOddAttribute = "A3", Description = "Away & 3 Goals up", DBAttribute = "cs30"},
                    new OddNodeBase { XmlAttribute = "ng", MatchOddAttribute = "NG", Description = "No Goal", DBAttribute = "cs00" }
                }
            },
            new BetTypeNodeBase
            {
                ID = 409,
                Tag = "aeg",
                Odds = new OddNodeBase[4] {
                    new OddNodeBase { XmlAttribute = "o0", MatchOddAttribute = "O0", Description = "0", DBAttribute = "cs00"},
                    new OddNodeBase { XmlAttribute = "o1", MatchOddAttribute = "O1", Description = "1", DBAttribute = "cs01"},
                    new OddNodeBase { XmlAttribute = "o2", MatchOddAttribute = "O2", Description = "2", DBAttribute = "cs02"},
                    new OddNodeBase { XmlAttribute = "o3", MatchOddAttribute = "O3", Description = "3 Goals & Over", DBAttribute = "cs03"}
                }
            },
            new BetTypeNodeBase
            {
                ID = 410,
                Tag = "x2_1",
                Odds = new OddNodeBase[3] {
                    new OddNodeBase { XmlAttribute = "hd", MatchOddAttribute = "HD", Description = "Home or Draw", DBAttribute = "cs10"},
                    new OddNodeBase { XmlAttribute = "ha", MatchOddAttribute = "HA", Description = "Home or Away", DBAttribute = "cs00"},
                    new OddNodeBase { XmlAttribute = "ad", MatchOddAttribute = "AD", Description = "Away or Draw", DBAttribute = "cs20"}
                }
            },
            new BetTypeNodeBase
            {
                ID = 411,
                Tag = "dnb_1",
                Odds = new OddNodeBase[2] {
                    new OddNodeBase { XmlAttribute = "h", MatchOddAttribute = "H", Description = "Home", DBAttribute = "odds1a"},
                    new OddNodeBase { XmlAttribute = "a", MatchOddAttribute = "A", Description = "Away", DBAttribute = "odds2a"}
                }
            },
            new BetTypeNodeBase
            {
                ID = 412,
                Tag = "egs_1st",
                Odds = new OddNodeBase[4] {
                    new OddNodeBase { XmlAttribute = "o0", MatchOddAttribute = "O0", Description = "0", DBAttribute = "cs00"},
                    new OddNodeBase { XmlAttribute = "o1", MatchOddAttribute = "O1", Description = "1", DBAttribute = "cs01"},
                    new OddNodeBase { XmlAttribute = "o2", MatchOddAttribute = "O2", Description = "2", DBAttribute = "cs10"},
                    new OddNodeBase { XmlAttribute = "o3", MatchOddAttribute = "O3", Description = "3 Goals & Over", DBAttribute = "cs11"}
                }
            },
            new BetTypeNodeBase
            {
                ID = 413,
                Tag = "cs",
                Odds = new OddNodeBase[26] {
                    new OddNodeBase { XmlAttribute = "cs10", MatchOddAttribute = "CS10", Description = "1-0", DBAttribute = "cs10"},
                    new OddNodeBase { XmlAttribute = "cs20", MatchOddAttribute = "CS20", Description = "2-0", DBAttribute = "cs20"},
                    new OddNodeBase { XmlAttribute = "cs21", MatchOddAttribute = "CS21", Description = "2-1", DBAttribute = "cs21"},
                    new OddNodeBase { XmlAttribute = "cs30", MatchOddAttribute = "CS30", Description = "3-0", DBAttribute = "cs30"},
                    new OddNodeBase { XmlAttribute = "cs31", MatchOddAttribute = "CS31", Description = "3-1", DBAttribute = "cs31"},
                    new OddNodeBase { XmlAttribute = "cs32", MatchOddAttribute = "CS32", Description = "3-2", DBAttribute = "cs32"},
                    new OddNodeBase { XmlAttribute = "cs40", MatchOddAttribute = "CS40", Description = "4-0", DBAttribute = "cs40"},
                    new OddNodeBase { XmlAttribute = "cs41", MatchOddAttribute = "CS41", Description = "4-1", DBAttribute = "cs41"},
                    new OddNodeBase { XmlAttribute = "cs42", MatchOddAttribute = "CS42", Description = "4-2", DBAttribute = "cs42"},
                    new OddNodeBase { XmlAttribute = "cs43", MatchOddAttribute = "CS43", Description = "4-3", DBAttribute = "cs43"},
                    new OddNodeBase { XmlAttribute = "cs01", MatchOddAttribute = "CS01", Description = "0-1", DBAttribute = "cs01"},
                    new OddNodeBase { XmlAttribute = "cs02", MatchOddAttribute = "CS02", Description = "0-2", DBAttribute = "cs02"},
                    new OddNodeBase { XmlAttribute = "cs12", MatchOddAttribute = "CS12", Description = "1-2", DBAttribute = "cs12"},
                    new OddNodeBase { XmlAttribute = "cs03", MatchOddAttribute = "CS03", Description = "0-3", DBAttribute = "cs03"},
                    new OddNodeBase { XmlAttribute = "cs13", MatchOddAttribute = "CS13", Description = "1-3", DBAttribute = "cs13"},
                    new OddNodeBase { XmlAttribute = "cs23", MatchOddAttribute = "CS23", Description = "2-3", DBAttribute = "cs23"},
                    new OddNodeBase { XmlAttribute = "cs04", MatchOddAttribute = "CS04", Description = "0-4", DBAttribute = "cs04"},
                    new OddNodeBase { XmlAttribute = "cs14", MatchOddAttribute = "CS14", Description = "1-4", DBAttribute = "cs14"},
                    new OddNodeBase { XmlAttribute = "cs24", MatchOddAttribute = "CS24", Description = "2-4", DBAttribute = "cs24"},
                    new OddNodeBase { XmlAttribute = "cs34", MatchOddAttribute = "CS34", Description = "3-4", DBAttribute = "cs34"},
                    new OddNodeBase { XmlAttribute = "cs00", MatchOddAttribute = "CS00", Description = "0-0", DBAttribute = "cs00"},
                    new OddNodeBase { XmlAttribute = "cs11", MatchOddAttribute = "CS11", Description = "1-1", DBAttribute = "cs11"},
                    new OddNodeBase { XmlAttribute = "cs22", MatchOddAttribute = "CS22", Description = "2-2", DBAttribute = "cs22"},
                    new OddNodeBase { XmlAttribute = "cs33", MatchOddAttribute = "CS33", Description = "3-3", DBAttribute = "cs33"},
                    new OddNodeBase { XmlAttribute = "cs44", MatchOddAttribute = "CS44", Description = "4-4", DBAttribute = "cs44"},
                    new OddNodeBase { XmlAttribute = "csoas", MatchOddAttribute = "CSOAS", Description = "AOS", DBAttribute = "cs99"}
                }
            },
            new BetTypeNodeBase
            {
                ID = 414,
                Tag = "cs_1",
                Odds = new OddNodeBase[17] {
                    new OddNodeBase { XmlAttribute = "cs10", MatchOddAttribute = "CS10", Description = "1-0", DBAttribute = "cs10"},
                    new OddNodeBase { XmlAttribute = "cs20", MatchOddAttribute = "CS20", Description = "2-0", DBAttribute = "cs20"},
                    new OddNodeBase { XmlAttribute = "cs21", MatchOddAttribute = "CS21", Description = "2-1", DBAttribute = "cs21"},
                    new OddNodeBase { XmlAttribute = "cs30", MatchOddAttribute = "CS30", Description = "3-0", DBAttribute = "cs30"},
                    new OddNodeBase { XmlAttribute = "cs31", MatchOddAttribute = "CS31", Description = "3-1", DBAttribute = "cs31"},
                    new OddNodeBase { XmlAttribute = "cs32", MatchOddAttribute = "CS32", Description = "3-2", DBAttribute = "cs32"},
                    new OddNodeBase { XmlAttribute = "cs01", MatchOddAttribute = "CS01", Description = "0-1", DBAttribute = "cs01"},
                    new OddNodeBase { XmlAttribute = "cs02", MatchOddAttribute = "CS02", Description = "0-2", DBAttribute = "cs02"},
                    new OddNodeBase { XmlAttribute = "cs12", MatchOddAttribute = "CS12", Description = "1-2", DBAttribute = "cs12"},
                    new OddNodeBase { XmlAttribute = "cs03", MatchOddAttribute = "CS03", Description = "0-3", DBAttribute = "cs03"},
                    new OddNodeBase { XmlAttribute = "cs13", MatchOddAttribute = "CS13", Description = "1-3", DBAttribute = "cs13"},
                    new OddNodeBase { XmlAttribute = "cs23", MatchOddAttribute = "CS23", Description = "2-3", DBAttribute = "cs23"},
                    new OddNodeBase { XmlAttribute = "cs00", MatchOddAttribute = "CS00", Description = "0-0", DBAttribute = "cs00"},
                    new OddNodeBase { XmlAttribute = "cs11", MatchOddAttribute = "CS11", Description = "1-1", DBAttribute = "cs11"},
                    new OddNodeBase { XmlAttribute = "cs22", MatchOddAttribute = "CS22", Description = "2-2", DBAttribute = "cs22"},
                    new OddNodeBase { XmlAttribute = "cs33", MatchOddAttribute = "CS33", Description = "3-3", DBAttribute = "cs33"},
                    new OddNodeBase { XmlAttribute = "csoas", MatchOddAttribute = "CSOAS", Description = "AOS", DBAttribute = "cs99"}
                }
            }
        };
    }
}
