namespace Nexcel.OddsFeed.Library.Matchs.OddNodeBuilder
{
    using Nexcel.OddsFeed.Library.Matches;
    using System;
    using System.Collections.Generic;
    using System.Data;

    class OddNodeFlex : OddNodeBase
    {
        public new OddNodeAttrFlex[] Odds { get; set; }

        public virtual MatchOdd CreateMatchOddBettype(DataRow item, IEnumerable<Model.OddFlex> flexOdds)
        {
            var odd = new MatchOdd
            {
                Type = Tag,
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };

            foreach (OddNodeAttrFlex oddNode in Odds)
            {
                oddNode.UpdateAttributeForMatchOdd(item, odd, flexOdds);
            }

            return odd;
        }
    }
}
