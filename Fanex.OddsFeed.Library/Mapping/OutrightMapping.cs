namespace Fanex.OddsFeed.Library.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Nodes;

    public class OutrightMapping : IOutrightMapping
    {
        public IEnumerable<Outright> GetOutrights(DataTable dataTable)
        {
            var events = new List<Outright>();
            var leagueId = 0;

            foreach (DataRow row in dataTable.Rows)
            {
                var currentLeagueId = Convert.ToInt32(row["LeagueID"]);
                var eventOdds = GetEventOdds(row);

                if (leagueId != currentLeagueId)
                {
                    events.Add(GetEvent(row, currentLeagueId, eventOdds));

                    leagueId = currentLeagueId;
                }
                else
                {
                    events.Last().OutrightOddsList.Add(eventOdds);
                }
            }

            return events;
        }

        private static Outright GetEvent(DataRow row, int leagueId, OutrightOdds eventOdds)
        {
            return new Outright
            {
                EventId = leagueId,
                EventNames = GetEventNames(row),
                SportId = Convert.ToInt32(row["SportID"]),
                SportName = row["LeagueNameTC"].ToString(),
                CountryCode = row["CountryCode"].ToString(),
                Time = Convert.ToDateTime(row["EventDate"].ToString()),
                OutrightOddsList = new List<OutrightOdds> { eventOdds }
            };
        }

        private static OutrightOdds GetEventOdds(DataRow row)
        {
            return new OutrightOdds
            {
                OddsId = Convert.ToInt32(row["OrID"]),
                Names = GetTeams(row),
                Value = Convert.ToDecimal(row["Odds"])
            };
        }

        private static IList<Language> GetTeams(DataRow row)
        {
            return new List<Language>
            {
                new Language
                    {
                        Code = "en-US",
                        Content = row["TeamName"].ToString()
                    },
                new Language
                    {
                        Code = "zh-TW",
                        Content = row["TeamNameCN"].ToString()
                    },
                new Language
                    {
                        Code = "zh-CN",
                        Content = row["TeamNameC_UNI"].ToString()
                    }
            };
        }

        private static IList<Language> GetEventNames(DataRow row)
        {
            return new List<Language>
            {
                new Language
                    {
                        Code = "en-US",
                        Content = row["LeagueName"].ToString()
                    },
                new Language
                    {
                        Code = "zh-TW",
                        Content = row["LeagueNameCN"].ToString()
                    },
                new Language
                    {
                        Code = "zh-CN",
                        Content = row["LeagueNameC_UNI"].ToString()
                    }
            };
        }
    }
}
