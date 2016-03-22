namespace Nexcel.OddsFeed.Library.Matchs.OddNodeBuilder
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Nexcel.OddsFeed.Library.Matches;
    using Nexcel.OddsFeed.Library.Model;
    using Fanex.Common;

    class OddNodeFlex : OddNodeBase
    {
        public string OddFlexBetChoice { get; set; }

        public void UpdateAttributeForMatchOdd(DataRow item, MatchOdd odd, List<OddFlex> flexOdds)
        {
            var value = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(flexOdds.Where(i => i.BetChoice == OddFlexBetChoice).Select(i => i.Odds).SingleOrDefault())));
            odd.GetType().GetProperty(MatchOddAttribute).SetValue(this, value, null);
        }
    }
}
