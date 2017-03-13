using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Couchbase;
using Fanex.OddsFeed.Common.DTO;
using Newtonsoft.Json;

namespace CouchbaseApp
{
    public class FakeData
    {
        public Document<MatchDocument> FakeDeadballMatches(int version)
        {
            var matches = BuildMatches();
            return new Document<MatchDocument>
            {
                Id = version.ToString(),
                Content = new MatchDocument { Version = version, Matches = matches }
            };
        }

        public Document<MatchDocument> FakeLiveMatches(int version)
        {
            var matches = BuildMatches();
            return new Document<MatchDocument>
            {
                Id = version.ToString(),
                Content = new MatchDocument { Version = version, Matches = matches }
            };
        }

        public Document<OutrightDocument> FakeOutrights(int version)
        {
            var outrights = BuildOutrights();
            return new Document<OutrightDocument>
            {
                Id = version.ToString(),
                Content = new OutrightDocument { Version = version, Outrights = outrights }
            };
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
            return new List<Outright> { };
        }
    }
}
