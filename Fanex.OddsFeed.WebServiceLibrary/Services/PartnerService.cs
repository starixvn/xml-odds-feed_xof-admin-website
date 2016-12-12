namespace Fanex.OddsFeed.WebServiceLibrary.Services
{
    using System.Text;
    using Common.DTO;
    using Criteria;
    using Helpers;
    using Repositories;

    public class PartnerService : IPartnerService
    {
        private readonly IPartnerRepository _repository;
        private readonly IBetTypeService _betTypeService;

        public PartnerService()
        {
            _repository = new PartnerRepository();
            _betTypeService = new BetTypeService();
        }

        public string GetBetTypesXmlContent(string username, UserProfile userProfile)
        {
            var bettypes = _repository.GetPartnerBetTypes(new GetPartnerBetTypeCriteria { Username = username });
            var enableEnglish = userProfile.Languages.Contains("en-US");
            var enableTraditionalChinese = userProfile.Languages.Contains("zh-TW");
            var enableSimplifiedChinese = userProfile.Languages.Contains("zh-CN");
            var content = new StringBuilder("<bettypes>");

            foreach (var bettype in bettypes)
            {
                content.Append($"<type id=\"{bettype.Id}\" t=\"{_betTypeService.GetBetTypeNameById(bettype.Id)}\"");

                if (enableEnglish)
                {
                    content.Append($" te=\"{bettype.NameEnglish}\"");
                }

                if (enableTraditionalChinese)
                {
                    content.Append($" tt=\"{SecurityHelper.EscapeXML(bettype.NameEnglish)}\"");
                }

                if (enableSimplifiedChinese)
                {
                    content.Append($" ts=\"{SecurityHelper.EscapeXML(bettype.NameTraditionalChinese)}\"");
                }

                content.Append($" gs=\"{SecurityHelper.EscapeXML(bettype.NameSimplifiedChinese)}\" />");
            }

            content.Append("</bettypes>");

            return content.ToString();
        }

        public string GetLeaguesXmlContent(string username, UserProfile userProfile)
        {
            var leagues = _repository.GetPartnerLeagues(new GetPartnerLeagueCriteria { Username = username });
            var enableEnglish = userProfile.Languages.Contains("en-US");
            var enableTraditionalChinese = userProfile.Languages.Contains("zh-TW");
            var enableSimplifiedChinese = userProfile.Languages.Contains("zh-CN");

            var content = new StringBuilder("<leagues>");

            foreach (var league in leagues)
            {
                content.Append($"<league id=\"{league.LeagueId}\" sid=\"{league.SportId}\" se=\"{league.SportName}\"");

                if (enableEnglish)
                {
                    content.Append($" ge=\"{SecurityHelper.EscapeXML(league.NameEnglish)}\"");
                }

                if (enableTraditionalChinese)
                {
                    content.Append($" gt=\"{SecurityHelper.EscapeXML(league.NameTraditionalChinese)}\"");
                }

                if (enableSimplifiedChinese)
                {
                    content.Append($" gs=\"{SecurityHelper.EscapeXML(league.NameSimplifiedChinese)}\"");
                }

                content.Append(" />");
            }

            content.Append("</leagues>");

            return content.ToString();
        }

        public string GetSportsXmlContent(string username, UserProfile userProfile)
        {
            var sports = _repository.GetPartnerSports(new GetPartnerSportTypeCriteria { Username = username });
            var enableEnglish = userProfile.Languages.Contains("en-US");
            var enableTraditionalChinese = userProfile.Languages.Contains("zh-TW");
            var enableSimplifiedChinese = userProfile.Languages.Contains("zh-CN");
            var content = new StringBuilder("<sports>");

            foreach (var sport in sports)
            {
                content.Append($"<sport id=\"{sport.Id}\"");

                if (enableEnglish)
                {
                    content.Append($" se=\"{SecurityHelper.EscapeXML(sport.NameEnglish)}\"");
                }

                if (enableTraditionalChinese)
                {
                    content.Append($" st=\"{SecurityHelper.EscapeXML(sport.NameTraditionalChinese)}\"");
                }

                if (enableSimplifiedChinese)
                {
                    content.Append($" ss=\"{SecurityHelper.EscapeXML(sport.NameSimplifiedChinese)}\"");
                }

                content.Append("/>");
            }

            content.Append("</sports>");

            return content.ToString();
        }
    }
}
