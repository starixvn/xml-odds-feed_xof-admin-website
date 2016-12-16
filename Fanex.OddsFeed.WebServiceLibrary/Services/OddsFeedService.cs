namespace Fanex.OddsFeed.WebServiceLibrary.Services
{
    using System;
    using System.Text;
    using Builders;
    using Caching;
    using Models;

    public class OddsFeedService : IOddsFeedService
    {
        private const int OddsTypeOutRight = 8;

        private readonly IMatchBuilder _matchBuilder;
        private readonly IOutrightBuilder _outrightBuilder;

        public OddsFeedService()
        {
            _matchBuilder = new MatchBuilder();
            _outrightBuilder = new OutrightBuilder();
        }

        public string BuildXmlDocument(OddsFeedsModel model)
        {
            var oddsFeedNode = new StringBuilder("<oddsfeed>");
            var oddsData = OddFeedDataCachingOperator.GetOddsData(model.UserProfile.DelayTimes);

            if (IsShownOutright(model))
            {
                _outrightBuilder.AppendOutrightsNode(oddsFeedNode, oddsData.Outrights, model);
            }

            _matchBuilder.AddMatchesNode(oddsFeedNode, oddsData, model, GetLastUpdate());

            oddsFeedNode.Append("</oddsfeed>");

            return oddsFeedNode.ToString();
        }

        private static bool IsShownOutright(OddsFeedsModel model)
        {
            return model.UserProfile.OddsType.Contains(OddsTypeOutRight) && (!model.IsSample || model.IsOutright);
        }

        private static DateTime GetLastUpdate()
        {
            return DateTime.Now.AddHours(12);
        }
    }
}
