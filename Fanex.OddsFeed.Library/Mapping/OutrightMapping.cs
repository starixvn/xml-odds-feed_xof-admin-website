namespace Fanex.OddsFeed.Library.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Nodes;

    public class OutrightMapping : IOutrightMapping
    {
        public IEnumerable<EventNode> GetEventNodes(DataTable dataTable)
        {
            var eventNodes = new List<EventNode>();
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
                    eventNodes.Last().EventOddsNodes.Add(eventOdds);
                }
            }

            return eventNodes;
        }

        private EventNode GetEventNode(DataRow row, int leagueId, EventOddsNode eventOddsNode)
        {
            return new EventNode
            {
                EventId = leagueId,
                EventNames = GetEventNames(row),
                SportId = Convert.ToInt32(row["SportID"]),
                SportName = row["LeagueNameTC"].ToString(),
                Time = Convert.ToDateTime(row["EventDate"].ToString()),
                EventOddsNodes = new[] { eventOddsNode }
            };
        }

        private EventOddsNode GetEventOdds(DataRow row)
        {
            return new EventOddsNode
            {
                OddsId = Convert.ToInt32(row["OrID"]),
                Names = GetTeams(row),
                Value = row["Odds"].ToString()
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
