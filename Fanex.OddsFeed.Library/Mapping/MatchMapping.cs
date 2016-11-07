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
        public IEnumerable<MatchNode> GetMatchNodes()
        {

        }

        public MatchOddsNode GetMatchOdds(IEnumerable<BetTypeSchema> schemas, DataRow row)
        {
            var schema = FindBetTypeSchema(Convert.ToInt32(row["BetTypeID"]), schemas);

            if (schema == null)
            {
                return null;
            }

            var matchOddsNode = CreateMatchOddsNode(schema, row);

            if (schema.Reserve == null)
            {
                matchOddsNode.BetChoices = MapBetChoices(schema.BetChoices, row);
            }
            else
            {
                matchOddsNode.BetChoices = MapBetChoicesReserve(schema, row);
            }

            return matchOddsNode;
        }

        public MatchOddsNode GetMatchOddsByOddsFlex(IEnumerable<BetTypeSchema> schemas, DataRow row, IEnumerable<OddFlex> oddsFlexs)
        {
            var schema = FindBetTypeSchema(Convert.ToInt32(row["BetTypeID"]), schemas);

            if (schema == null)
            {
                return null;
            }

            var matchOddsNode = CreateMatchOddsNode(schema, row);
            var filterOddsFlexs = oddsFlexs.Where(odds => odds.OddId == Convert.ToInt32(row["oddsid"]));

            matchOddsNode.BetChoices = MapBetChoicesOddsFlex(schema.BetChoices, filterOddsFlexs);

            return matchOddsNode;
        }

        private MatchOddsNode CreateMatchOddsNode(BetTypeSchema schema, DataRow row)
        {
            return new MatchOddsNode
            {
                Type = schema.Tag,
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
                    Value = ConvertToMalayOddsString(Convert.ToDecimal(value))
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
                    });
                }
                else
                {
                    betChoices.Add(new BetChoice
                    {
                        Code = betChoiceSchema.MatchAttribute,
                        Value = GetOddsValue(betChoiceSchema, row[dataAttribute]),
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

            var value1 = ConvertToMalayOddsString(Convert.ToDecimal(row[GetDataAttribute(betChoice1)]));
            var value2 = ConvertToMalayOddsString(Convert.ToDecimal(row[GetDataAttribute(betChoice2)]));

            betChoices.Add(new BetChoice
            {
                Code = betTypeSchema.BetChoices.ElementAt(0).MatchAttribute,
                Value = ConvertOddToString(hdp2 - hdp1)
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
                return ConvertOddToString(Convert.ToDecimal(value));
            }

            return ConvertToMalayOddsString(Convert.ToDecimal(value));
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

        private string ConvertOddToString(decimal odds)
        {
            return string.Format("{0:0.#########}", odds);
        }

        private string ConvertToMalayOddsString(decimal odds)
        {
            return string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Convert.ToDecimal(odds)));
        }
    }
}
