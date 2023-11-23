namespace Fanex.OddsFeed.Models
{
    using System;

    [Serializable]
    public class MissingPartnersProfile
    {
        public int ErrCode { get; set; }

        public string AffsOddsType { get; set; } = string.Empty;

        public string AffsLanguageList { get; set; } = string.Empty;

        public string AffsSportIDList { get; set; } = string.Empty;

        public string AffsCountryCodeList { get; set; } = string.Empty;

        public string AffsLeagueIDList { get; set; } = string.Empty;

        public string AffsOutrightIDList { get; set; } = string.Empty;

        public string AffsBettypeIDList { get; set; } = string.Empty;
    }
}