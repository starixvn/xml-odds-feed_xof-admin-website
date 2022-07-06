namespace Fanex.OddsFeed.Core.Model
{
    using System;

    [Serializable]
    public class PermissionFlag
    {
        public short NumberOfIPs { get; set; }

        public bool OddsTypeFlag { get; set; }

        public bool OddsRegionFlag { get; set; }

        public bool LanguageListFlag { get; set; }

        public bool SportIDListFlag { get; set; }

        public bool CountryCodeListFlag { get; set; }

        public bool BetTypeIDListFlag { get; set; }
    }
}