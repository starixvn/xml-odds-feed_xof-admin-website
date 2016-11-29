namespace Fanex.OddsFeed.WebServiceLibrary.Filters
{
    using System.Collections.Generic;
    using Fanex.OddsFeed.Library.Nodes;

    public interface IOutrightFilter
    {
        IList<Outright> QueryOutrights(OddsProfile oddsProfile, OddsFeedFilter oddsFeedFilter, IEnumerable<Outright> outrights);
    }
}
