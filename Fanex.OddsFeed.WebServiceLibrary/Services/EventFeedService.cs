namespace Fanex.OddsFeed.WebServiceLibrary.Services
{
    using Builders;
    using Caching;
    using Common.DTO;
    using System.Linq;
    using System.Text;

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

            eventFeedNode.Append("<eventfeed>");

            if (!isSample || isOutright)
            {
                _eventBuilder.AppendOutrightsNode(eventFeedNode, oddsData.Outrights);
            }

            _eventBuilder.AppendMatchesNode(eventFeedNode, fullMatches);

            eventFeedNode.Append("</eventfeed>");

            return eventFeedNode.ToString();
        }
    }
}