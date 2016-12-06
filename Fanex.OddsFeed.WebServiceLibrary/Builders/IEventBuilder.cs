namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using System.Collections.Generic;
    using System.Text;
    using Common.DTO;

    public interface IEventBuilder
    {
        void AppendOutrightsNode(StringBuilder oddsFeedNode, IEnumerable<Outright> outrights);

        void AppendMatchesNode(StringBuilder oddsFeedNode, IEnumerable<Match> matches);
    }
}
