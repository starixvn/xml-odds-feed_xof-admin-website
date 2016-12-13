namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using Common.Converters;
    using Common.DTO;
    using Filters;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Models;
    using System.Security;

    public class OutrightBuilder : IOutrightBuilder
    {
        private OutrightFilter _outrightFilter;

        public OutrightBuilder()
        {
            _outrightFilter = new OutrightFilter();
        }

        public void AppendOutrightsNode(
            StringBuilder oddsFeedNode,
            IEnumerable<Outright> outrights,
            OddsFeedsModel model)
        {
            oddsFeedNode.Append("<outrights>");

            if (IsSampleOutright(model.IsOutright, model.IsSample, outrights.Count()))
            {
                AppendEventNode(oddsFeedNode, model.UserProfile, outrights.First());
            }

            if (FilterBetType(model.UserQuery, model.IsSample))
            {
                foreach (var outright in outrights)
                {
                    AppendEventNode(oddsFeedNode, model.UserProfile, outright);
                }
            }

            oddsFeedNode.Append("</outrights>");
        }

        private static void AppendEventNode(StringBuilder oddsFeedNode, UserProfile userProfile, Outright outright)
        {
            oddsFeedNode.Append($"<event id=\"{outright.EventId}\" ge=\"{SecurityElement.Escape(outright.EventEnglish)}\" gt=\"{SecurityElement.Escape(outright.EventTraditionalChinese)}\" gs=\"{SecurityElement.Escape(outright.EventSimplifiedChinese)}\" mt=\"{outright.Time}\" sport=\"{outright.SportName}\">");

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
            oddsFeedNode.AppendFormat($"<odd id=\"{outrightOddsNode.OddsId}\" te=\"{SecurityElement.Escape(outrightOddsNode.OddNameEnglish)}\" tt=\"{SecurityElement.Escape(outrightOddsNode.OddNameTraditionalChinese)}\" ts=\"{SecurityElement.Escape(outrightOddsNode.OddNameSimplifiedChinese)}\" o=\"{OddsConverter.ConvertOdds(userProfile.OddRegion, outrightOddsNode.Value)}\" />");
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