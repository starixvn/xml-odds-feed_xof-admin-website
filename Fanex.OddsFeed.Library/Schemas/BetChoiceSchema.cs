namespace Fanex.OddsFeed.Library.Schemas
{
    using System;

    [Serializable]
    public class BetChoiceSchema
    {
        public string MatchAttribute { get; set; }

        public string DataAttribute { get; set; }

        public bool NotConvert { get; set; }

        public bool IsResource { get; set; }
    }
}
