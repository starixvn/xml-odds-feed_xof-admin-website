using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Couchbase;
using Fanex.OddsFeed.Common.DTO;
using Newtonsoft.Json;

namespace CouchbaseApp
{
    public class FakeDataV2
    {
        public List<IDocument<LeagueDocument>> FakeDeadMatches(int version)
        {
            var matches = BuildMatches();
            return BuildLeagueDocuments(matches, version);
        }

        public List<IDocument<LeagueDocument>> FakeLiveMatches(int version)
        {
            var matches = BuildMatches();
            return BuildLeagueDocuments(matches, version);
        }

        public List<IDocument<OutrightDocumentV1>> FakeOutrights(int version)
        {
            var outrights = BuildOutrights();

            var result = new List<IDocument<OutrightDocumentV1>>();
            foreach (var outright in outrights)
            {
                var ele = new Document<OutrightDocumentV1>
                {
                    Id = Guid.NewGuid().ToString(),
                    Content = new OutrightDocumentV1
                    {
                        Version = version,
                        EventId = outright.EventId,
                        EventEn = outright.EventEn,
                        EventTw = outright.EventTw,
                        EventCn = outright.EventCn,
                        Time = outright.Time,
                        SportId = outright.SportId,
                        SportName = outright.SportName,
                        CountryCode = outright.CountryCode,
                        OutrightOddsList = outright.OutrightOddsList
                    }
                };

                result.Add(ele);
            }

            return result;
        }

        private List<IDocument<MatchDocumentV1>> BuidMatchDocument(List<Match> matches, int version)
        {
            var result = new List<IDocument<MatchDocumentV1>>();
            foreach (var match in matches)
            {
                var ele = new Document<MatchDocumentV1>
                {
                    Id = Guid.NewGuid().ToString(),
                    Content = new MatchDocumentV1
                    {
                        Version = version,
                        MatchId = match.MatchId,
                        LeagueId = match.LeagueId,
                        LeagueEn = match.LeagueEn,
                        LeagueTw = match.LeagueTw,
                        LeagueCn = match.LeagueCn,
                        HomeEn = match.HomeCn,
                        HomeTw = match.HomeTw,
                        HomeCn = match.HomeCn,
                        AwayEn = match.AwayCn,
                        AwayTw = match.AwayTw,
                        AwayCn = match.AwayCn,
                        SportId = match.SportId,
                        SportName = match.SportName,
                        CountryCode = match.CountryCode,
                        KickOfTime = match.KickOfTime,
                        TimeStamp = match.TimeStamp,
                        MatchOddsList = match.MatchOddsList
                    }
                };
                result.Add(ele);
            }

            return result;
        }

        private static List<IDocument<LeagueDocument>> BuildLeagueDocuments(List<Match> matchList, int currentVersion)
        {
            var leagueDocs = from match in matchList
                             group match by match.LeagueId into league
                             select new LeagueDocument
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

            var result = new List<IDocument<LeagueDocument>>();
            foreach (var leagueDoc in leagueDocs)
            {
                var ele = new Document<LeagueDocument>
                {
                    Id = Guid.NewGuid().ToString(),
                    Content = leagueDoc
                };
                result.Add(ele);
            }

            return result;
        }

        private List<Match> BuildMatches()
        {
            var json = string.Empty;
            var filePath = Application.UserAppDataPath;
            using (StreamReader r = new StreamReader("App_Data/Data/Deadballs.json"))
            {
                json = r.ReadToEnd();
            }
            var matches = JsonConvert.DeserializeObject<List<Match>>(json);

            return matches;
        }

        private List<Outright> BuildOutrights()
        {
            var json = string.Empty;
            var filePath = Application.UserAppDataPath;
            using (StreamReader r = new StreamReader("App_Data/Data/Outright.json"))
            {
                json = r.ReadToEnd();
            }
            var outrights = JsonConvert.DeserializeObject<List<Outright>>(json);

            return outrights;
        }
    }
}
