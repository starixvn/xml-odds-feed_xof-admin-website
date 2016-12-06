namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Common.DTO;

    public interface IMatchBuilder
    {
        void AppendMatchesNode(
            StringBuilder oddsFeedNode,
            UserProfile userProfile,
            UserQuery userQuery,
            IList<Match> deadballMatches,
            IList<Match> liveMatches,
            string bookieName,
            DateTime lastUpdate,
            bool isSample);
    }
}
