namespace Fanex.OddsFeed.WebServiceLibrary.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Builders;
    using Caching;
    using Common.DTO;
    using Filters;
    using Models;

    public class OddsFeedService : IOddsFeedService
    {
        private const int OddsTypeOutRight = 8;
        private const int OddsTypeLiveMatch = 4;
        private const int OddsTypeDeadBallMatch = 2;

        private readonly IMatchBuilder _matchBuilder;
        private readonly IOutrightBuilder _outrightBuilder;

        private readonly IMatchFilter _matchFilter;
        private readonly IOutrightFilter _outrightFilter;

        public OddsFeedService()
        {
            _matchBuilder = new MatchBuilder();
            _outrightBuilder = new OutrightBuilder();

            _matchFilter = new MatchFilter();
            _outrightFilter = new OutrightFilter();
        }

        public StringBuilder BuildXmlDocument(OddsFeedsModel model)
        {
            var oddsFeedNode = new StringBuilder("<oddsfeed>");
            var oddsData = OddFeedDataCachingOperator.GetOddsData(model.UserProfile.DelayTimes);

            if (IsShownOutright(model))
            {
                AddOutrightNode(oddsFeedNode, oddsData, model);
            }

            AddMatchesNode(oddsFeedNode, oddsData, model);

            oddsFeedNode.Append("</oddsfeed>");

            return oddsFeedNode;
        }

        private static bool IsShownOutright(OddsFeedsModel model)
        {
            return model.UserProfile.OddsType.Contains(OddsTypeOutRight) && (!model.IsSample || model.IsOutright);
        }

        private void AddOutrightNode(
            StringBuilder oddsFeedNode, 
            OddsData oddsData,
            OddsFeedsModel model)
        {
            var outrights = _outrightFilter.QueryOutrights(oddsData.Outrights, model.UserProfile, model.UserQuery);

            if (outrights != null)
            {
                _outrightBuilder.AppendOutrightsNode(oddsFeedNode, outrights, model);
            }
        }

        private void AddMatchesNode(
            StringBuilder oddsFeedNode,
            OddsData oddsData,
            OddsFeedsModel model)
        {
            var changetime = !string.IsNullOrEmpty(model.TimeStamp);
            var timeStamp = changetime ? int.Parse(model.TimeStamp) : 0;

            var liveMatches = GetLiveMatches(oddsData.LiveMatches, model, changetime, timeStamp);
            var deadballMatches = GetDeadMatches(oddsData.DeadballMatches, model, changetime, timeStamp);

            if (liveMatches != null || deadballMatches != null)
            {
                _matchBuilder.AppendMatchesNode(
                    oddsFeedNode,
                    deadballMatches, 
                    liveMatches,
                    model,
                    GetLastUpdate());
            }
        }

        private IList<Match> GetLiveMatches(IList<Match> liveMatches, OddsFeedsModel model, bool changeTime, int timeStamp)
        {
            return HaveLiveBallMatches(liveMatches, model.UserProfile)
                ? _matchFilter.QueryMatches(liveMatches, model.UserProfile, model.UserQuery, changeTime, timeStamp)
                : new List<Match>();
        }

        private static bool HaveLiveBallMatches(IEnumerable<Match> liveMatches, UserProfile userProfile)
        {
            return userProfile.OddsType.Contains(OddsTypeLiveMatch) && liveMatches.Count() > 0;
        }

        private IList<Match> GetDeadMatches(IList<Match> deadMatches, OddsFeedsModel model, bool changeTime, int timeStamp)
        {
            return HaveDeadBallMatches(deadMatches, model.UserProfile)
                ? _matchFilter.QueryMatches(deadMatches, model.UserProfile, model.UserQuery, changeTime, timeStamp)
                : new List<Match>();
        }

        private static bool HaveDeadBallMatches(IEnumerable<Match> deadMatches, UserProfile userProfile)
        {
            return userProfile.OddsType.Contains(OddsTypeDeadBallMatch) && deadMatches.Count() > 0;
        }

        private static DateTime GetLastUpdate()
        {
            return DateTime.Now.AddHours(12);
        }
    }
}
