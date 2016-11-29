namespace Fanex.OddsFeed.Library.Model
{
    using System;

    [Serializable]
    public class Sport
    {
        public int SportId { get; set; }

        public string Name_en { get; set; }

        public string Name_cs { get; set; }

        public string Name_ct { get; set; }
    }
}
