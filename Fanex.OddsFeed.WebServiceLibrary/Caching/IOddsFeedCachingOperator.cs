namespace Fanex.OddsFeed.WebServiceLibrary.Caching
{
    using Fanex.OddsFeed.Common.DTO;
    using Models;

    public interface IOddsFeedCachingOperator
    {
        SafeList<Match> GetMatches(OddsFeedsModel model, bool isLive);

        SafeList<Outright> GetOutrights(OddsFeedsModel model);
    }
}
