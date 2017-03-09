namespace Fanex.OddsFeed.WebServiceLibrary.Services
{
    using System;
    using System.Text;
    using Builders;
    using Caching;
    using Common.DTO;
    using Common.Helpers;
    using Models;

    public class OddsFeedService : IOddsFeedService
    {
        private const int OddsTypeOutRight = 8;

        private readonly IMatchBuilder _matchBuilder;
        private readonly IOutrightBuilder _outrightBuilder;
        private readonly IOddsFeedCachingOperator _oddsFeedCachingOperator;

        public OddsFeedService()
        {
            _matchBuilder = new MatchBuilder();
            _outrightBuilder = new OutrightBuilder();
            _oddsFeedCachingOperator = new OddsFeedCachingOperator();
        }

        public string BuildXmlDocument(OddsFeedsModel model)
        {
            var oddsFeedNode = new StringBuilder("<oddsfeed>");
            var oddsData = _oddsFeedCachingOperator.GetOddsData(model.UserProfile, model.UserQuery);
            var userLanguage = new UserLanguage
            {
                IsSupportedEnglish = StringHelper.ContainInList(model.UserProfile.Languages, "en-US"),
                IsSupportedTraditionalChinese = StringHelper.ContainInList(model.UserProfile.Languages, "zh-TW"),
                IsSupportedSimplifiedChinese = StringHelper.ContainInList(model.UserProfile.Languages, "zh-CN")
            };

            if (IsShownOutright(model))
            {
                _outrightBuilder.AppendOutrightsNode(oddsFeedNode, oddsData.Outrights, model, userLanguage);
            }

            _matchBuilder.AddMatchesNode(oddsFeedNode, oddsData, model, userLanguage, GetLastUpdate());

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
