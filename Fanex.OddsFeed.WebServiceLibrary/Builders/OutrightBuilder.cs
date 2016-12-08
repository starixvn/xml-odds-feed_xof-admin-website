namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using Common.Converters;
    using Common.DTO;
    using Filters;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class OutrightBuilder : IOutrightBuilder
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
            oddsFeedNode.Append("<outrights>");

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

            oddsFeedNode.Append("</outrights>");
        }

        private static void AppendEventNode(StringBuilder oddsFeedNode, UserProfile userProfile, Outright outright)
        {
            oddsFeedNode.Append($"<event id=\"{outright.EventId}\" ge=\"{outright.EventEnglish}\" gt=\"{outright.EventTraditionalChinese}\" gs=\"{outright.EventSimplifiedChinese}\" mt=\"{outright.Time}\" sport=\"{outright.SportName}\">");

            foreach (var outrightOddsNode in outright.OutrightOddsList)
            {
                AppendOdd(oddsFeedNode, userProfile, outrightOddsNode);
            }

            oddsFeedNode.Append("</event>");
        }

        private static void AppendOdd(
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