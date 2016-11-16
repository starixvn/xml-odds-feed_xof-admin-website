namespace Fanex.OddsFeed.Library.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.OddsFeed.Library.Nodes;
    using Fanex.OddsFeed.Library.Schemas;
    using Model;

    public class MatchMapping : IMatchMapping
    {
        public IEnumerable<MatchNode> GetMatchNodes(
            DataTable dataTable, 
            IEnumerable<BetTypeSchema> schemas, 
            bool isLive, 
            IEnumerable<OddFlex> oddsFlexs)
        {
            int matchId = 0;
            var matchNodes = new List<MatchNode>();

            foreach (DataRow row in dataTable.Rows)
            {
                var currentMatchId = Convert.ToInt32(row["MatchID"]);
                var matchOdds = GetMatchOdds(schemas, row, oddsFlexs);
                var matchNode = CreateMatchNode(row, isLive);

                if (matchOdds == null)
                {
                    continue;
                }

                if (matchId != currentMatchId)
                {
                    matchNode.BookieNode = new BookieNode
                    {
                        BookieNames = GetBookieNames(),
                        OddsNodes = new List<MatchOddsNode> { matchOdds }
                    };

                    matchNode.MatchId = currentMatchId;

                    matchNodes.Add(matchNode);

                    matchId = currentMatchId;
                }
                else
                {
                    if (matchOdds.TimeStamp > matchNode.TimeStamp)
                    {
                        matchNode.TimeStamp = matchOdds.TimeStamp;
                    }

                    matchNodes.Last().BookieNode.OddsNodes.Add(matchOdds);
                }
            }

            return matchNodes;
        }

        private MatchNode CreateMatchNode(DataRow row, bool isLive)
        {
            return new MatchNode
            {
                SportName = row["SportTypeName"].ToString(),
                SportId = Convert.ToInt32(row["SportType"]),
                KickOfTime = Convert.ToDateTime(row["KickOffTime"].ToString()),
                HomeNames = GetHomeTeams(row),
                AwayNames = GetAwayTeams(row),
                LeagueId = Convert.ToInt32(row["LeagueID"]),
                LeagueNames = GetLeagues(row),
                CountryCode = row["CountryCode"].ToString(),
                BetTypeId = Convert.ToInt32(row["BetType"]),
                IsLive = isLive,
                TimeStamp = Convert.ToInt32(row["TimeStamp"])
            };
        }

        private MatchOddsNode GetMatchOdds(IEnumerable<BetTypeSchema> schemas, DataRow row, IEnumerable<OddFlex> oddsFlexs)
        {
            var schema = FindBetTypeSchema(Convert.ToInt32(row["BetType"]), schemas);

            if (schema == null)
            {
                return null;
            }

            var matchOddsNode = CreateMatchOddsNode(schema, row);

            if (schema.IsOddsFlex)
            {
                matchOddsNode.BetChoices = GetOddsFlexBetChoices(schema, row, oddsFlexs);
            }
            else
            {
                matchOddsNode.BetChoices = GetNormalBetChoices(schema, row);
            }

            return matchOddsNode;
        }

        private IEnumerable<BetChoice> GetNormalBetChoices(BetTypeSchema schema, DataRow row)
        {
            if (schema.Reserve == null)
            {
                return MapBetChoices(schema.BetChoices, row);
            }
            else
            {
                return MapBetChoicesReserve(schema, row);
            }
        }

        private IEnumerable<BetChoice> GetOddsFlexBetChoices(BetTypeSchema schema, DataRow row, IEnumerable<OddFlex> oddsFlexs)
        {
            var filterOddsFlexs = oddsFlexs.Where(odds => odds.OddId == Convert.ToInt32(row["oddsid"]));
            
            return MapBetChoicesOddsFlex(schema.BetChoices, filterOddsFlexs); ;
        }

        private MatchOddsNode CreateMatchOddsNode(BetTypeSchema schema, DataRow row)
        {
            return new MatchOddsNode
            {
                Tag = schema.Tag,
                Time = Convert.ToDateTime(row["ChangeTime"]),
                TimeStamp = Convert.ToInt32(row["TimeStamp"]),
                BetTypeId = Convert.ToInt32(row["BetType"])
            };
        }

        private IEnumerable<BetChoice> MapBetChoicesOddsFlex(IEnumerable<BetChoiceSchema> betChoiceSchemas, IEnumerable<OddFlex> oddsFlexs)
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
                    Value = ConvertMalayToDecimalOddsString(Convert.ToDecimal(value))
                });
            }

            return betChoices;
        }

        private IEnumerable<BetChoice> MapBetChoices(IEnumerable<BetChoiceSchema> betChoiceSchemas, DataRow row)
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

        private IEnumerable<BetChoice> MapBetChoicesReserve(BetTypeSchema betTypeSchema, DataRow row)
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

        private void AssignResourceOdds(IList<BetChoice> betChoices, BetTypeSchema betTypeSchema, DataRow row)
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

        private string GetDataAttribute(BetChoiceSchema betChoiceSchema)
        {
            return string.IsNullOrEmpty(betChoiceSchema.DataAttribute)
                            ? betChoiceSchema.MatchAttribute
                            : betChoiceSchema.DataAttribute;
        }

        private string GetOddsValue(BetChoiceSchema betChoiceSchema, object value)
        {
            if (betChoiceSchema.NotConvert)
            {
                return ConvertOddsToString(Convert.ToDecimal(value));
            }

            return ConvertMalayToDecimalOddsString(Convert.ToDecimal(value));
        }

        private BetTypeSchema FindBetTypeSchema(int betTypeId, IEnumerable<BetTypeSchema> schemas)
        {
            return schemas.Where(schema => schema.BetTypeId == betTypeId).FirstOrDefault();
        }

        private BetChoiceSchema GetResourceBetChoice(IEnumerable<BetChoiceSchema> betChoiceSchemas)
        {
            foreach (var betChoiceSchema in betChoiceSchemas)
            {
                if (betChoiceSchema.IsResource)
                {
                    return betChoiceSchema;
                }
            }

            return null;
        }

        private string ConvertOddsToString(decimal odds)
        {
            return string.Format("{0:0.#########}", odds);
        }

        private string ConvertMalayToDecimalOddsString(decimal odds)
        {
            return string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Convert.ToDecimal(odds)));
        }

        private IList<Language> GetBookieNames()
        {
            return new List<Language>
            {
                new Language
                    {
                        Code = "en-US",
                        Content = string.Empty
                    },
                new Language
                    {
                        Code = "zh-TW",
                        Content = string.Empty
                    },
                new Language
                    {
                        Code = "zh-CN",
                        Content = string.Empty
                    }
            };
        }

        private IList<Language> GetAwayTeams(DataRow row)
        {
            return new List<Language>
            {
                new Language
                    {
                        Code = "en-US",
                        Content = row["Away"].ToString()
                    },
                new Language
                    {
                        Code = "zh-TW",
                        Content = row["AwayCN"].ToString()
                    },
                new Language
                    {
                        Code = "zh-CN",
                        Content = row["AwayC_UNI"].ToString()
                    }
            };
        }

        private IList<Language> GetHomeTeams(DataRow row)
        {
            return new List<Language>
            {
                new Language
                    {
                        Code = "en-US",
                        Content = row["Home"].ToString()
                    },
                new Language
                    {
                        Code = "zh-TW",
                        Content = row["HomeCN"].ToString()
                    },
                new Language
                    {
                        Code = "zh-CN",
                        Content = row["HomeC_UNI"].ToString()
                    }
            };
        }

        private IList<Language> GetLeagues(DataRow row)
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
