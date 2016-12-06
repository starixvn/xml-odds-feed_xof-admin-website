namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using System.Collections.Generic;
    using System.Text;
    using Common.DTO;

    public interface IOutrightBuilder
    {
        void AppendOutrightsNode(
            StringBuilder oddsFeedNode,
            UserProfile userProfile,
            UserQuery userQuery,
            IEnumerable<Outright> outrights,
            bool isOutright,
            bool isSample);
    }
}
