namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using System.Collections.Generic;
    using System.Text;
    using Common.DTO;
    using Models;

    public interface IOutrightBuilder
    {
        void AppendOutrightsNode(
            StringBuilder oddsFeedNode,
            IEnumerable<Outright> outrights,
            OddsFeedsModel model,
            UserLanguage userLanguage);
    }
}
