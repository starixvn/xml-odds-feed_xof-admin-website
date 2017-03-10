namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using System;
    using System.Text;
    using Common.DTO;
    using Models;

    public interface IMatchBuilder
    {
        void AddMatchesNode(
            StringBuilder oddsFeedNode,
            SafeList<Match> matches,
            OddsFeedsModel model,
            UserLanguage userLanguage,
            DateTime lastUpdate);
    }
}
