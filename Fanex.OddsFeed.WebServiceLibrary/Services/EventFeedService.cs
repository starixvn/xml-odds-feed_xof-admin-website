namespace Fanex.OddsFeed.WebServiceLibrary.Services
{
    using Builders;
    using Caching;
    using Common.DTO;
    using System.Linq;
    using System.Text;
    using Common.Helpers;

    public class EventFeedService : IEventFeedService
    {
        private UserProfile _userProfile;
        private EventBuilder _eventBuilder;
        private readonly IOddsFeedCachingOperator _oddsFeedCachingOperator;

        public EventFeedService(UserProfile userProfile)
        {
            _userProfile = userProfile;
            _eventBuilder = new EventBuilder();
            _oddsFeedCachingOperator = new OddsFeedCachingOperator();
        }

        public string BuildXmlEvent(bool isSample = false, bool isOutright = true)
        {
            var eventFeedNode = new StringBuilder();
            var data = _oddsFeedCachingOperator.GetOddsData(new Models.OddsFeedsModel(), isOutright, true, true);
            //var outrights = _oddsFeedCachingOperator.GetEventOutrights();
            //var deadMatches = _oddsFeedCachingOperator.GetEventMatches(false);
            //var liveMatches = _oddsFeedCachingOperator.GetEventMatches(true);
            var outrights = data.Outrights;
            var fullMatches = data.DeadballMatches.Concat(data.LiveMatches);

            var userLanguage = new UserLanguage
            {
                IsSupportedEnglish = StringHelper.ContainInList(_userProfile.Languages, "en-US"),
                IsSupportedTraditionalChinese = StringHelper.ContainInList(_userProfile.Languages, "zh-TW"),
                IsSupportedSimplifiedChinese = StringHelper.ContainInList(_userProfile.Languages, "zh-CN"),
            };

            eventFeedNode.Append("<eventfeed>");

            if (!isSample || isOutright)
            {
                _eventBuilder.AppendOutrightsNode(eventFeedNode, userLanguage, outrights);
            }

            _eventBuilder.AppendMatchesNode(eventFeedNode, userLanguage, fullMatches);

            eventFeedNode.Append("</eventfeed>");

            return eventFeedNode.ToString();
        }
    }
}