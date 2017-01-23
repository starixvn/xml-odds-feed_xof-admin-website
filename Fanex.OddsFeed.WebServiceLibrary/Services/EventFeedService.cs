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

        public EventFeedService(UserProfile userProfile)
        {
            _userProfile = userProfile;
            _eventBuilder = new EventBuilder();
        }

        public string BuildXmlEvent(bool isSample = false, bool isOutright = true)
        {
            var eventFeedNode = new StringBuilder();
            var oddsData = OddFeedDataCachingOperator.GetOddsData(_userProfile.DelayTimes);
            var fullMatches = oddsData.DeadballMatches.Concat(oddsData.LiveMatches);
            var userLanguage = new UserLanguage
            {
                IsSupportedEnglish = StringHelper.ContainInList(_userProfile.Languages, "en-US"),
                IsSupportedTraditionalChinese = StringHelper.ContainInList(_userProfile.Languages, "zh-TW"),
                IsSupportedSimplifiedChinese = StringHelper.ContainInList(_userProfile.Languages, "zh-CN"),
            };

            eventFeedNode.Append("<eventfeed>");

            if (!isSample || isOutright)
            {
                _eventBuilder.AppendOutrightsNode(eventFeedNode, userLanguage, oddsData.Outrights);
            }

            _eventBuilder.AppendMatchesNode(eventFeedNode, userLanguage, fullMatches);

            eventFeedNode.Append("</eventfeed>");

            return eventFeedNode.ToString();
        }
    }
}