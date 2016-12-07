namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Common.Converters;
    using Common.DTO;
    using Filters;

    public class OutrightBuilder: IOutrightBuilder
    {
        private OutrightFilter _outrightFilter;

        public OutrightBuilder()
        {
            _outrightFilter = new OutrightFilter();
        }

        public void AppendOutrightsNode(
            StringBuilder oddsFeedNode,
            UserProfile userProfile,
            UserQuery userQuery,
            IEnumerable<Outright> outrights,
            bool isOutright,
            bool isSample)
        {
            AppendBeginOutrights(oddsFeedNode);

            if (IsSampleOutright(isOutright, isSample, outrights.Count()))
            {
                AppendEventNode(oddsFeedNode, userProfile, outrights.First());
            }

            if (FilterBetType(userQuery, isSample))
            {
                foreach (var outright in outrights)
                {
                    AppendEventNode(oddsFeedNode, userProfile, outright);
                }
            }

            AppendEndOutrights(oddsFeedNode);
        }

        public void AppendBeginOutrights(StringBuilder oddsFeedNode)
        {
            oddsFeedNode.Append("<outrights>");
        }

        private void AppendEndOutrights(StringBuilder oddsFeedNode)
        {
            oddsFeedNode.Append("</outrights>");
        }

        private void AppendEventNode(StringBuilder oddsFeedNode, UserProfile userProfile, Outright outright)
        {
            AppendBeginEvent(oddsFeedNode, outright);

            foreach (var outrightOddsNode in outright.OutrightOddsList)
            {
                AppendOdd(oddsFeedNode, userProfile, outrightOddsNode);
            }

            AppendEndEvent(oddsFeedNode);
        }

        public void AppendBeginEvent(StringBuilder oddsFeedNode, Outright outright)
        {
            oddsFeedNode.Append($"<event id=\"{outright.EventId}\" ge=\"{outright.EventEnglish}\" gt=\"{outright.EventTraditionalChinese}\" gs=\"{outright.EventSimplifiedChinese}\" mt=\"{outright.Time}\" sport=\"{outright.SportName}\">"); 
        }

        private void AppendEndEvent(StringBuilder oddsFeedNode)
        {
            oddsFeedNode.Append("</event>");
        }

        private void AppendOdd(
            StringBuilder oddsFeedNode,
            UserProfile userProfile,
            OutrightOdds outrightOddsNode)
        {
            oddsFeedNode.AppendFormat($"<odd id=\"{outrightOddsNode.OddsId}\" te=\"{outrightOddsNode.OddNameEnglish}\" tt=\"{outrightOddsNode.OddNameTraditionalChinese}\" ts=\"{outrightOddsNode.OddNameSimplifiedChinese}\" o=\"{OddsConverter.ConvertOdds(userProfile.OddRegion, outrightOddsNode.Value)}\" />");
        }

        private static bool IsSampleOutright(bool isOutright, bool isSample, int numberOutright)
        {
            return isSample && isOutright && numberOutright > 0;
        }

        public static bool FilterBetType(UserQuery userQuery, bool isSample)
        {
            return !isSample
                && (userQuery.BetTypes == null
                || userQuery.BetTypes.Count == 0
                || (userQuery.BetTypes != null
                && userQuery.BetTypes.Contains(10)));
        }
    }
}
