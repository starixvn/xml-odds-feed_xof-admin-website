namespace Fanex.OddsFeed.Core.Model
{
    using System;

    [Serializable]
    public class PartnersProfile
    { 
        public string AffsOddsType { get; set; }

        public string AffsLanguageList { get; set; }

        public string AffsSportIDList { get; set; }

        public string AffsCountryCodeList { get; set; }

        public string AffsLeagueIDList { get; set; }

        public string AffsOutrightIDList { get; set; }

        public string AffsBettypeIDList { get; set; }
    }
}