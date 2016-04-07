namespace Nexcel.OddsFeed.Library.Matchs.OddNodeBuilder
{
    using System;
    using System.Data;
    using Nexcel.OddsFeed.Library.Matches;
    using Fanex.Common;

    class OddNodeHandicap : OddNodeBase
    {
        public string Hdp2 { get; set; }
        public string Hdp1 { get; set; }
        public string Odds2A { get; set; }
        public string Odds1A { get; set; }

        public override MatchOdd CreateMatchOddBettype(DataRow item)
        {
            var h = Converter.ToDecimal(item[Hdp2]) - Converter.ToDecimal(item[Hdp1]);
            var odds2A_7 = Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item[Odds2A]));
            var odds1A_7 = Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item[Odds1A]));
            var odd = new MatchOdd
            {
                Type = Tag,
                H = string.Format("{0:0.#########}", h),
                O = string.Format("{0:0.#########}", h > 0 ? odds2A_7 : odds1A_7),
                U = string.Format("{0:0.#########}", h > 0 ? odds1A_7 : odds2A_7),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };

            foreach (OddNodeAttrBase oddNode in Odds)
            {
                if (oddNode.MatchOddAttribute != "H" && oddNode.MatchOddAttribute != "O" && oddNode.MatchOddAttribute != "U")
                {
                    oddNode.UpdateAttributeForMatchOdd(item, odd);
                }
            }

            return odd;
        }

    }
}
