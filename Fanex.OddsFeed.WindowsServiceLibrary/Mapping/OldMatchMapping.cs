namespace Fanex.OddsFeed.WindowsServiceLibrary.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Converters;
    using Common.DTO;
    using Common.Schemas;

    public class OldMatchMapping : IOldMatchMapping
    {
        public IEnumerable<Match> GetMatches(
            DataTable dataTable,
            IEnumerable<BetTypeSchema> schemas,
            bool isLive,
            IEnumerable<OddsFlex> oddsFlexs)
        {
            var matchId = 0;
            var matches = new List<Match>();

            foreach (DataRow row in dataTable.Rows)
            {
                var currentMatchId = Convert.ToInt32(row["MatchID"]);
                var matchOdds = GetMatchOdds(schemas, row, oddsFlexs);
                var match = CreateMatch(row, isLive);

                if (matchOdds == null)
                {
                    continue;
                }

                if (matchId != currentMatchId)
                {
                    match.MatchId = currentMatchId;
                    match.MatchOddsList = new List<MatchOdds> { matchOdds };

                    matches.Add(match);

                    matchId = currentMatchId;
                }
                else
                {
                    if (matchOdds.TimeStamp > match.TimeStamp)
                    {
                        match.TimeStamp = matchOdds.TimeStamp;
                    }

                    matches.Last().MatchOddsList.Add(matchOdds);
                }
            }

            return matches;
        }

        private static Match CreateMatch(DataRow row, bool isLive)
        {
            return new Match
            {
                SportName = row["SportTypeName"].ToString(),
                SportId = Convert.ToInt32(row["SportType"]),
                KickOfTime = Convert.ToDateTime(row["KickOffTime"].ToString()),
                HomeEnglish = row["Home"].ToString(),
                HomeTraditionalChinese = row["HomeCN"].ToString(),
                HomeSimplifiedChinese = row["HomeC_UNI"].ToString(),
                AwayEnglish = row["Away"].ToString(),
                AwayTraditionalChinese = row["AwayCN"].ToString(),
                AwaySimplifiedChinese = row["AwayC_UNI"].ToString(),
                LeagueId = Convert.ToInt32(row["LeagueID"]),
                LeagueEnglish = row["LeagueName"].ToString(),
                LeagueTraditionalChinese = row["LeagueNameCN"].ToString(),
                LeagueSimplifiedChinese = row["LeagueNameC_UNI"].ToString(),
                CountryCode = row["CountryCode"].ToString(),
                IsLive = isLive,
                TimeStamp = Convert.ToInt32(row["TimeStamp"])
            };
        }

        private static MatchOdds GetMatchOdds(IEnumerable<BetTypeSchema> schemas, DataRow row, IEnumerable<OddsFlex> oddsFlexs)
        {
            var schema = FindBetTypeSchema(Convert.ToInt32(row["BetType"]), schemas);

            if (schema == null)
            {
                return null;
            }

            var matchOdds = CreateMatchOdds(schema, row);

            matchOdds.BetChoices = schema.IsOddsFlex 
                                        ? GetOddsFlexBetChoices(schema, row, oddsFlexs)
                                        : GetNormalBetChoices(schema, row);   

            return matchOdds;
        }

        private static IEnumerable<BetChoice> GetNormalBetChoices(BetTypeSchema schema, DataRow row)
        {
            return schema.Reserve == null ? MapBetChoices(schema.BetChoices, row) : MapBetChoicesReserve(schema, row);
        }

        private static IEnumerable<BetChoice> GetOddsFlexBetChoices(BetTypeSchema schema, DataRow row, IEnumerable<OddsFlex> oddFlexs)
        {
            var filterOddsFlexs = oddFlexs.Where(odds => odds.OddId == Convert.ToInt32(row["oddsid"]));

            return MapBetChoicesOddsFlex(schema.BetChoices, filterOddsFlexs);
        }

        private static IEnumerable<BetChoice> MapBetChoicesOddsFlex(IEnumerable<BetChoiceSchema> betChoiceSchemas, IEnumerable<OddsFlex> oddsFlexs)
        {
            var betChoices = new List<BetChoice>();

            foreach (var betChoiceSchema in betChoiceSchemas)
            {
                var value = oddsFlexs.Where(odds => odds.BetChoice == betChoiceSchema.DataAttribute)
                    .Select(odds => odds.Odds)
                    .FirstOrDefault();

                betChoices.Add(new BetChoice
                {
                    Code = betChoiceSchema.MatchAttribute,
                    Value = ConvertMalayToDecimalOddsString(value)
                });
            }

            return betChoices;
        }

        private static MatchOdds CreateMatchOdds(BetTypeSchema schema, DataRow row)
        {
            return new MatchOdds
            {
                Tag = schema.Tag,
                Time = Convert.ToDateTime(row["ChangeTime"]),
                TimeStamp = Convert.ToInt32(row["TimeStamp"]),
                BetTypeId = Convert.ToInt32(row["BetType"])
            };
        }

        private static IEnumerable<BetChoice> MapBetChoices(IEnumerable<BetChoiceSchema> betChoiceSchemas, DataRow row)
        {
            var betChoices = new List<BetChoice>();

            foreach (var betChoiceSchema in betChoiceSchemas)
            {
                var dataAttribute = GetDataAttribute(betChoiceSchema);

                if (betChoiceSchema.IsResource)
                {
                    betChoices.Add(new BetChoice
                    {
                        Code = betChoiceSchema.MatchAttribute,
                        Value = Convert.ToString(row[dataAttribute]),
                        NotConvert = true
                    });
                }
                else
                {
                    betChoices.Add(new BetChoice
                    {
                        Code = betChoiceSchema.MatchAttribute,
                        Value = GetOddsValue(betChoiceSchema, row[dataAttribute]),
                        NotConvert = betChoiceSchema.NotConvert
                    });
                }
            }

            return betChoices;
        }

        private static IEnumerable<BetChoice> MapBetChoicesReserve(BetTypeSchema betTypeSchema, DataRow row)
        {
            var betChoices = new List<BetChoice>();
            var betChoice1 = betTypeSchema.BetChoices.ElementAt(1);
            var betChoice2 = betTypeSchema.BetChoices.ElementAt(2);

            var hdp1 = Convert.ToDecimal(row[betTypeSchema.Reserve.FirstHdp]);
            var hdp2 = Convert.ToDecimal(row[betTypeSchema.Reserve.SecondHdp]);

            var value1 = ConvertMalayToDecimalOddsString(Convert.ToDecimal(row[GetDataAttribute(betChoice1)]));
            var value2 = ConvertMalayToDecimalOddsString(Convert.ToDecimal(row[GetDataAttribute(betChoice2)]));

            betChoices.Add(new BetChoice
            {
                Code = betTypeSchema.BetChoices.ElementAt(0).MatchAttribute,
                Value = ConvertOddsToString(hdp2 - hdp1),
                NotConvert = true
            });

            if (hdp1 > hdp2)
            {
                betChoices.Add(new BetChoice { Code = betChoice1.MatchAttribute, Value = value1 });
                betChoices.Add(new BetChoice { Code = betChoice2.MatchAttribute, Value = value2 });
            }
            else
            {
                betChoices.Add(new BetChoice { Code = betChoice2.MatchAttribute, Value = value1 });
                betChoices.Add(new BetChoice { Code = betChoice1.MatchAttribute, Value = value2 });
            }

            AssignResourceOdds(betChoices, betTypeSchema, row);

            return betChoices;
        }

        private static void AssignResourceOdds(IList<BetChoice> betChoices, BetTypeSchema betTypeSchema, DataRow row)
        {
            var resourceBetChoice = GetResourceBetChoice(betTypeSchema.BetChoices);

            if (resourceBetChoice != null)
            {
                betChoices.Add(new BetChoice
                {
                    Code = resourceBetChoice.MatchAttribute,
                    Value = Convert.ToString(row["resourceID"]),
                });
            }
        }

        private static string GetDataAttribute(BetChoiceSchema betChoiceSchema)
        {
            return string.IsNullOrEmpty(betChoiceSchema.DataAttribute)
                            ? betChoiceSchema.MatchAttribute
                            : betChoiceSchema.DataAttribute;
        }

        private static string GetOddsValue(BetChoiceSchema betChoiceSchema, object value)
        {
            if (betChoiceSchema.NotConvert)
            {
                return ConvertOddsToString(Convert.ToDecimal(value));
            }

            return ConvertMalayToDecimalOddsString(Convert.ToDecimal(value));
        }

        private static BetTypeSchema FindBetTypeSchema(int betTypeId, IEnumerable<BetTypeSchema> schemas)
        {
            return schemas.FirstOrDefault(schema => schema.BetTypeId == betTypeId);
        }

        private static BetChoiceSchema GetResourceBetChoice(IEnumerable<BetChoiceSchema> betChoiceSchemas)
        {
            return betChoiceSchemas.FirstOrDefault(i => i.IsResource);
        }

        private static string ConvertOddsToString(decimal odds)
        {
            return string.Format("{0:0.#########}", odds);
        }

        private static string ConvertMalayToDecimalOddsString(decimal odds)
        {
            return string.Format("{0:0.#########}", OddsConverter.ToDecimalOddsFromMalayOdds(Convert.ToDecimal(odds)));
        }
    }
}