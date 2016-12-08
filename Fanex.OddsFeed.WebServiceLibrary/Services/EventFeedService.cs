namespace Fanex.OddsFeed.WebServiceLibrary.Services
{
    using System.Linq;
    using System.Text;
    using Builders;
    using Caching;
    using Common.DTO;

    public class EventFeedService : IEventFeedService
    {
        private UserProfile _userProfile;
        private EventBuilder _eventBuilder;

        public EventFeedService(UserProfile userProfile)
        {
            _userProfile = userProfile;
            _eventBuilder = new EventBuilder();
        }

        public StringBuilder BuildXmlEvent(bool isSample = false, bool isOutright = true)
        {
            var eventFeedNode = new StringBuilder();
            var oddsData = OddFeedDataCachingOperator.GetOddsData(_userProfile.DelayTimes);
            var fullMatches = oddsData.DeadballMatches.Concat(oddsData.LiveMatches);

            AppendBeginEventFeed(eventFeedNode);

            if (IsShownOutRightEvent(isSample, isOutright))
            {
                _eventBuilder.AppendOutrightsNode(eventFeedNode, oddsData.Outrights);
            }

            _eventBuilder.AppendMatchesNode(eventFeedNode, fullMatches);

            AppendEndEventFeed(eventFeedNode);

            return eventFeedNode;
        }

        private void AppendBeginEventFeed(StringBuilder eventFeedNode)
        {
            eventFeedNode.Append("<eventfeed>");
        }

        private void AppendEndEventFeed(StringBuilder eventFeedNode)
        {
            eventFeedNode.Append("</eventfeed>");
        }

        private bool IsShownOutRightEvent(bool isSample, bool isOutrights)
        {
            return !isSample || isOutrights;
        }
    }
}
