using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Configuration.Client;
using Fanex.OddsFeed.Common.DTO;
using Newtonsoft.Json;

namespace CouchbaseApp
{
    public class LeagueDocumentTest
    {
        private readonly string _serverUrl = "http://10.35.35.253:8091/pools";
        private readonly Cluster _cluster;
        private readonly FakeDataV1 _fakeData;
        private readonly string _liveMatchBucket = "livematches";
        private readonly string _deadMatchBucket = "deadmatches";
        private readonly string _outrightBucket = "outrights";
        private readonly string _oddsfeedBucket = "oddsfeed";

        public LeagueDocumentTest()
        {
            var config = new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri(_serverUrl) }
            };

            _cluster = new Cluster(config);
            _fakeData = new FakeDataV1();
        }

        public void Update()
        {
            var deadBalls = ReadData();

            using (var bucket = _cluster.OpenBucket("deadmatches"))
            {
                for (var index = 0; index < 60; index++)
                {
                    var documents = GetLeagueDocuments(deadBalls, index);

                    foreach(var document in documents)
                    {
                        bucket.Upsert(new Document<DeadballDocument> { Id = $"{index}_{document.LeagueId}", Content = document });
                    }
                }
            } 
        }

        public void Update2()
        {
            var deadBalls = ReadData();

            using (var bucket = _cluster.OpenBucket("livematches"))
            {
                for (var index = 0; index < 60; index++)
                {
                    bucket.Upsert(new Document<MatchDocument> { Id = $"{index}", Content = new MatchDocument { Version = index, Matches = deadBalls } });
                }
            }
        }

        public void Update3()
        {
            var deadBalls = ReadData();
        }

        private IList<Deadball> ReadData()
        {
            try
            {
                var fullPath = @"D:\WORKING\Deadballs.txt";

                using (var reader = new StreamReader(fullPath))
                {
                    var temp = reader.ReadToEnd();

                    if (!string.IsNullOrEmpty(temp))
                    {
                        return JsonConvert.DeserializeObject<List<Deadball>>(temp);
                    }
                }
            }
            catch (Exception exception)
            {
            }

            return new List<Deadball>();
        }

        private IEnumerable<DeadballDocument> GetLeagueDocuments(IList<Deadball> matchList, int currentVersion)
        {
            return from match in matchList
                   group match by match.LeagueId into league
                   select new DeadballDocument
                   {
                       Version = currentVersion,
                       LeagueId = league.Key,
                       LeagueEn = league.First().LeagueEn,
                       LeagueCn = league.First().LeagueCn,
                       LeagueTw = league.First().LeagueTw,
                       SportId = league.First().SportId,
                       SportName = league.First().SportName,
                       CountryCode = league.First().CountryCode,
                       Matches = league.ToList()
                   };
        }
    }

    [Serializable]
    public class MatchDocument
    {
        public int Version { get; set; }

        public IList<Deadball> Matches { get; set; }
    }

    [Serializable]
    public class DeadballDocument
    {
        public int Version { get; set; }

        public int LeagueId { get; set; }

        public string LeagueEn { get; set; }

        public string LeagueTw { get; set; }

        public string LeagueCn { get; set; }

        public int SportId { get; set; }

        public string SportName { get; set; }

        public string CountryCode { get; set; }

        public IList<Deadball> Matches { get; set; }
    }

    public class Deadball
    {
        public int MatchId { get; set; }

        public int LeagueId { get; set; }

        //[JsonProperty("LeagueEnglish")]
        public string LeagueEn { get; set; }

        //[JsonProperty("LeagueTraditionalChinese")]
        public string LeagueTw { get; set; }

        //[JsonProperty("LeagueSimplifiedChinese")]
        public string LeagueCn { get; set; }

        //[JsonProperty("HomeEnglish")]
        public string HomeEn { get; set; }

        //[JsonProperty("HomeTraditionalChinese")]
        public string HomeTw { get; set; }

        //[JsonProperty("HomeSimplifiedChinese")]
        public string HomeCn { get; set; }

        //[JsonProperty("AwayEnglish")]
        public string AwayEn { get; set; }

        //[JsonProperty("AwayTraditionalChinese")]
        public string AwayTw { get; set; }

        //[JsonProperty("AwaySimplifiedChinese")]
        public string AwayCn { get; set; }

        public int SportId { get; set; }

        public string SportName { get; set; }

        public string CountryCode { get; set; }

        public DateTime KickOfTime { get; set; }

        public int TimeStamp { get; set; }

        public IList<MatchOdds> MatchOddsList { get; set; }
    }
}
