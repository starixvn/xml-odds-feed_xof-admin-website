namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Common.DTO;
    using Models;

    public interface IMatchBuilder
    {
        void AppendMatchesNode(
            StringBuilder oddsFeedNode,
            IList<Match> deadballMatches,
            IList<Match> liveMatches,
            OddsFeedsModel model,
            DateTime lastUpdate);
    }
}
