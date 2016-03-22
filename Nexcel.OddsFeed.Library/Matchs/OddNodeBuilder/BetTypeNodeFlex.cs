namespace Nexcel.OddsFeed.Library.Matchs.OddNodeBuilder
{
    using Nexcel.OddsFeed.Library.Matches;
    using System;
    using System.Collections.Generic;
    using System.Data;

    class BetTypeNodeFlex : BetTypeNodeBase
    {
        public new OddNodeFlex[] Odds { get; set; }

        public virtual MatchOdd CreateMatchOddBettype(DataRow item, List<Model.OddFlex> flexOdds)
        {
            var odd = new MatchOdd
            {
                Type = Tag,
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };

            foreach (OddNodeFlex oddNode in Odds)
            {
                oddNode.UpdateAttributeForMatchOdd(item, odd, flexOdds);
            }

            return odd;
        }
    }
}
