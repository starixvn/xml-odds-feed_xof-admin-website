namespace Fanex.OddsFeed.Library.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Nodes;

    public class OutrightMapping : IOutrightMapping
    {
        public IEnumerable<Outright> GetOutrightNodes(DataTable dataTable)
        {
            var eventNodes = new List<Outright>();
            int leagueId = 0;

            foreach (DataRow row in dataTable.Rows)
            {
                int currentLeagueId = Convert.ToInt32(row["LeagueID"]);
                var eventOdds = GetEventOdds(row);

                if (leagueId != currentLeagueId)
                {
                    eventNodes.Add(GetEventNode(row, currentLeagueId, eventOdds));

                    leagueId = currentLeagueId;
                }
                else
                {
                    eventNodes.Last().OutrightOddsList.Add(eventOdds);
                }
            }

            return eventNodes;
        }

        private Outright GetEventNode(DataRow row, int leagueId, OutrightOdds eventOddsNode)
        {
            return new Outright
            {
                EventId = leagueId,
                EventNames = GetEventNames(row),
                SportId = Convert.ToInt32(row["SportID"]),
                SportName = row["LeagueNameTC"].ToString(),
                CountryCode = row["CountryCode"].ToString(),
                Time = Convert.ToDateTime(row["EventDate"].ToString()),
                OutrightOddsList = new List<OutrightOdds> { eventOddsNode }
            };
        }

        private OutrightOdds GetEventOdds(DataRow row)
        {
            return new OutrightOdds
            {
                OddsId = Convert.ToInt32(row["OrID"]),
                Names = GetTeams(row),
                Value = Convert.ToDecimal(row["Odds"])
            };
        }

        private IList<Language> GetTeams(DataRow row)
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

        private IList<Language> GetEventNames(DataRow row)
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
