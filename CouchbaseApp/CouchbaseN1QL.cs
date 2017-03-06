using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Couchbase;
using Couchbase.Configuration.Client;
using Fanex.OddsFeed.Common.DTO;

namespace CouchbaseApp
{
    public partial class CouchbaseN1QL : Form
    {
        private readonly Cluster Cluster;

        public CouchbaseN1QL()
        {
            InitializeComponent();

            var config = new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri("http://10.35.35.253:8091/pools") }
            };

            Cluster = new Cluster(config);
        }

        private void query_Click(object sender, EventArgs e)
        {
            using (var bucket = Cluster.OpenBucket("oddsfeedTest"))
            {
                //var get = bucket.GetDocument<dynamic>("Test");
                //var document = get.Document;
                //var msg = string.Format("{0} {1}!", document.Id, document.Content.name);
                //MessageBox.Show(msg);

                var result = bucket.Query<dynamic>("SELECT * from oddsfeedTest");
                var data = result.ToList();
            }
        }

        private void store_Click(object sender, EventArgs e)
        {
            using (var bucket = Cluster.OpenBucket("oddsfeedTest"))
            {
                //var document = new Document<dynamic>
                //{
                //    Id = "Test",
                //    Content = new { name = "Couchbase" }
                //};

                //var upsert = bucket.Upsert(document);
                //if (upsert.Success)
                //{
                //    MessageBox.Show("Success");
                //}

                var document = new Document<IList<Match>>
                {
                    Id = "Deadball",
                    Content = GetMatchNodes(false)
                };

                var upsert = bucket.Upsert(document);
                if (upsert.Success)
                {
                    MessageBox.Show("Success");
                }
            }
        }

        private static IList<Match> GetMatchNodes(bool isLive)
        {
            return new List<Match>
            {
                new Match
                {
                    MatchId = 20485706,
                    IsLive = isLive,
                    LeagueId = 10,
                    LeagueEnglish = "UEFA EUROPA LEAGUE",
                    LeagueTraditionalChinese = "UEFA EUROPA LEAGUE",
                    LeagueSimplifiedChinese = "UEFA EUROPA LEAGUE",
                    HomeEnglish = "FC Astana",
                    HomeTraditionalChinese = "FC Astana",
                    HomeSimplifiedChinese = "FC Astana",
                    AwayEnglish = "Olympiakos",
                    AwayTraditionalChinese = "Olympiakos",
                    AwaySimplifiedChinese = "Olympiakos",
                    KickOfTime = new DateTime(2016, 11, 3, 11, 59, 59),
                    SportName = "Soccer",
                    SportId = 5,
                    BookieNameEnglish = "12bet",
                    TimeStamp = 1477878874,
                    MatchOddsList = new []
                    {
                        new MatchOdds
                        {
                            Tag = "oe",
                            BetTypeId = 1,
                            Time = new DateTime(2016, 11, 2, 15, 51, 07),
                            TimeStamp = 1477332693,
                            BetChoices = new[]
                            {
                                new BetChoice { Code = "o", Value = "2.0" },
                                new BetChoice { Code = "e", Value = "1.9" }
                            }
                        },
                        new MatchOdds
                        {
                            Tag = "ou",
                            BetTypeId = 2,
                            Time = new DateTime(2016, 11, 2, 15, 51, 07),
                            TimeStamp = 1477332693,
                            BetChoices = new[]
                            {
                                new BetChoice { Code = "h", Value = "0.5", NotConvert = true },
                                new BetChoice { Code = "o", Value = "1.0" },
                                new BetChoice { Code = "u", Value = "1.4" }
                            }
                        },
                        new MatchOdds
                        {
                            Tag = "ou",
                            BetTypeId = 2,
                            Time = new DateTime(2016, 11, 2, 15, 51, 07),
                            TimeStamp = 1477332693,
                            BetChoices = new[]
                            {
                                new BetChoice { Code = "h", Value = "1", NotConvert = true },
                                new BetChoice { Code = "o", Value = "2.0" },
                                new BetChoice { Code = "u", Value = "4.4" }
                            }
                        },
                        new MatchOdds
                        {
                            Tag = "ah",
                            BetTypeId = 3,
                            Time = new DateTime(2016, 11, 2, 15, 51, 07),
                            TimeStamp = 1477332693,
                            BetChoices = new[]
                            {
                                new BetChoice { Code = "h", Value = "0.5", NotConvert = true },
                                new BetChoice { Code = "o", Value = "1.0" },
                                new BetChoice { Code = "u", Value = "1.4" }
                            }
                        },
                        new MatchOdds
                        {
                            Tag = "ah",
                            BetTypeId = 3,
                            Time = new DateTime(2016, 11, 2, 15, 51, 07),
                            TimeStamp = 1477332693,
                            BetChoices = new[]
                            {
                                new BetChoice { Code = "h", Value = "1", NotConvert = true },
                                new BetChoice { Code = "o", Value = "1.2" },
                                new BetChoice { Code = "u", Value = "1.6" }
                            }
                        },
                        new MatchOdds
                        {
                            Tag = "st",
                            BetTypeId = 4,
                            Time = new DateTime(2016, 11, 2, 15, 51, 07),
                            TimeStamp = 1477332693,
                            BetChoices = new[]
                            {
                                new BetChoice { Code = "o1", Value = "1.0" },
                                new BetChoice { Code = "o2", Value = "1.1" },
                                new BetChoice { Code = "o3", Value = "1.2" }
                            }
                        }
                    }
                },
                new Match
                {
                    MatchId = 20485707,
                    IsLive = isLive,
                    LeagueId = 10,
                    LeagueEnglish = "UEFA EUROPA LEAGUE",
                    LeagueTraditionalChinese = "UEFA EUROPA LEAGUE",
                    LeagueSimplifiedChinese = "UEFA EUROPA LEAGUE",
                    HomeEnglish = "FC Astana 1",
                    HomeTraditionalChinese = "FC Astana 1",
                    HomeSimplifiedChinese = "FC Astana 1",
                    AwayEnglish = "Olympiakos 1",
                    AwayTraditionalChinese = "Olympiakos 1",
                    AwaySimplifiedChinese = "Olympiakos 1",
                    KickOfTime = new DateTime(2016, 11, 3, 11, 59, 59),
                    SportName = "Soccer",
                    SportId = 5,
                    BookieNameEnglish = "12bet",
                    TimeStamp = 1477878874,
                    MatchOddsList = new []
                    {
                        new MatchOdds
                        {
                            Tag = "oe",
                            BetTypeId = 1,
                            Time = new DateTime(2016, 11, 2, 15, 51, 07),
                            TimeStamp = 1477332693,
                            BetChoices = new[]
                            {
                                new BetChoice { Code = "o", Value = "2.0" },
                                new BetChoice { Code = "e", Value = "1.9" }
                            }
                        },
                        new MatchOdds
                        {
                            Tag = "ou",
                            BetTypeId = 2,
                            Time = new DateTime(2016, 11, 2, 15, 51, 07),
                            TimeStamp = 1477332693,
                            BetChoices = new[]
                            {
                                new BetChoice { Code = "h", Value = "0.5", NotConvert = true },
                                new BetChoice { Code = "o", Value = "1.0" },
                                new BetChoice { Code = "u", Value = "1.4" }
                            }
                        },
                        new MatchOdds
                        {
                            Tag = "ou",
                            BetTypeId = 2,
                            Time = new DateTime(2016, 11, 2, 15, 51, 07),
                            TimeStamp = 1477332693,
                            BetChoices = new[]
                            {
                                new BetChoice { Code = "h", Value = "1", NotConvert = true },
                                new BetChoice { Code = "o", Value = "2.0" },
                                new BetChoice { Code = "u", Value = "4.4" }
                            }
                        },
                        new MatchOdds
                        {
                            Tag = "ah",
                            BetTypeId = 3,
                            Time = new DateTime(2016, 11, 2, 15, 51, 07),
                            TimeStamp = 1477332693,
                            BetChoices = new[]
                            {
                                new BetChoice { Code = "h", Value = "0.5", NotConvert = true },
                                new BetChoice { Code = "o", Value = "1.0" },
                                new BetChoice { Code = "u", Value = "1.4" }
                            }
                        },
                        new MatchOdds
                        {
                            Tag = "ah",
                            BetTypeId = 3,
                            Time = new DateTime(2016, 11, 2, 15, 51, 07),
                            TimeStamp = 1477332693,
                            BetChoices = new[]
                            {
                                new BetChoice { Code = "h", Value = "1", NotConvert = true },
                                new BetChoice { Code = "o", Value = "1.2" },
                                new BetChoice { Code = "u", Value = "1.6" }
                            }
                        },
                        new MatchOdds
                        {
                            Tag = "st",
                            BetTypeId = 4,
                            Time = new DateTime(2016, 11, 2, 15, 51, 07),
                            TimeStamp = 1477332693,
                            BetChoices = new[]
                            {
                                new BetChoice { Code = "o1", Value = "1.0" },
                                new BetChoice { Code = "o2", Value = "1.1" },
                                new BetChoice { Code = "o3", Value = "1.2" }
                            }
                        }
                    }
                }
            };
        }
    }
}
