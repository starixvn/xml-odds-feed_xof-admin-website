namespace Fanex.OddsFeed.WindowsServiceLibrary.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.DTO;

    public class OutrightMapping : IOutrightMapping
    {
        public SafeList<Outright> GetOutrights(DataTable dataTable)
        {
            var events = new SafeList<Outright>();

            var task = Task.Factory.StartNew(() =>
            {
                Parallel.ForEach(dataTable.AsEnumerable(), row =>
                {
                    var eventOdds = GetEventOdds(row);

                    if (eventOdds != null)
                    {
                        AddOutrightOdds(row, events, eventOdds);
                    }
                });
            });

            task.Wait();

            return events;
        }

        private static void AddOutrightOdds(DataRow row, SafeList<Outright> outrights, OutrightOdds outrightOdds)
        {
            var currentLeagueId = Convert.ToInt32(row["LeagueID"]);
            var existedOutright = outrights.FirstOrDefault(o => o.EventId == currentLeagueId);

            if (existedOutright != null)
            {
                existedOutright.OutrightOddsList.Add(outrightOdds);
            }
            else
            {
                outrights.Add(GetOutright(row, currentLeagueId, outrightOdds));
            }
        }

        private static Outright GetOutright(DataRow row, int leagueId, OutrightOdds eventOdds)
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
