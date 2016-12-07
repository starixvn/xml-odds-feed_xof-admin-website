namespace Fanex.OddsFeed.WebServiceLibrary.Services
{
    using System.Collections.Generic;
    using Common.Helpers;
    using Criteria;
    using Fanex.OddsFeed.Common.Configurations;
    using Newtonsoft.Json;
    using Repositories;

    public class FeedsService : IFeedsService
    {
        private readonly IFeedsRepository _feedsRepository;

        public FeedsService()
        {
            _feedsRepository = new FeedsRepository();
        }

        public string GetFeedsData(
            FormatType formatType,
            string feedName,
            string username,
            string domain,
            string ipAddress,
            int? fromid)
        {
            var feeds = string.Empty;

            switch (feedName.ToLower())
            {
                case "ip":
                    feeds = GetIpFeeds(formatType, username, domain, ipAddress, fromid);
                    break;
                case "team":
                    feeds = GetTeamFeeds(formatType, username, domain, ipAddress, fromid);
                    break;
                case "league":
                    feeds = GetLeagueFeeds(formatType, username, domain, ipAddress, fromid);
                    break;
                case "country":
                    feeds = GetCountryFeeds(formatType, username, domain, ipAddress, fromid);
                    break;
                case "sport":
                    feeds = GetSportFeeds(formatType, username, domain, ipAddress, fromid);
                    break;
                case "event":
                    feeds = GetEventFeeds(formatType, username, domain, ipAddress, fromid);
                    break;
            }

            return feeds;
        }

        private string GetIpFeeds(
            FormatType formatType,
            string username,
            string domain,
            string ipAddress,
            int? fromid)
        {
            var criteria = new GetIpCriteria
            {
                Username = username,
                Domain = domain,
                Ip = ipAddress,
                FromId = fromid
            };

            return SerializeData(formatType, _feedsRepository.GetIpList(criteria));
        }

        private string GetTeamFeeds(
            FormatType formatType,
            string username,
            string domain,
            string ipAddress,
            int? fromid)
        {
            var criteria = new GetTeamCriteria
            {
                Username = username,
                Domain = domain,
                Ip = ipAddress,
                FromId = fromid
            };

            return SerializeData(formatType, _feedsRepository.GetTeamList(criteria));
        }

        private string GetLeagueFeeds(
            FormatType formatType,
            string username,
            string domain,
            string ipAddress,
            int? fromid)
        {
            var criteria = new GetLeagueCriteria
            {
                Username = username,
                Domain = domain,
                Ip = ipAddress,
                FromId = fromid
            };

            return SerializeData(formatType, _feedsRepository.GetLeagueList(criteria));
        }

        private string GetCountryFeeds(
            FormatType formatType,
            string username,
            string domain,
            string ipAddress,
            int? fromid)
        {
            var criteria = new GetCountryCriteria
            {
                Username = username,
                Domain = domain,
                Ip = ipAddress,
                FromId = fromid
            };

            return SerializeData(formatType, _feedsRepository.GetCountryList(criteria));
        }

        private string GetSportFeeds(
            FormatType formatType,
            string username,
            string domain,
            string ipAddress,
            int? fromid)
        {
            var criteria = new GetSportCriteria
            {
                Username = username,
                Domain = domain,
                Ip = ipAddress,
                FromId = fromid
            };

            return SerializeData(formatType, _feedsRepository.GetSportList(criteria));
        }

        private string GetEventFeeds(
            FormatType formatType,
            string username,
            string domain,
            string ipAddress,
            int? fromid)
        {
            var criteria = new GetEventCriteria
            {
                Username = username,
                Domain = domain,
                Ip = ipAddress,
                FromId = fromid
            };

            return SerializeData(formatType, _feedsRepository.GetEventList(criteria));
        }

        private string SerializeData<T>(FormatType formatType, IEnumerable<T> feedsData)
        {
            if (formatType == FormatType.Json)
            {
                return JsonConvert.SerializeObject(feedsData);
            }

            return SerializerHelper.SerializeList(feedsData);
        }
    }
}
