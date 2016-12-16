namespace Fanex.OddsFeed.WindowsServiceLibrary.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.DTO;

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
                EventEnglish = row["LeagueName"].ToString(),
                EventTraditionalChinese = row["LeagueNameCN"].ToString(),
                EventSimplifiedChinese = row["LeagueNameC_UNI"].ToString(),
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
                OddNameEnglish = row["TeamName"].ToString(),
                OddNameTraditionalChinese = row["TeamNameCN"].ToString(),
                OddNameSimplifiedChinese = row["TeamNameC_UNI"].ToString(),
                Value = Convert.ToDecimal(row["Odds"])
            };
        }
    }
}
