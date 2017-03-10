namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using System.Text;
    using Common.DTO;
    using Models;

    public interface IMatchBuilder
    {
        void AppendMatchNodes(
            StringBuilder oddsFeedNode,
            SafeList<Match> matches,
            UserLanguage userLanguage,
            OddsFeedsModel model,
            bool isLive);
    }
}
